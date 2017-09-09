using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Repository.UserRepository;
using Domain.Model.UserInfos;

namespace Service.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        public AccountService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Result<UserInfo> SetGlobalUserContext(string userId)
        {
            var user = _userRepository.GetByLoginName(userId);

            var userContext = new UserInfo
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Mail = user.Mail
            };

            return Result<UserInfo>.AsSuccess(userContext);
        }
        public Result<string> IsExist(string userName, string password)
        {
            return _userRepository.IsExist(userName, password) ? Result<string>.AsSuccess(null) :
                Result<string>.AsSuccess("Kullanıcı adınızı veya şifrenizi hatalı girdiniz.");
        }
    }
}
