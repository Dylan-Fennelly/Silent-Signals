using UnityEngine;

namespace CrashKonijn.Logger
{
    public interface IGameObjectLogger : IRabbitLogger
    {
        GameObject GameObject { get; }
    }
}
