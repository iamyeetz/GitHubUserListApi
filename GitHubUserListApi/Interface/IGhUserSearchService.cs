﻿using GitHubUserListApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubUserListApi
{
    public interface IGhUserSearchService
    {
         Task<UserInfoRequestResponse> GetUserFromGithubAsync(List<Users> name);
    }
}
