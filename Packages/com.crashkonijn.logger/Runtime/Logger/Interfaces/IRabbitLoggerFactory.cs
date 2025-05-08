using UnityEngine;

namespace CrashKonijn.Logger
{
    public interface IRabbitLoggerFactory
    {
        IRabbitLogger Create(GameObject gameObject, string name, string path = "", ILoggerConfig config = null);
        IRabbitLogger Create<T>(GameObject gameObject, string path = "", ILoggerConfig config = null);

        IRabbitLogger Create<T>(string name = null, string path = null, ILoggerConfig config = null)
            where T : class;

        IRabbitLogger Create<T>(T instance, string name = null, string path = null, ILoggerConfig config = null)
            where T : class;
    }
}
