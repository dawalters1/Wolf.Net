using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SocketIOClient;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;

namespace WOLF.Net.Client
{
    public class WolfClient
    {
        public WolfBot Bot { get; private set; }

        public SocketIO Socket { get; private set; }

        public string Host { get; set; } = "";

        public int Port { get; set; } = 3051;

        public bool Reconnection = true;

        public double ReconnectionDelay = 1000;

        public double ConnectionTimeout = 15000;

        public WolfClient(WolfBot bot, LoginData loginData)
        {
            Bot = bot;

            Socket = new SocketIO($"{Host}:{Port}", new SocketIOOptions()
            {
                AllowedRetryFirstConnection = true,
                ConnectionTimeout = TimeSpan.FromMilliseconds(ConnectionTimeout),
                Reconnection = true,
                ReconnectionDelay = 1000,
                Query = new Dictionary<string, string>()
                {
                    ["token"] = loginData.Token,
                    ["device"] = loginData.LoginDevice.ToString().ToLower()
                }
            });

            Socket.OnConnected += (sender, eventArgs) => Bot.On.Emit(InternalEvent.CONNCETED);

            Socket.OnDisconnected += (sender, eventArgs) => Bot.On.Emit(InternalEvent.DISCONNECTED);

            Socket.OnError += (sender, eventArgs) => Bot.On.Emit(InternalEvent.CONNECTION_ERROR);

            Socket.OnPing += (sender, eventArgs) => Bot.On.Emit(InternalEvent.PING);

            Socket.OnReceivedEvent += (sender, eventArgs) => Bot.On.Emit(InternalEvent.PACKET_RECEIVED, eventArgs.Event, eventArgs.Response.GetValue<Response<object>>());

            Socket.OnReconnecting += (sender, eventArgs) => Bot.On.Emit(InternalEvent.RECONNECTING);

        }

        public void On<T>(string command, Action<T> action)
        {
            Socket.On(command, callback => action(callback.GetValue<T>()));
        }

        public async Task<Response<T>> Emit<T>(string command, object data)
        {
            Bot.On.Emit(InternalEvent.PACKET_SENT, data);

            var result = new TaskCompletionSource<Response<T>>();

            try
            {
                await Socket.EmitAsync(
                    command,
                    response =>
                    {
                        if (result.Task.IsCompleted)
                            return;

                        try
                        {
                            result.SetResult(response.GetValue<Response<T>>());
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
            Bot.On.Emit(InternalEvent.PACKET_SENT, data);

            var result = new TaskCompletionSource<Response>();

            try
            {
                await Socket.EmitAsync(
                    command,
                    response =>
                    {
                        if (result.Task.IsCompleted)
                            return;

                        try
                        {
                            result.SetResult(response.GetValue<Response>());
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