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
            //Arrange
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

            var expected = GetTestSessionsExistingAccounts();

            //Act
            var mockGhUserSearchService = new Mock<IGhUserSearchService>();
            mockGhUserSearchService.Setup(repo => repo.GetUserFromGithubAsync(users)).Returns(expected);
            var sut = new UserController(mockGhUserSearchService.Object);
            var result = sut.GetGithubUsers(users);

            //Assert
            var _result = Assert.IsType<JsonResult>(result.Result);
            var model = Assert.IsAssignableFrom<UserInfoRequestResponse>((_result.Value));
            Assert.Equal(2, model.UserInformation.Count);
        }
       

        [Fact]
        public void ReturnOutOfRangeArgumentExceptionIfUsernameIsMoreThanTen()
        {
            //Arrange
            List<Users> users = new List<Users>();
            for(int i = 0; i < 11; i++)
            {
                Users user = new Users
                {
                    username = "chris" + i
                };
                users.Add(user);
            }
            //Act
            var mockGhUserSearchService = new Mock<IGhUserSearchService>();
            var sut = new UserController(mockGhUserSearchService.Object);
            var result = sut.GetGithubUsers(users);

            //Assert
            var _result = Assert.IsType<JsonResult>(result.Result);
            var model = Assert.IsAssignableFrom<ArgumentOutOfRangeException>((_result.Value));

     
        }

        [Fact]
        public void TestRequestWithInvalidUserName()
        {
            //Arrange
            List<Users> users = new List<Users>();
            Users toAdd = new Users
            {
                username = "123123123213"
            };
            users.Add(toAdd);
            Users toAdd1 = new Users
            {
                username = "iamyeetz"
            };
            users.Add(toAdd);

            var expected = GetTestSessionsWithNotExistingAccount();

            //Act

            var mockGhUserSearchService = new Mock<IGhUserSearchService>();
            mockGhUserSearchService.Setup(repo => repo.GetUserFromGithubAsync(users)).Returns(expected);
            var sut = new UserController(mockGhUserSearchService.Object);
            var result = sut.GetGithubUsers(users);

            //Assert
            var _result = Assert.IsType<JsonResult>(result.Result);
            var model = Assert.IsAssignableFrom<UserInfoRequestResponse>((_result.Value));
            Assert.Single(model.userDoesntExists);
            Assert.Single(model.UserInformation);
        }


        private Task<UserInfoRequestResponse> GetTestSessionsExistingAccounts()
        {
            UserInfoRequestResponse userInfoRequestResponse = new UserInfoRequestResponse();


            var data1 = new UserInformation
            {

                id = null,
                company = "test1",
                followers = 1,
                following = 0,
                login = "testlogin1",
                name = "test1",
                public_repos = 0
            };
            var data2 = new UserInformation
            {
                id = 2,
                company = "test2",
                followers = 2,
                following = 0,
                login = "testlogin2",
                name = "test2",
                public_repos = 0
            };
            userInfoRequestResponse.UserInformation.Add(data1);
            userInfoRequestResponse.UserInformation.Add(data2);

            return Task.FromResult(userInfoRequestResponse);

        }
        private Task<UserInfoRequestResponse> GetTestSessionsWithNotExistingAccount()
        {
            UserInfoRequestResponse userInfoRequestResponse = new UserInfoRequestResponse();


            var data1 = new Users
            {

                username = "123123123213"
            };
            var data2 = new UserInformation
            {
                id = 2,
                company = "test2",
                followers = 2,
                following = 0,
                login = "testlogin2",
                name = "test2",
                public_repos = 0
            };
            userInfoRequestResponse.userDoesntExists.Add(data1);
            userInfoRequestResponse.UserInformation.Add(data2);

            return Task.FromResult(userInfoRequestResponse);

        }
    }
}
