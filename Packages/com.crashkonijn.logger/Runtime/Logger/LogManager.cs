using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CrashKonijn.Logger
{
    public class LogManager : ILogManager
    {
        private static int loggerId = 0;
        private static int logId = 0;
        private TimeHandler timeHandler;
        private readonly ILogManagerConfig config;
        private readonly Dictionary<Type, ILoggerConfig> loggerConfigs = new();
        public IRabbitLoggerFactory LoggerFactory { get; set; }
        public List<IRabbitLogger> Loggers { get; set; } = new();
        public Dictionary<int, IRabbitLogger> LoggersById { get; set; } = new();
        public LogFormatter Formatter { get; set; }

        public static UnityEvent<ILogManager> OnNewManager = new();
        public static UnityEvent<IRabbitLogger> OnLoggerRegistered = new();
        public static UnityEvent<IRabbitLogger> OnLoggerUnregistered = new();
        public static UnityEvent<Log> OnLog = new();
        public static bool Stacktrace { get; set; } = false;

        public static string Guid { get; set; } = System.Guid.NewGuid().ToString();

        private static LogManager instance;

        public static LogManager Instance
        {
            get
            {
                if (instance == null)
                    throw new LoggerException("No LogManager instance found");

                return instance;
            }
            set
            {
                instance?.Dispose();
                instance = value;

                OnNewManager.Invoke(instance);
            }
        }

        public static bool HasInstance => instance != null;

        public LogManager(ILogManagerConfig config)
        {
            if (instance != null)
                throw new LoggerException("LogManager already initialized");

            if (config == null)
                throw new LoggerException("LogManagerConfig is null");

            Instance = this;
            this.timeHandler = new TimeHandler();
            this.Formatter = new LogFormatter(this.timeHandler);

            this.config = config;
        }

        public ILoggerConfig GetConfig(Type type)
        {
            if (this.loggerConfigs.TryGetValue(type, out var config))
                return config;

            config = this.config.GetConfig(type);
            this.loggerConfigs.Add(type, config);

            return config;
        }

        public ILoggerConfig GetConfig<T>()
        {
            return this.GetConfig(typeof(T));
        }

        public ILoggerConfig GetConfig()
        {
            return this.config.GetDefaultConfig();
        }

        public static int GetNextId()
        {
            return loggerId++;
        }

        public void Register(IRabbitLogger logger)
        {
            logger.Id = GetNextId();

            this.Loggers.Add(logger);
            this.LoggersById.Add(logger.Id, logger);

            OnLoggerRegistered.Invoke(logger);
        }

        public void Unregister(IRabbitLogger logger)
        {
            this.Loggers.Remove(logger);
            this.LoggersById.Remove(logger.Id);

            OnLoggerUnregistered.Invoke(logger);
        }

        [HideInCallstack]
        public Log BuildLog(IRabbitLogger logger, string message, DebugSeverity severity, string callerFilePath, int callerLineNumber)
        {
            var log = new Log
            {
                id = logId++,
                owner = logger.Id,
                frame = Time.frameCount,
                message = message,
                severity = severity,
                time = this.timeHandler.GetTime(),
                callerFilePath = callerFilePath,
                callerLineNumber = callerLineNumber,
            };

            OnLog.Invoke(log);

            return log;
        }

        public void Bind(IRabbitLoggerFactory loggerFactory)
        {
            this.LoggerFactory = loggerFactory;

            this.Initialize();
        }

        private void Initialize()
        {
            Application.quitting += this.OnApplicationQuit;

            foreach (var target in this.config.Targets)
            {
                target?.Initialize(this);
            }
        }

        public void Dispose()
        {
            Application.quitting -= this.OnApplicationQuit;

            foreach (var target in this.config.Targets)
            {
                target.Dispose();
            }

            var loggers = new List<IRabbitLogger>(this.Loggers);

            foreach (var logger in loggers)
            {
                logger.Dispose();
            }
        }

        private void OnApplicationQuit()
        {
            Instance = null;
        }

        public static LogManager Create(ILogManagerConfig config)
        {
            var manager = new LogManager(config);
            var factory = new DefaultLoggerFactory(manager);

            return manager;
        }
    }
}
