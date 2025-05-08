using System;
using UnityEngine;

namespace CrashKonijn.Logger
{
    [Serializable]
    public class LogConfig : ILoggerConfig
    {
        [field: SerializeField, HideInInspector]
        public string Type { get; set; }

        [field: SerializeField]
        public DebugSeverity MinimumSeverity { get; set; }

        [field: SerializeField]
        public int MaxLogSize { get; set; }

        [field: SerializeField]
        public bool CanLogToConsole { get; set; } = true;
    }
}
