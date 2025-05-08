using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CrashKonijn.Logger.Web
{
    [Serializable]
    public class WebLogTarget : ILogTarget
    {
        [SerializeField, HideInInspector]
        private string name = "WebLogTarget";

        [SerializeField, Header("Connection")]
        private string url = "ws://localhost:5000/unity";

        [SerializeField]
        private string id;

        [SerializeField, Header("Config")]
        private DebugSeverity minimumSeverity = DebugSeverity.Log;

        [SerializeField]
        [Range(0f, 1f)]
        private float sendInterval = 0.3f;

        private ApplicationLogPassthrough passthrough;
        private WebConnection connection;
        private SocketMessage socketMessage;

        public void Initialize(ILogManager manager)
        {
#if RABBIT_LOGGER_WEB
            LogManager.OnLog.AddListener(this.OnLog);
            LogManager.OnLoggerRegistered.AddListener(this.OnLoggerRegistered);
            LogManager.OnLoggerUnregistered.AddListener(this.OnLoggerUnregistered);

            this.socketMessage = new SocketMessage();

            this.passthrough = new ApplicationLogPassthrough(manager);
            this.connection = new WebConnection();

            this.connection.Connect(this.url, string.IsNullOrEmpty(this.id) ? LogManager.Guid : this.id);

            this.Run();
#endif
        }

        public bool IsValid()
        {
#if RABBIT_LOGGER_WEB
            return true;
#else
            return false;
#endif
        }


#if RABBIT_LOGGER_WEB
        private async Task Run()
        {
            while (!this.connection.IsConnected && !this.connection.IsClosed)
            {
                await Task.Delay(TimeSpan.FromSeconds(Time.fixedDeltaTime));
            }

            while (this.connection.IsConnected)
            {
                this.connection.SendWebSocketMessage(this.socketMessage);
                await Task.Delay(TimeSpan.FromSeconds(this.sendInterval));
            }
        }

        private void OnLoggerRegistered(IRabbitLogger logger)
        {
            this.socketMessage.register.Add(new RegisterMessage
            {
                id = logger.Id,
                name = logger.Name,
                path = logger.Path,
                frame = Time.frameCount,
            });
        }

        private void OnLoggerUnregistered(IRabbitLogger logger)
        {
            this.socketMessage.unregister.Add(new UnregisterMessage
            {
                id = logger.Id,
                frame = Time.frameCount,
            });
        }

        private void OnLog(Log log)
        {
            if (log.owner == -1)
            {
                throw new Exception("Log owner not set");
            }

            if (log.severity < this.minimumSeverity)
                return;

            this.socketMessage.logs.Add(log);
        }
#endif

        public void Dispose()
        {
#if RABBIT_LOGGER_WEB
            this.passthrough.Dispose();
            this.connection.Dispose();
#endif
        }
    }
}
