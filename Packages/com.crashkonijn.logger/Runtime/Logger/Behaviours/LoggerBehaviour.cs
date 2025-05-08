using UnityEngine;

namespace CrashKonijn.Logger
{
    [RequireComponent(typeof(LoggerFactoryBehaviour))]
    public abstract class LoggerBehaviour : MonoBehaviour, IHasLogger
    {
        [field: SerializeReference]
        public IRabbitLogger Logger { get; set; }

        /// <summary>
        ///     Wrapper to capture Unity's Debug.Log
        /// </summary>
        public IRabbitLogger Debug => this.Logger;

        /// <summary>
        ///     Wrapper to capture Unity's print method
        /// </summary>
        /// <param name="message"></param>
        public new void print(object message)
        {
            this.Debug.Log(message.ToString());
        }
    }
}
