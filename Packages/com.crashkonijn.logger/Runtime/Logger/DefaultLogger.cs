using System;
using UnityEngine;

namespace CrashKonijn.Logger
{
    [Serializable]
    public class ClassLogger : BaseRabbitLogger
    {
        public Type Type { get; private set; }

        public ClassLogger(Type type, ILoggerConfig config, ILogManager manager, string path, string name) : base(config, manager, path, name)
        {
            this.Type = type;
        }

        public override bool ReferencesIntact()
        {
            return true;
        }
    }

    public class ClassLogger<T> : ClassLogger
    {
        public T Instance { get; private set; }

        public ClassLogger(T instance, ILoggerConfig config, ILogManager manager, string path, string name) : base(instance.GetType(), config, manager, path, name)
        {
            this.Instance = instance;
        }

        public override bool ReferencesIntact()
        {
            return this.Instance != null;
        }
    }

    [Serializable]
    public class GameObjectLogger : BaseRabbitLogger, IGameObjectLogger
    {
        public GameObject GameObject { get; protected set; }

        public GameObjectLogger(GameObject gameObject, ILoggerConfig config, ILogManager manager, string path) : base(config, manager, path, gameObject.name)
        {
            this.GameObject = gameObject;
        }

        public GameObjectLogger(GameObject gameObject, ILoggerConfig config, ILogManager manager, string path, string name) : base(config, manager, path ?? gameObject.name, name)
        {
            this.GameObject = gameObject;
        }

        public override bool ReferencesIntact()
        {
            return this.GameObject != null;
        }
    }

    [Serializable]
    public class MonoLogger : GameObjectLogger
    {
        public MonoBehaviour MonoBehaviour { get; private set; }

        public MonoLogger(MonoBehaviour mono, ILoggerConfig config, ILogManager manager, string path) : base(mono.gameObject, config, manager, path, mono.GetType().Name)
        {
            this.GameObject = mono.gameObject;
            this.MonoBehaviour = mono;
        }
    }
}
