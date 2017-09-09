using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Repository;
using Domain.Model.UserInfos;

namespace Repository.UserRepository
{
    public class UserRepository : RepositoryBase<UserInfo, Guid>, IUserRepository
    {
        public UserInfo GetByLoginName(string userId)
        {
            return Session.QueryOver<UserInfo>().Where(g => g.Password == userId).SingleOrDefault();
        }
        public bool IsExist(string userName, string password)
        {
            return Session.QueryOver<UserInfo>().Where(p => p.UserName == userName && p.Password == password).RowCount() > 0;
        }
    }
}
