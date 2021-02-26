using GitHubUserListApi.Interface;
using GitHubUserListApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GitHubUserListApi.Services
{
    public  class GhUserSearchService : IGhUserSearchService
    {


        private ICacheService _cache;
        public GhUserSearchService(ICacheService cache)
        {
            _cache = cache;
        }
        public async Task<UserInformation> GetUserFromGithubAsync(Users name)
        {
            UserInformation userToReturn = new UserInformation();
            var username = name.username;
            try
            {
                userToReturn = await _cache.GetUserFromCache(username);
                if(userToReturn != null)
                { 
                    return userToReturn;
                }
              
            }
            catch (Exception ex)
            {
                //log error            
            }
   
            userToReturn = await ConsumeGitHubApi(username);
            await _cache.CacheItem(username, userToReturn);
             return userToReturn;

            //if (!_cache.TryGetValue(String.Concat("User", name.username), out userToReturn))
            //{
            //    if (userToReturn == null)
            //    {
            //        userToReturn =  await ConsumeGitHubApi(name.username);

            //    }            
            //    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2));
            //    _cache.Set(String.Concat("User", name.username), userToReturn, cacheEntryOptions);


            //}


        }


        private Task<UserInformation> ConsumeGitHubApi(string name)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.github.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("product", "1"));
                var response = client.GetAsync(String.Format("users/{0}", name)).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<UserInformation>(result);
                return Task.FromResult(user);

            }
        }


    }
}

