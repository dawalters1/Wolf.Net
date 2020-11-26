using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Redis;

namespace WOLF.Net.ExampleBot
{

    public class Cache
    {
        public const string Keyword = "example.";

        private RedisCli Redis { get; set; }

        public Cache()
        {
            Redis = new RedisCli();
        }

        public async Task<bool> SetAsync<T>(object key, T value, [Optional] TimeSpan? duration)
        {
            return await Redis.SetAsync($"{Keyword}{key}", value, duration);
        }

        public async Task<bool> DeleteAsync(object key)
        {
            return await Redis.DeleteAsync($"{Keyword}{key}");
        }

        public async Task<bool> ExpireAsync(object key, TimeSpan duration)
        {
            return await Redis.ExpireAsync($"{Keyword}{key}", duration);
        }

        public async Task<T> GetAsync<T>(object key)
        {
            return JsonConvert.DeserializeObject<T>(await Redis.GetAsync($"{Keyword}{key}")) ?? default(T);
        }

        public async Task<bool> ExistsAsync(object key)
        {
            return await Redis.ExistsAsync($"{Keyword}{key}");
        }
    }
}