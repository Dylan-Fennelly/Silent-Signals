using UnityEngine;

namespace CrashKonijn.Logger
{
    [DefaultExecutionOrder(-99)]
    public class LogManagerInitializer : MonoBehaviour
    {
        [SerializeField]
        private LogManagerConfigScriptable config;

        private void Awake()
        {
            LogManager.Create(this.config);
        }
    }
}
