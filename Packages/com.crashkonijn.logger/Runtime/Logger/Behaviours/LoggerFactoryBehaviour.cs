using System.Collections.Generic;
using UnityEngine;

namespace CrashKonijn.Logger
{
    [DefaultExecutionOrder(-98)]
    public class LoggerFactoryBehaviour : MonoBehaviour, IRabbitLoggerFactory
    {
        private List<IRabbitLogger> loggers = new();

        private void Awake()
        {
            var factory = LogManager.Instance.LoggerFactory;
            var behaviours = this.GetComponentsInChildren<LoggerBehaviour>();

            foreach (var behaviour in behaviours)
            {
                var logger = factory.Create(behaviour);
                this.RegisterLogger(logger);
                behaviour.Logger = logger;
            }
        }

        private void RegisterLogger(IRabbitLogger logger)
        {
            this.loggers.Add(logger);
        }

        private void OnDestroy()
        {
            this.loggers.ForEach(logger => logger.Dispose());
        }

        public IRabbitLogger Create(GameObject gameObject, string name, string path = "", ILoggerConfig config = null)
        {
            var logger = LogManager.Instance.LoggerFactory.Create(gameObject, name, path, config);
            this.RegisterLogger(logger);
            return logger;
        }

        public IRabbitLogger Create<T>(GameObject gameObject, string path = "", ILoggerConfig config = null)
        {
            var logger = LogManager.Instance.LoggerFactory.Create<T>(gameObject, path, config);
            this.RegisterLogger(logger);
            return logger;
        }

        public IRabbitLogger Create<T>(string name = null, string path = null, ILoggerConfig config = null) where T : class
        {
            var logger = LogManager.Instance.LoggerFactory.Create<T>(name, path, config);
            this.RegisterLogger(logger);
            return logger;
        }

        public IRabbitLogger Create<T>(T instance, string name = null, string path = null, ILoggerConfig config = null) where T : class
        {
            var logger = LogManager.Instance.LoggerFactory.Create(instance, name, path, config);
            this.RegisterLogger(logger);
            return logger;
        }
    }
}
