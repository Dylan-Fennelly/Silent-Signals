using System;
using System.Text;
using System.Threading.Tasks;
#if RABBIT_LOGGER_WEB
using NativeWebSocket;
using Newtonsoft.Json;
#endif
using UnityEngine;

namespace CrashKonijn.Logger.Web
{
    public class WebConnection : IDisposable
    {
#if RABBIT_LOGGER_WEB
        private WebSocket websocket;
        public bool IsClosed { get; set; } = false;
        private bool isSending = false;
        public bool IsConnected => !this.IsClosed && this.websocket != null && this.websocket.State == WebSocketState.Open;

        public async Task Connect(string url, string id)
        {
            this.websocket = new WebSocket($"{url}/{id}");

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

            // waiting for messages
            await this.websocket.Connect();

            if (this.websocket.State != WebSocketState.Open)
            {
                Debug.Log("Unable to connect to the server");
                this.IsClosed = true;
            }
        }
        
        public async void SendWebSocketMessage(SocketMessage message)
        {
            if (this.isSending)
                return;

            if (this.websocket.State != WebSocketState.Open)
                return;

            if (message.logs.Count == 0)
                return;

            var send = this.Send(message);

            message.register.Clear();
            message.unregister.Clear();
            message.logs.Clear();

            await send;
        }

        private async Task Send<T>(T message)
        {
            if (this.IsClosed)
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

        private string ToJson<T>(T message)
        {
            return JsonConvert.SerializeObject(message);
        }
#endif

        public void Dispose()
        {
#if RABBIT_LOGGER_WEB
            this.IsClosed = true;
            this.websocket.Close();
#endif
        }
    }
}
