using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using Newtonsoft.Json;
using UnityEngine;

namespace CrashKonijn.Logger
{
    public class WebLoggerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private string url = "ws://localhost:5000/unity";

        [SerializeField]
        private string key = "test";

#if UNITY_EDITOR || DEBUG
        private WebSocket websocket;
        private SocketMessage socketMessage = new();

        private IRabbitLogger Logger { get; set; }
        private bool closed = false;
        private bool isSending = false;

        // Start is called before the first frame update
        private async void Start()
        {
            this.websocket = new WebSocket($"{this.url}/{this.key}");

            this.websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
            };

            this.websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            this.websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");
            };

            this.websocket.OnMessage += (bytes) =>
            {
                // getting the message as a string
                var message = Encoding.UTF8.GetString(bytes);
                Debug.Log("OnMessage! " + message);
            };

            // Keep sending messages at every 0.3s
            this.InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

            // waiting for messages
            await this.websocket.Connect();

            if (this.websocket.State != WebSocketState.Open)
            {
                Debug.Log("Unable to connect to the server");
                this.closed = true;
            }
        }

        private void OnEnable()
        {
            LogManager.OnLog.AddListener(this.OnLog);
            LogManager.OnLoggerRegistered.AddListener(this.OnLoggerRegistered);
            LogManager.OnLoggerUnregistered.AddListener(this.OnLoggerUnregistered);
            LogManager.OnNewManager.AddListener((logManager) =>
            {
                this.Logger?.Dispose();
                this.Logger = logManager.LoggerFactory.Create(this, "console", "system", new LoggerConfig
                {
                    MinimumSeverity = DebugSeverity.Log,
                    MaxLogSize = 1,
                });
            });

            Application.logMessageReceived += this.OnApplicationLogMessageReceived;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= this.OnApplicationLogMessageReceived;
            this.Logger?.Dispose();

            LogManager.OnLog.RemoveListener(this.OnLog);
            LogManager.OnLoggerRegistered.RemoveListener(this.OnLoggerRegistered);
            LogManager.OnLoggerUnregistered.RemoveListener(this.OnLoggerUnregistered);
        }

        private void OnApplicationLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (this.Logger == null)
                return;

            var isLoggerLog = condition.Contains("[logger/");

            if (isLoggerLog)
                return;

            var severity = DebugSeverity.Log;
            switch (type)
            {
                case LogType.Error:
                    severity = DebugSeverity.Error;
                    break;
                case LogType.Assert:
                    severity = DebugSeverity.Error;
                    break;
                case LogType.Warning:
                    severity = DebugSeverity.Warning;
                    break;
                case LogType.Log:
                    severity = DebugSeverity.Log;
                    break;
                case LogType.Exception:
                    severity = DebugSeverity.Error;
                    break;
            }

            this.Logger.Handle(condition, severity);
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
            this.socketMessage.logs.Add(log);
        }

        private async void SendWebSocketMessage()
        {
            if (this.isSending)
                return;

            if (this.websocket.State != WebSocketState.Open)
                return;

            if (this.socketMessage.logs.Count == 0)
                return;

            var send = this.Send(this.socketMessage);

            this.socketMessage.register.Clear();
            this.socketMessage.unregister.Clear();
            this.socketMessage.logs.Clear();

            await send;
        }

        private async Task Send<T>(T message)
        {
            if (this.closed == true)
                return;

            this.isSending = true;
            var json = this.ToJson(message);

            while (this.websocket == null || this.websocket.State == WebSocketState.Connecting)
            {
                await Task.Delay(100);
            }

            // Sending plain text
            await this.websocket.SendText(json);

            this.isSending = false;
        }

        public string ToJson<T>(T message)
        {
            return JsonConvert.SerializeObject(message);
        }

        private async void OnApplicationQuit()
        {
            await this.websocket.Close();
        }
#endif

#if !UNITY_WEBGL && (UNITY_EDITOR || DEBUG)
        private void Update()
        {
            this.websocket.DispatchMessageQueue();
        }
#endif
    }

    public class SocketMessage
    {
        public List<RegisterMessage> register = new();
        public List<UnregisterMessage> unregister = new();
        public List<Log> logs = new();
    }

    public class RegisterMessage
    {
        public string path;
        public int id;
        public string name;
        public int frame;
    }

    public class UnregisterMessage
    {
        public int id;
        public int frame;
    }
}
