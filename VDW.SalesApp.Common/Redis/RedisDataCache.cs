using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Newtonsoft.Json;

namespace VDW.SalesApp.Common.Redis
{
    public class RedisDataCache
    {
        private readonly RedisCacheOptions _options;
        private readonly RedisCache _cache;
        public RedisDataCache(string connectionString) {
            _options = new RedisCacheOptions { Configuration = connectionString };
            _cache = new RedisCache(_options);
       }
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            var value = await _cache.GetStringAsync(key);
            if (value != null)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return null;
        }

        public async Task<T> SetAsync<T>(string key,T value) 
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), options);

            return value;
        }
        public async Task<T> SetAsync<T>(string key, T value, int expiredInMinutes, int slidingExpirationInMinutes)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(expiredInMinutes),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiredInMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationInMinutes)
            };
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), options);

            return value;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
            return true;
        }

        public bool TestAsync(string key)
        {
            return true;
        }
    }
}
