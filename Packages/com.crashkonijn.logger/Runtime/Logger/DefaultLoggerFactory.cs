using System;
using System.Linq;
using UnityEngine;

namespace CrashKonijn.Logger
{
    public class DefaultLoggerFactory : IRabbitLoggerFactory
    {
        private ILogManager manager;

        public DefaultLoggerFactory(ILogManager manager)
        {
            this.manager = manager;
            this.manager.Bind(this);
        }

        public IRabbitLogger Create(GameObject gameObject, string name, string path = "", ILoggerConfig config = null)
        {
            return new GameObjectLogger(gameObject, config ?? this.manager.GetConfig(), this.manager, path, name);
        }

        public IRabbitLogger Create<T>(GameObject gameObject, string path = "", ILoggerConfig config = null)
        {
            return new GameObjectLogger(gameObject, config ?? this.manager.GetConfig<T>(), this.manager, path, typeof(T).Name);
        }

        public IRabbitLogger Create<T>(string name = null, string path = null, ILoggerConfig config = null)
            where T : class
        {
            return new ClassLogger(typeof(T), config ?? this.manager.GetConfig<T>(), this.manager, path, name ?? nameof(T));
        }

        public IRabbitLogger Create<T>(T instance, string name = null, string path = null, ILoggerConfig config = null)
            where T : class
        {
            if (instance is MonoBehaviour mono)
                return new MonoLogger(mono, config ?? this.manager.GetConfig(mono.GetType()), this.manager, path);

            name ??= GetGenericTypeName(typeof(T));

            return new ClassLogger<T>(instance, config ?? this.manager.GetConfig<T>(), this.manager, path ?? name, name);
        }

        public static string GetGenericTypeName(Type type)
        {
            var typeName = type.Name;

            if (type.IsGenericType)
            {
                var genericArguments = type.GetGenericArguments();
                var genericTypeName = typeName.Substring(0, typeName.IndexOf('`'));
                var typeArgumentNames = string.Join(",", genericArguments.Select(GetGenericTypeName));
                typeName = $"{genericTypeName}<{typeArgumentNames}>";
            }

            return typeName;
        }
    }

    public class LoggerFactory
    {
        public static IRabbitLogger Create(GameObject gameObject, string name, string path = null, ILoggerConfig config = null)
        {
            return LogManager.Instance.LoggerFactory.Create(gameObject, name, path, config);
        }

        public static IRabbitLogger Create<T>(GameObject gameObject, string path = null, ILoggerConfig config = null)
        {
            return LogManager.Instance.LoggerFactory.Create<T>(gameObject, path, config);
        }

        public static IRabbitLogger Create<T>(T instance, string name = null, string path = null, ILoggerConfig config = null)
            where T : class
        {
            return LogManager.Instance.LoggerFactory.Create(instance, name, path, config);
        }

        public static IRabbitLogger Create<T>(string name = null, string path = null, ILoggerConfig config = null)
            where T : class
        {
            return LogManager.Instance.LoggerFactory.Create<T>(name, path, config);
        }
    }

    [Serializable]
    public class LoggerConfig : ILoggerConfig
    {
        [field: SerializeField]
        public DebugSeverity MinimumSeverity { get; set; }

        [field: SerializeField]
        public int MaxLogSize { get; set; }

        [field: SerializeField]
        public bool CanLogToConsole { get; set; } = true;
    }
}
