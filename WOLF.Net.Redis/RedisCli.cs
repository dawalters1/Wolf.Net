using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WOLF.Net.Redis
{
    public class RedisCli
    {
        ConnectionMultiplexer redis;

        private IDatabase database => redis.GetDatabase();

        public RedisCli()
        {
            redis = ConnectionMultiplexer.Connect("localhost");
        }

        public async Task<string> GetAsync(string key)
        {
            return await database.StringGetAsync(key);
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expire)
        {
            return await database.StringSetAsync(key, JsonConvert.SerializeObject(value), expire);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await database.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await database.KeyExistsAsync(key);
        }

        public async Task<bool> SAddAsync<T>(string key, T value)
        {
            return await database.SetAddAsync(key, JsonConvert.SerializeObject(value));
        }

        public async Task<bool> SDeleteAsync<T>(string key, T value)
        {
            return await database.SetRemoveAsync(key, JsonConvert.SerializeObject(value));
        }

        public async Task<List<string>> SMembersAsync(string key)
        {
            return (await database.SetMembersAsync(key)).Select(r => r.ToString()).ToList();
        }

        public async Task<bool> ExpireAsync(string key, TimeSpan expire)
        {
            return await database.KeyExpireAsync(key, expire);
        }
    }
}
