using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SpaceCards.API.Options;
using SpaceCards.API.PrivateResolvers;
using StackExchange.Redis;
using System.Text.Json;

namespace SpaceCards.API.Cache
{
    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;
        private readonly RedisOptions _redisOptions;

        public CacheService(IOptions<RedisOptions> options)
        {
            _redisOptions = options.Value;
            if (_redisOptions is null)
            {
                throw new ArgumentNullException($"{nameof(_redisOptions)} cannot be a null.");
            }

            var redis = ConnectionMultiplexer.Connect(_redisOptions.ConnectionString);
            _cacheDb = redis.GetDatabase();
        }

        public async Task<T?> GetData<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            var result = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver()
            });

            return result;
        }

        public async Task<object> RemoveData(string key)
        {
            var isExist = await _cacheDb.KeyExistsAsync(key);
            if (!isExist)
            {
                return false;
            }

            return await _cacheDb.KeyDeleteAsync(key);
        }

        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expTime)
        {
            var exp = expTime.DateTime.Subtract(DateTime.Now);
            return await _cacheDb.StringSetAsync(key, System.Text.Json.JsonSerializer.Serialize(value), exp);
        }
    }
}
