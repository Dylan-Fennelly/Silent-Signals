using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrashKonijn.Logger
{
    public interface ILogManager : IDisposable
    {
        public IRabbitLoggerFactory LoggerFactory { get; }
        List<IRabbitLogger> Loggers { get; set; }
        Dictionary<int, IRabbitLogger> LoggersById { get; set; }
        LogFormatter Formatter { get; set; }
        ILoggerConfig GetConfig(Type type);
        ILoggerConfig GetConfig<T>();
        ILoggerConfig GetConfig();
        void Register(IRabbitLogger logger);
        void Unregister(IRabbitLogger logger);

        [HideInCallstack]
        Log BuildLog(IRabbitLogger logger, string message, DebugSeverity severity, string callerFilePath, int callerLineNumber);

        void Bind(IRabbitLoggerFactory loggerFactory);
    }
}
