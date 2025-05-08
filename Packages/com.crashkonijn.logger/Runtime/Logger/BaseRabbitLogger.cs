using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace CrashKonijn.Logger
{
    [Serializable]
    public abstract class BaseRabbitLogger : IRabbitLogger
    {
        public int Id { get; set; }
        private readonly ILogManager manager;
        public ILoggerConfig Config { get; private set; }
        public string Name { get; private set; }
        public string Path { get; set; }

        [field: SerializeField]
        public List<Log> Logs { get; } = new();

        public UnityEvent<Log> OnLog { get; } = new();

        public BaseRabbitLogger(ILoggerConfig config, ILogManager manager, string path, string name)
        {
            this.Config = config;
            this.manager = manager;
            this.Path = path;
            this.Name = name;

            this.manager.Register(this);
        }

        [HideInCallstack]
#if DEBUG
        public void Log(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(message, DebugSeverity.Log, filePath, lineNumber);
        }
#else
        public void Log(string message)
        {
                this.Handle(message, DebugSeverity.Log);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void LogFormat(string format, string arg1, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(string.Format(format, arg1), DebugSeverity.Log, filePath?.ToString(), lineNumber);
        }
#else
        public void LogFormat(string format, string arg1)
        {
                this.Handle(string.Format(format, arg1), DebugSeverity.Log);
        }
#endif

        [HideInCallstack]
#if DEBUG
        public void LogFormat(string format, string arg1, string arg2, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(string.Format(format, arg1, arg2), DebugSeverity.Log, filePath?.ToString(), lineNumber);
        }
#else
        public void LogFormat(string format, string arg1, string arg2)
        {
                this.Handle(string.Format(format, arg1, arg2), DebugSeverity.Log);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void LogFormat(string format, string arg1, string arg2, string arg3, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(string.Format(format, arg1, arg2, arg3), DebugSeverity.Log, filePath?.ToString(), lineNumber);
        }
#else
        public void LogFormat(string format, string arg1, string arg2, string arg3)
        {
                this.Handle(string.Format(format, arg1, arg2, arg3), DebugSeverity.Log);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void Warning(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(message, DebugSeverity.Warning, filePath, lineNumber);
        }
#else
        public void Warning(string message)
        {
                this.Handle(message, DebugSeverity.Warning);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void LogWarning(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(message, DebugSeverity.Warning, filePath, lineNumber);
        }
#else
        public void LogWarning(string message)
        {
                this.Handle(message, DebugSeverity.Warning);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void LogWarningFormat(string format, string arg1, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(string.Format(format, arg1), DebugSeverity.Warning, filePath?.ToString(), lineNumber);
        }
#else
        public void LogWarningFormat(string format, string arg1)
        {
                this.Handle(string.Format(format, arg1), DebugSeverity.Warning);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void LogWarningFormat(string format, string arg1, string arg2, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(string.Format(format, arg1, arg2), DebugSeverity.Warning, filePath?.ToString(), lineNumber);
        }
#else
        public void LogWarningFormat(string format, string arg1, string arg2)
        {
                this.Handle(string.Format(format, arg1, arg2), DebugSeverity.Warning);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void LogWarningFormat(string format, string arg1, string arg2, string arg3, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(string.Format(format, arg1, arg2, arg3), DebugSeverity.Warning, filePath?.ToString(), lineNumber);
        }
#else
        public void LogWarningFormat(string format, string arg1, string arg2, string arg3)
        {
                this.Handle(string.Format(format, arg1, arg2, arg3), DebugSeverity.Warning);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void Error(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(message, DebugSeverity.Warning, filePath, lineNumber);
        }
#else
        public void Error(string message)
        {
                this.Handle(message, DebugSeverity.Warning);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void LogError(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(message, DebugSeverity.Warning, filePath, lineNumber);
        }
#else
        public void LogError(string message)
        {
                this.Handle(message, DebugSeverity.Warning);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void LogErrorFormat(string format, string arg1, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(string.Format(format, arg1), DebugSeverity.Error, filePath?.ToString(), lineNumber);
        }
#else
        public void LogErrorFormat(string format, string arg1)
        {
                this.Handle(string.Format(format, arg1), DebugSeverity.Error);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void LogErrorFormat(string format, string arg1, string arg2, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(string.Format(format, arg1, arg2), DebugSeverity.Error, filePath?.ToString(), lineNumber);
        }
#else
        public void LogErrorFormat(string format, string arg1, string arg2)
        {
                this.Handle(string.Format(format, arg1, arg2), DebugSeverity.Error);
        }
#endif


        [HideInCallstack]
#if DEBUG
        public void LogErrorFormat(string format, string arg1, string arg2, string arg3, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            this.Handle(string.Format(format, arg1, arg2, arg3), DebugSeverity.Error, filePath?.ToString(), lineNumber);
        }
#else
        public void LogErrorFormat(string format, string arg1, string arg2, string arg3)
        {
                this.Handle(string.Format(format, arg1, arg2, arg3), DebugSeverity.Error);
        }
#endif


        [HideInCallstack]
        public bool ShouldLog()
        {
            return this.Config.MinimumSeverity <= DebugSeverity.Exception;
        }

        [HideInCallstack]
        public void Handle(string message, DebugSeverity severity, string compiledFileName = null, int compiledLineNumber = 0)
        {
            var (fileName, lineNumber) = this.GetCallerInfo(compiledFileName, compiledLineNumber);

            if (severity < this.Config.MinimumSeverity)
                return;

            this.Store(message, severity, fileName, lineNumber);
        }

        [HideInCallstack]
        private (string fileName, int lineNumber) GetCallerInfo(string compiledFileName, int compiledLineNumber)
        {
            if (compiledFileName != null)
                return (compiledFileName, compiledLineNumber);

            if (!LogManager.Stacktrace)
                return (string.Empty, 0);

            var stackTrace = new StackTrace(3, true);
            var frame = stackTrace.GetFrame(0);
            var fileName = frame?.GetFileName();
            var lineNumber = frame?.GetFileLineNumber() ?? 0;

            return (fileName, lineNumber);
        }

        [HideInCallstack]
        private void Store(string message, DebugSeverity severity, string callerFilePath, int callerLineNumber)
        {
            if (this.Config.MaxLogSize == 0)
            {
                return;
            }

            if (this.Logs.Count >= this.Config.MaxLogSize)
            {
                this.Logs.RemoveAt(0);
            }

            var log = this.manager.BuildLog(this, message, severity, callerFilePath, callerLineNumber);

            this.Logs.Add(log);

            this.OnLog.Invoke(log);
        }

        public abstract bool ReferencesIntact();

        public void Dispose()
        {
            this.manager.Unregister(this);
        }
    }
}
