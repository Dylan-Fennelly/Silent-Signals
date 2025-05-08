using UnityEngine;

namespace CrashKonijn.Logger.Demo
{
    public class SerializedExample : MonoBehaviour
    {
        [SerializeReference]
        private IRabbitLogger logger;

        private void Awake()
        {
            // Create a new logger using the factory.
            this.logger = LoggerFactory.Create<SerializedExample>(this, "SomeName");
        }

        private void Start()
        {
            // Log a message with the logger.
            this.logger.Log("Hello, serialized! {0}", "asd");
        }

        private void OnDestroy()
        {
            // Dispose the logger when the object is destroyed.
            this.logger?.Dispose();
        }
    }
}
