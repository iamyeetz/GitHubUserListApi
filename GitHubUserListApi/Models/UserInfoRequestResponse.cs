using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubUserListApi.Models
{
    public class UserInfoRequestResponse
    {
        public List<UserInformation> UserInformation { get; set; } = new List<UserInformation>();
        public List<Users> userDoesntExists { get; set; } = new List<Users>();
    }
}
