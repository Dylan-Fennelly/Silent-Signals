using System;
using UnityEngine;

namespace CrashKonijn.Logger
{
    [Serializable]
    public class UnityConsoleTarget : ILogTarget
    {
#pragma warning disable 414
        [SerializeField, HideInInspector]
        private string name = "UnityLogTarget";
#pragma warning restore 414

        [SerializeField, Header("Minimum Severity")]
        private DebugSeverity unityEditor = DebugSeverity.Log;

        [SerializeField]
        private DebugSeverity debug = DebugSeverity.Log;

        [SerializeField]
        private DebugSeverity production = DebugSeverity.Warning;

        private DebugSeverity severity;

        public void Initialize(ILogManager manager)
        {
            this.severity = this.GetSeverity();

            LogManager.OnLog.AddListener(this.OnLog);
        }

        public bool IsValid()
        {
            return true;
        }

        [HideInCallstack]
        private void OnLog(Log log)
        {
            if (log.severity < this.severity)
                return;

            if (!LogManager.Instance.LoggersById.TryGetValue(log.owner, out var logger))
                return;

            if (!logger.Config.CanLogToConsole)
                return;

            switch (log.severity)
            {
                case DebugSeverity.None:
                    break;
                case DebugSeverity.Log:
                    Debug.Log(this.FormatLog(logger, log));
                    break;
                case DebugSeverity.Warning:
                    Debug.LogWarning(this.FormatLog(logger, log));
                    break;
                case DebugSeverity.Error:
                    Debug.LogError(this.FormatLog(logger, log));
                    break;
                case DebugSeverity.Exception:
                    Debug.LogError(this.FormatLog(logger, log));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string FormatLog(IRabbitLogger logger, Log log)
        {
#if UNITY_EDITOR
            return $"[Rabbit::Logger::{log.severity.ToString()}] {log.message}";
#else
            return @$"
[Rabbit::Logger::{log.severity.ToString()}] {log.message}
{logger.Path}|{logger.Name}|{logger.Id}|{log.id}|{log.frame}|{log.time}
";
#endif
        }

        public void Dispose()
        {
            LogManager.OnLog.RemoveListener(this.OnLog);
        }

        private DebugSeverity GetSeverity()
        {
            if (Application.isEditor)
                return this.unityEditor;

            if (Debug.isDebugBuild)
                return this.debug;

            return this.production;
        }
    }
}
