using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Repository;
using Domain.Model.UserInfos;

namespace Repository.UserRepository
{
    public interface IUserRepository : IRepository<UserInfo, Guid>
    {
       UserInfo GetByLoginName(string userId);
        bool IsExist(string userName, string password);
    }
}
