using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace KJ1012.Core.Caching
{
    public static class RedisExtend
    {
        public static async Task<string> GetOrAddStringAsync(this IDistributedCache cache, string key, Func<string> addValue)
        {
            string value = await cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                string addValueString = addValue.Invoke();
                await cache.SetStringAsync(key, addValueString);
                return addValueString;
            }
            return value;
        }
        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> addValue) where T : class
        {
            string value = await cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                {
                    T addValueObj = await addValue.Invoke();
                    if (addValueObj != null)
                    {
                        string json = JsonConvert.SerializeObject(addValueObj);
                        await cache.SetStringAsync(key, json);
                        return addValueObj;
                    }
                    return default(T);
                }
            }
            return JsonConvert.DeserializeObject<T>(value);
        }
        public static async Task<T> AddOrUpdate<T>(this IDistributedCache cache, string key, Func<Task<T>> addValue) where T : class
        {
            string valueString = await cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(valueString))
            {
                await cache.RemoveAsync(key);
            }
            T addValueObj = await addValue.Invoke();
            if (addValueObj != null)
            {
                string json = JsonConvert.SerializeObject(addValueObj);
                await cache.SetStringAsync(key, json);
                return addValueObj;
            }
            return default(T);
        }
    }
}
