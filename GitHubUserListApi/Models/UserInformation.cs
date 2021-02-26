using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubUserListApi.Models
{
    public class UserInformation 
    {
        public int? id { get; set; }
        public string login { get; set; }
        public string name { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
        public string company { get; set; }
        public int public_repos { get; set; }
        

        public decimal averageFollower
        {
            get {
                return getAverageFollowers();
            }
        }


       public decimal getAverageFollowers()
        {
            try
            {
                return this.followers / this.public_repos;
            }
            catch (Exception ex)
            {

                return 0;
            }
            
        }

    }
    public class UserInfoRequestResponse
    {
        public List<UserInformation> UserInformation { get; set; } = new List<UserInformation>();
        public List<Users> userDoesntExists { get; set; } = new List<Users>();
    }
}
