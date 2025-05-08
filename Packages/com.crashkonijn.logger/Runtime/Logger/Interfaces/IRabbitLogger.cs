using System;
using System.Collections.Generic;
using UnityEngine.Events;
#if DEBUG
using System.Runtime.CompilerServices;
#endif

namespace CrashKonijn.Logger
{
    public interface IRabbitLogger : IDisposable
    {
        int Id { get; set; }
        string Name { get; }
        string Path { get; set; }
        List<Log> Logs { get; }
        UnityEvent<Log> OnLog { get; }
        ILoggerConfig Config { get; }
        bool ShouldLog();
        bool ReferencesIntact();
        void Handle(string message, DebugSeverity severity, string compiledFileName = null, int compiledLineNumber = 0);
#if DEBUG
        void Log(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogFormat(string format, string arg1, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogFormat(string format, string arg1, string arg2, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogFormat(string format, string arg1, string arg2, string arg3, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0);
        void Warning(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogWarning(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogWarningFormat(string format, string arg1, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogWarningFormat(string format, string arg1, string arg2, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogWarningFormat(string format, string arg1, string arg2, string arg3, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0);
        void Error(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogError(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogErrorFormat(string format, string arg1, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogErrorFormat(string format, string arg1, string arg2, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0);
        void LogErrorFormat(string format, string arg1, string arg2, string arg3, [CallerFilePath] object filePath = null, [CallerLineNumber] int lineNumber = 0);
#else
        void Log(string message);
        void LogFormat(string format, string arg1);
        void LogFormat(string format, string arg1, string arg2);
        void LogFormat(string format, string arg1, string arg2, string arg3);
        void Warning(string message);
        void LogWarning(string message);
        void LogWarningFormat(string format, string arg1);
        void LogWarningFormat(string format, string arg1, string arg2);
        void LogWarningFormat(string format, string arg1, string arg2, string arg3);
        void Error(string message);
        void LogError(string message);
        void LogErrorFormat(string format, string arg1);
        void LogErrorFormat(string format, string arg1, string arg2);
        void LogErrorFormat(string format, string arg1, string arg2, string arg3);
#endif
    }
}
