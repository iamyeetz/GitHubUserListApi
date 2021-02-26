using GitHubUserListApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubUserListApi.Interface
{
    public interface ICacheService
    {

        Task CacheItem(string username,UserInformation user);
        Task<UserInformation>GetUserFromCache(string key);
    }
}
