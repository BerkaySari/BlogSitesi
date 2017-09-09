using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Domain.Model.UserInfos;
using Dto;

namespace Service.UserInfoService
{
    public interface IUserInfoService
    {
        Result<IList<UserInfo>> GetUsers();
        Result GetSingleUser(string userName);
        Result<IList<UserInfo>> GetUsers(string userName);
        Result<IList<UserInfo>> GetUsersAndOrderBy();
        Result DeleteUser(Guid id);
        Result AddUser(UserInfo model);
        Result UserNewPassword(LocalPasswordModel model, string userName);
    }
}
