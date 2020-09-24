using System.IO;
using CachingFramework.Redis.Contracts;

namespace KJ1012.Core.Caching
{
    public class RedisOptions
    {
        public string Configuration { get; set; }
        public ISerializer Serializer { get; set; }
        public TextWriter Log { get; set; }
    }
}
