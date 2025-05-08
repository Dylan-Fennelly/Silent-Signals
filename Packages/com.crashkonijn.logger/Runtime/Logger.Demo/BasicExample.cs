using UnityEngine;

// Make sure to use the ILogger interface from CrashKonijn.Logger, not UnityEngine.

namespace CrashKonijn.Logger.Demo
{
    public class BasicExample : MonoBehaviour
    {
        private IRabbitLogger logger;

        private void Awake()
        {
            // Create a new logger using the factory.
            this.logger = LoggerFactory.Create<BasicExample>(this);
        }

        private void Start()
        {
            // Log a message with the logger.
            this.logger.Log("Hello, world!");
        }

        private void OnDestroy()
        {
            // Dispose the logger when the object is destroyed.
            this.logger?.Dispose();
        }
    }
}
