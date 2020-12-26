
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RTimer
{
    private Dictionary<string, Type> events = new Dictionary<string, Type>();

    private IRedisClientsManagerAsync manager =  new RedisManagerPool("localhost:6379");

    public RTimer()
    {
    }


    public async Task SubscribeToEvents()
    {
        await using var redis = await manager.GetClientAsync();

        await using var cacheSubscription = await redis.CreateSubscriptionAsync();

        cacheSubscription.OnMessageAsync +=  async (channel, msg) =>
        {
            Console.WriteLine(channel);
            Console.WriteLine(msg);
            //FireOnKeyExpired(expiredKey);
        };

        await cacheSubscription.SubscribeToChannelsAsync(new[] { "rtimer.*@0__:expired" });
    }

    public async Task PostEvent<T>(string key, string handler, T data, double duration)
    {
        await using var redis = await manager.GetClientAsync();

        await redis.SetAsync($"rtimer.{key}", new
        {
            handler,
            data
        }, TimeSpan.FromMilliseconds(duration));
    }

    public async Task ConfirmEvent(string key)
    {

    }

    public async Task CancelEvent(string key)
    {

    }

    public async Task ChangeDelay(string key, double duration)
    {

    }
}