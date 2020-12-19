using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using WOLF.Net.Redis;

namespace WOLF.Net.RTimer
{
    public class RTimer
    {
        ConnectionMultiplexer redis;

        private IDatabase database => redis.GetDatabase();

        private Dictionary<string, Type> Handlers = new Dictionary<string, Type>();

        private Dictionary<string, Timer> Timers = new Dictionary<string, Timer>();

        private void Subscribe(Timer Timer, string handlerName, object data)
        {
            Timer.Elapsed += (a, d) =>
            {
                if (Handlers.ContainsKey(handlerName))
                {
                    var handler = Handlers[handlerName];

                    var i = (ITimeout)Activator.CreateInstance(handler);

                    i.RTimer = this;

                    i.OnTimeout(Timer.data);
                }
            };
        }

        public async Task SubscribeToEvents()
        {
            var events = await database.HashGetAll()
        }
        
        public async Task PostEvent<T>(string key, string handlerName, T data, double duration)
        {
            var Timer = new Timer()
            {
                AutoReset = true,
                Interval = duration
            };

            Subscribe(Timer, handlerName, data);

            await database.set
            Timer.Start();
        }

        public async Task ConfirmEvent(string key)
        {
            if (Timers.ContainsKey($"rtimer.{key}"))
            {
                Timers[$"rtimer.{key}"].Stop();
                Timers.Remove(key);
            }
            await database.KeyDeleteAsync($"rtimer.{key}");
        }

        public async Task CancelEvent(string key)
        {
            await ConfirmEvent(key);
        }

        public async Task ChangeDelay(string key, double duration)
        {
            if (Timers.ContainsKey(key))
            {
                var timer = Timers[key];
                timer.Stop();
                timer.Interval = duration;
                timer.Start();

                await RedisCli.SetAsync($"rtimer.{key}", timer, null);
            }
        }

        public RTimer()
        {
            RedisCli = redisCli;

            var types = (from x in Assembly.GetExecutingAssembly().GetTypes() from z in x.GetInterfaces() let y = x.BaseType where (y != null && y.IsGenericType && typeof(ITimeout).IsAssignableFrom(y.GetGenericTypeDefinition())) || (z.IsGenericType && typeof(ITimeout).IsAssignableFrom(z.GetGenericTypeDefinition())) select x).Where(r => !r.IsAbstract).ToList();

            foreach (var eventHandler in types)
            {
                ITimeout @event = Activator.CreateInstance(eventHandler) as ITimeout;

                Handlers.Add(@event.HandlerName, eventHandler);               
            }
        }
    }
}
