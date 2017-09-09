using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Core.Repository;
using Domain.Model.UserInfos;

namespace Repository.UserInfoRepository
{
    public interface IUserInfoRepository : IRepository<UserInfo, Guid>
    {
        UserInfo GetSingleUser(string userName);
        IList<UserInfo> GetUsers(string userName);
        IList<UserInfo> GetUsersAndOrderBy();
    }
}
