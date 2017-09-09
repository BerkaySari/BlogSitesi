using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Repository;
using Domain.Model.UserInfos;
using NHibernate;

namespace Repository.UserInfoRepository
{
    public class UserInfoRepository : RepositoryBase<UserInfo, Guid>, IUserInfoRepository
    {
        public UserInfo GetSingleUser(string userName)
        {
            var result = Session.QueryOver<UserInfo>().Where(u => u.UserName == userName).SingleOrDefault();

            return result;
        }
        public IList<UserInfo> GetUsers(string userName)
        {
            var result = Session.QueryOver<UserInfo>().Where(u => u.UserName == userName).List();

            return result;
        }
        public IList<UserInfo> GetUsersAndOrderBy()
        {
            var result = Session.QueryOver<UserInfo>().OrderBy(u => u.Date).Desc.Take(7).List();

            return result;
        }
    }
}
