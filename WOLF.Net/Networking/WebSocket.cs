using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Utils;

namespace WOLF.Net.Networking
{
    public class WebSocket
    {
        private WolfBot _bot { get; set; }

        public SocketIO _socket { get; internal set; }

        public string Host { get; set; } = "https://v3-rc.palringo.com";

        public int Port { get; set; } = 3051;


        public bool Reconnection = true;


        public double ReconnectionDelay = 1000;


        public double ConnectionTimeout = 15000;

        public void On<T>(string eventString, Action<T> action) => _socket.On(eventString, callback => action(callback.GetValue<T>()));

        public async Task<T> Emit<T>(string command, object data = null)
        {
            var tsk = new TaskCompletionSource<T>();

            if (data != null && !data.HasProperty("body") && !data.HasProperty("headers"))
                data = new { body = data };

            _bot.On.Emit(Constants.Internal.PACKET_SENT, command, data);
            try
            {
                await _socket.EmitAsync(
                    command, resp => tsk.SetResult(resp.GetValue<T>()),
                    data);
            }
            catch (Exception d)
            {
                var result = default(T);

                (result as Response).Code = 400;
                (result as Response).Headers = new Dictionary<string, string>()
                {
                    { "message", d.Message }
                };

                tsk.SetResult(result);
            }

            return await tsk.Task;
        }

        internal async Task CreateSocket()
        {
            bool isReconnecting = false;
            _socket = new SocketIO($"{Host}:{Port}", new SocketIOOptions()
            {
                AllowedRetryFirstConnection = true,
                ConnectionTimeout = TimeSpan.FromMilliseconds(ConnectionTimeout),
                Reconnection = Reconnection,
                ReconnectionDelay = (int)ReconnectionDelay,
                Query = new Dictionary<string, string>()
                {
                    ["token"] = _bot.LoginSettings.Token,
                    ["device"] = $"{_bot.LoginSettings.LoginDevice}".ToLower(),
                    ["state"] = ((int)_bot.LoginSettings.OnlineState).ToString()
                },
            });

            _bot._eventHandler.Register();

            _socket.OnConnected += (sender, eventArgs) =>
            {
                if (!isReconnecting)
                    _bot.On.Emit(Constants.Internal.CONNCETED);
                else
                {
                    isReconnecting = false;
                    _bot.On.Emit(Constants.Internal.RECONNECTED);
                }
            };

            _socket.OnDisconnected += (sender, eventArgs) =>
            {
                _bot._cleanUp();

                _bot.On.Emit(Constants.Internal.DISCONNECTED, eventArgs);
            };

            _socket.OnError += (sender, eventArgs) => _bot.On.Emit(Constants.Internal.CONNECTION_ERROR);

            _socket.OnPing += (sender, eventArgs) => _bot.On.Emit(Constants.Internal.PING);
            _socket.OnPong += (sender, EventArgs) => _bot.On.Emit(Constants.Internal.PONG, EventArgs);

            _socket.OnReceivedEvent += (sender, eventArgs) => _bot.On.Emit(Constants.Internal.PACKET_RECEIVED, eventArgs.Event, eventArgs.Response.GetValue<Response<object>>());

            _socket.OnReconnecting += (sender, eventArgs) =>
            {
                isReconnecting = true;
                _bot.On.Emit(Constants.Internal.RECONNECTING);
            };

            _socket.OnReconnectFailed += (sender, eventArgs) => _bot.On.Emit(Constants.Internal.RECONNECT_FAILED, eventArgs);

            _bot.On.Emit(Constants.Internal.CONNECTING);

            await _socket.ConnectAsync();
        }

        public WebSocket(WolfBot bot)
        {
            this._bot = bot;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
    }
}