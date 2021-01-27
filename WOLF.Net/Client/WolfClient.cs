using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SocketIOClient;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Utilities;

namespace WOLF.Net.Client
{
    public class WolfClient
    {
        public WolfBot Bot { get; private set; }

        public SocketIO Socket { get; internal set; }

        public string Host { get; set; } = "https://v3-rc.palringo.com";

        public int Port { get; set; } = 3051;


        public bool Reconnection = true;


        public double ReconnectionDelay = 1000;


        public double ConnectionTimeout = 15000;

        public WolfClient(WolfBot bot)
        {
            Bot = bot;

            //Continue to ignore this, because the socket I am using doesnt care to ignore nulls.
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public async Task CreateSocket()
        {
            bool isReconnecting = false;
            Socket = new SocketIO($"{Host}:{Port}", new SocketIOOptions()
            {
                AllowedRetryFirstConnection = true,
                ConnectionTimeout = TimeSpan.FromMilliseconds(ConnectionTimeout),
                Reconnection = Reconnection,
                ReconnectionDelay = (int)ReconnectionDelay,
                Query = new Dictionary<string, string>()
                {
                    ["token"] = Bot.LoginData.Token,
                    ["device"] = Bot.LoginData.LoginDevice.ToString().ToLower(),
                    ["state"] = ((int)Bot.LoginData.OnlineState).ToString()
                },
            });

            Bot.On.SubscribeToEvents(Bot);

            Socket.OnConnected += (sender, eventArgs) =>
            {
                if (!isReconnecting)
                    Bot.On.Emit(InternalEvent.CONNCETED);
                else
                {
                    isReconnecting = false;
                    Bot.On.Emit(InternalEvent.RECONNECTED);
                }
            };

            Socket.OnDisconnected += (sender, eventArgs) =>
            {
                Bot.Achievements.Clear();
                Bot.Charms.Clear();
                Bot.Subscribers.Clear();
                Bot.Groups.Clear();
                Bot.CurrentSubscriber = default;

                Bot.On.Emit(InternalEvent.DISCONNECTED, eventArgs);
            };

            Socket.OnError += (sender, eventArgs) => Bot.On.Emit(InternalEvent.CONNECTION_ERROR);

            Socket.OnPing += (sender, eventArgs) => Bot.On.Emit(InternalEvent.PING);
            Socket.OnPong += (sender, EventArgs) => Bot.On.Emit(InternalEvent.PONG, EventArgs);

            Socket.OnReceivedEvent += (sender, eventArgs) => Bot.On.Emit(InternalEvent.PACKET_RECEIVED, eventArgs.Event, eventArgs.Response.GetValue<Response<object>>());

            Socket.OnReconnecting += (sender, eventArgs) =>
            {
                isReconnecting = true;
                Bot.On.Emit(InternalEvent.RECONNECTING);
            };

            Socket.OnReconnectFailed += (sender, eventArgs) => Bot.On.Emit(InternalEvent.RECONNECT_FAILED, eventArgs);

            Bot.On.Emit(InternalEvent.CONNECTING);

            await Socket.ConnectAsync();
        }

        public void On<T>(string command, Action<T> action)
        {
            Socket.On(command, callback => action(callback.GetValue<T>()));
        }

        public async Task<Response<T>> Emit<T>(string command, object data)
        {
            if (!data.HasProperty("body") && !data.HasProperty("headers"))
                data = new { body = data };

            Bot.On.Emit(InternalEvent.PACKET_SENT, command, data);

            var result = new TaskCompletionSource<Response<T>>();

            try
            {
                await Socket.EmitAsync(
                    command,
                    resp =>
                    {
                        if (result.Task.IsCompleted)
                            return;

                        try
                        {
                            var response = resp.GetValue<Response<T>>();

                            if (response.Headers != null && response.Headers.ContainsKey("subCode")&&!response.Headers.ContainsKey("message"))                      
                                response.Headers.Add("message", command.ToErrorMessage(int.Parse(response.Headers["subCode"]), response.Headers.ContainsKey("subMessage") ? response.Headers["subMessage"] : ""));

                            result.SetResult(response);
                        }
                        catch (Exception d)
                        {
                            result.SetResult(new Response<T>()
                            {
                                Body = default,
                                Code = 400,
                                Headers = new Dictionary<string, string>()
                                {
                                    { "error", d.ToString() }
                                }

                            });
                        }
                    },
                    data);
            }
            catch (Exception d)
            {
                result.SetResult(new Response<T>()
                {
                    Body = default,
                    Code = 400,
                    Headers = new Dictionary<string, string>()
                    {
                        { "error", d.ToString() }
                    }

                });
            }

            return await result.Task;
        }

        public async Task<Response> Emit(string command, object data)
        {
            if (!data.HasProperty("body") && !data.HasProperty("headers"))
                data = new { body = data };

            Bot.On.Emit(InternalEvent.PACKET_SENT, command, data);

            var result = new TaskCompletionSource<Response>();

            try
            {
                await Socket.EmitAsync(
                    command,
                    resp =>
                    {
                        if (result.Task.IsCompleted)
                            return;

                        try
                        {
                            var response = resp.GetValue<Response>();

                            if (response.Headers != null && response.Headers.ContainsKey("subCode") && !response.Headers.ContainsKey("message"))
                                response.Headers.Add("message", command.ToErrorMessage(int.Parse(response.Headers["subCode"]), response.Headers.ContainsKey("subMessage") ? response.Headers["subMessage"] : ""));

                            result.SetResult(response);
                        }
                        catch (Exception d)
                        {
                            result.SetResult(new Response()
                            {
                                Code = 400,
                                Headers = new Dictionary<string, string>()
                                {
                                    { "error", d.ToString() }
                                }

                            });
                        }
                    },
                    data);
            }
            catch (Exception d)
            {
                result.SetResult(new Response()
                {
                    Code = 400,
                    Headers = new Dictionary<string, string>()
                    {
                        { "error", d.ToString() }
                    }

                });
            }

            return await result.Task;
        }
    }
}