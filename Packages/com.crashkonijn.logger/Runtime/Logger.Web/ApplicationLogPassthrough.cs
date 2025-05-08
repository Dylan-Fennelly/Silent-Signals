using System;
using UnityEngine;

namespace CrashKonijn.Logger.Web
{
    public class ApplicationLogPassthrough : IDisposable
    {
        private IRabbitLogger logger;

        public ApplicationLogPassthrough(ILogManager manager)
        {
            this.logger = manager.LoggerFactory.Create(this, path: "system", config: new LoggerConfig
            {
                MinimumSeverity = DebugSeverity.Log,
                MaxLogSize = 1,
                CanLogToConsole = false,
            });

            Application.logMessageReceived += this.OnApplicationLogMessageReceived;
        }

        public void Dispose()
        {
            Application.logMessageReceived -= this.OnApplicationLogMessageReceived;
            this.logger?.Dispose();
        }

        private void OnApplicationLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (this.logger == null)
                return;

            var isLoggerLog = condition.StartsWith("[Rabbit");

            if (isLoggerLog)
                return;

            var severity = DebugSeverity.Log;
            switch (type)
            {
                case LogType.Error:
                    severity = DebugSeverity.Error;
                    break;
                case LogType.Assert:
                    severity = DebugSeverity.Error;
                    break;
                case LogType.Warning:
                    severity = DebugSeverity.Warning;
                    break;
                case LogType.Log:
                    severity = DebugSeverity.Log;
                    break;
                case LogType.Exception:
                    severity = DebugSeverity.Error;
                    break;
            }

            this.logger.Handle(condition, severity);
        }
    }
}
