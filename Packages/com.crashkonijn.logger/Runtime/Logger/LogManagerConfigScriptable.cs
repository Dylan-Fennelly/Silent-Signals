using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrashKonijn.Logger
{
    [CreateAssetMenu(fileName = "LoggerConfig", menuName = "Logger/LoggerConfigScriptable", order = 0)]
    public class LogManagerConfigScriptable : ScriptableObject, ILogManagerConfig
    {
        [field: SerializeField, Header("Default Config")]
        public DebugSeverity MinimumSeverity { get; set; } = DebugSeverity.Log;

        [field: SerializeField]
        public int MaxLogSize { get; set; } = 30;

        [field: SerializeField, Header("Type Configs")]
        public List<LogConfig> Configs { get; set; }

        [field: SerializeReference]
        public List<ILogTarget> Targets { get; set; } = new();

        public ILoggerConfig GetConfig(Type type)
        {
            var fullName = type.FullName;
            var config = this.Configs.FirstOrDefault(x => x.Type == fullName);

            if (config != null)
                return config;

            var newConfig = new LogConfig
            {
                Type = fullName,
                MinimumSeverity = this.MinimumSeverity,
                MaxLogSize = this.MaxLogSize,
            };

            this.Configs.Add(newConfig);

            return newConfig;
        }

        public ILoggerConfig GetConfig<T>()
        {
            return this.GetConfig(typeof(T));
        }

        public ILoggerConfig GetDefaultConfig()
        {
            return new LogConfig
            {
                Type = "Default",
                MinimumSeverity = this.MinimumSeverity,
                MaxLogSize = this.MaxLogSize,
            };
        }

        public void AddTarget(ILogTarget target)
        {
            this.Targets.Add(target);
        }
    }
}
