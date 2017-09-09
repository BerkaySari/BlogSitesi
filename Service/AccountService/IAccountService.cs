using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Domain.Model.UserInfos;

namespace Service.AccountService
{
    public interface IAccountService
    {
        Result<UserInfo> SetGlobalUserContext(string userId);
        Result<string> IsExist(string userName, string password);
    }
}
