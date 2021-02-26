using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GitHubUserListApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GitHubUserListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGhUserSearchService _ghUserSearchService;
        public UserController(IGhUserSearchService ghUserSearchService)
        {
            _ghUserSearchService = ghUserSearchService;
        }


        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> getGithubUsers([FromBody] List<Users> users)
        {

            if(users.Count > 10)
            {
                return new JsonResult(new ArgumentOutOfRangeException());
            }

            UserInfoRequestResponse userInfoRequestResponse = new UserInfoRequestResponse();



            foreach (var user in users)
            {
                try
                {
                    var data = await _ghUserSearchService.GetUserFromGithubAsync(user);
                    if(data.id != null)
                    {
                        userInfoRequestResponse.UserInformation.Add(data);
                    }
                    else
                    {
                        userInfoRequestResponse.userDoesntExists.Add(user);
                    }
               
             
              
                }
                catch (Exception ex)
                {

                  
                }
        
            }
            userInfoRequestResponse.UserInformation = userInfoRequestResponse.UserInformation.OrderBy(x => x.name).ToList(); ;
            return new JsonResult(userInfoRequestResponse);
        }

       
    }
}