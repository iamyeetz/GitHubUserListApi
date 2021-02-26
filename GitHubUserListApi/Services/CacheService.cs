using GitHubUserListApi.Interface;
using GitHubUserListApi.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubUserListApi.Services
{
    public class CacheService : ICacheService
    {
        private IMemoryCache _cache;
        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public  async Task CacheItem(string username,UserInformation user)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2));
            await Task.Run(() => _cache.Set(String.Concat("User", username), user, cacheEntryOptions));
        }

        public  async Task<UserInformation> GetUserFromCache(string name)
        {
            return await  Task.FromResult((UserInformation)_cache.Get(String.Concat("User", name)));
        }
    }
}
