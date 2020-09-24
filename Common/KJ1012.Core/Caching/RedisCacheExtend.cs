using System;
using CachingFramework.Redis;
using CachingFramework.Redis.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.Core.Caching
{
    public static class RedisCacheExtend
    {
        public static IServiceCollection AddRedis(this IServiceCollection serviceCollection, Action<RedisOptions> options)
        {
            RedisOptions redisOptions=new RedisOptions();
            options.Invoke(redisOptions);
            //if(redisOptions.Serializer==null) redisOptions.Serializer=new JsonSerializer();
            RedisContext context = new RedisContext(redisOptions.Configuration);
            serviceCollection.AddSingleton<IContext>(context);
            serviceCollection.AddSingleton(context.Cache);
            serviceCollection.AddSingleton(context.Collections);
            return serviceCollection;
        }
    }
}
