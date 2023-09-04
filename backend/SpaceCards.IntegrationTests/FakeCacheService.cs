using SpaceCards.API.Cache;
using System;
using System.Threading.Tasks;

namespace SpaceCards.IntegrationTests
{
    public class FakeCacheService : ICacheService
    {
        public async Task<T> GetData<T>(string key)
        {
            return default;
        }

        public async Task<object> RemoveData(string key)
        {
            return true;
        }

        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expTime)
        {
            return true;
        }
    }
}
