using GitHubUserListApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using GitHubUserListApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GitHubUserListApi.Interface;

namespace GitHubUserListApi.Test
{
    public class UserControllerTest
    {


        [Fact]
        public void TestSuccessfullProcessByPassingValidUsernames()
        {

            List<Users> users = new List<Users>();
            Users toAdd = new Users {
                username = "iamyeetz"
            };
            users.Add(toAdd);
            Users toAdd1 = new Users
            {
                username = "jessica"
            };
            users.Add(toAdd);


            //var sut = new Mock<UserController>(new Mock<IGhUserSearchService>());
            //sut.Setup(x => x.getGithubUsers(users)).Returns(new JsonResult());

            var mockGhUserSearchService = new Mock<IGhUserSearchService>();
            mockGhUserSearchService.Setup(repo => repo.GetUserFromGithubAsync(toAdd)).Returns(GetTestSessions(true));

            var sut = new UserController(mockGhUserSearchService.Object);

            var result = sut.getGithubUsers(users);
            var _result = Assert.IsType<JsonResult>(result.Result);
            var model = Assert.IsAssignableFrom<UserInfoRequestResponse>((_result.Value));
            Assert.Equal(2, model.UserInformation.Count);
        }
        private Task<UserInformation> GetTestSessions(bool isValid)
        {
            UserInformation sessions = new UserInformation();
            if (isValid)
            {
                sessions = new UserInformation
                {
                    id = 1,
                    company = "test1",
                    followers = 1,
                    following = 0,
                    login = "testlogin1",
                    name = "test1",
                    public_repos = 0
                };
           
            }
            else
            {
                sessions = new UserInformation
                {
                    id = null
                };
            }
            return Task.FromResult(sessions);

        }

        [Fact]
        public void ReturnOutOfRangeArgumentExceptionIfUsernameIsMoreThanTen()
        {
            List<Users> users = new List<Users>();
            for(int i = 0; i < 11; i++)
            {
                Users user = new Users
                {
                    username = "chris" + i
                };
                users.Add(user);
            }

            var mockGhUserSearchService = new Mock<IGhUserSearchService>();
            var sut = new UserController(mockGhUserSearchService.Object);
            var result = sut.getGithubUsers(users);
            var _result = Assert.IsType<JsonResult>(result.Result);
            var model = Assert.IsAssignableFrom<ArgumentOutOfRangeException>((_result.Value));

     
        }

        [Fact]
        public void TestRequestWithInvalidUserName()
        {

            List<Users> users = new List<Users>();
            Users toAdd = new Users
            {
                username = "123123123213"
            };
            users.Add(toAdd);


            //var sut = new Mock<UserController>(new Mock<IGhUserSearchService>());
            //sut.Setup(x => x.getGithubUsers(users)).Returns(new JsonResult());

            var mockGhUserSearchService = new Mock<IGhUserSearchService>();
            mockGhUserSearchService.Setup(repo => repo.GetUserFromGithubAsync(toAdd)).Returns(GetTestSessions(false));
            
            var sut = new UserController(mockGhUserSearchService.Object);

            var result = sut.getGithubUsers(users);
            var _result = Assert.IsType<JsonResult>(result.Result);
            var model = Assert.IsAssignableFrom<UserInfoRequestResponse>((_result.Value));
            Assert.Equal(2, model.userDoesntExists.Count);
        }

        //
    }
}
