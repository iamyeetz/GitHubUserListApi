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
        public async Task<IActionResult> GetGithubUsers([FromBody] List<Users> users)
        {

            if(users.Count > 10)
            {
                return new JsonResult(new ArgumentOutOfRangeException());
            }

            UserInfoRequestResponse userInfoRequestResponse = new UserInfoRequestResponse();
            var data = await _ghUserSearchService.GetUserFromGithubAsync(users);          
            userInfoRequestResponse.UserInformation = data.UserInformation.OrderBy(x => x.name).ToList(); 
            userInfoRequestResponse.userDoesntExists = data.userDoesntExists.OrderBy(x => x.username).ToList();

            return new JsonResult(userInfoRequestResponse);
        }

       
    }
}