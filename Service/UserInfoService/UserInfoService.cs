using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Core.Entity;
using Domain.Model.UserInfos;
using Repository.UserInfoRepository;
using Dto;


namespace Service.UserInfoService
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IUserInfoRepository _userInfoRepository;

        public UserInfoService(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public Result<IList<UserInfo>> GetUsers()
        {
            return Result<IList<UserInfo>>.AsSuccess(_userInfoRepository.GetAll());
        }

        public Result GetSingleUser(string userName)
        {
            return Result<UserInfo>.AsSuccess(_userInfoRepository.GetSingleUser(userName));
        }

        public Result<IList<UserInfo>> GetUsers(string userName)
        {
            return Result<IList<UserInfo>>.AsSuccess(_userInfoRepository.GetUsers(userName));
        }
        public Result<IList<UserInfo>> GetUsersAndOrderBy()
        {
            return Result<IList<UserInfo>>.AsSuccess(_userInfoRepository.GetUsersAndOrderBy());
        }
        public Result DeleteUser(Guid id)
        {
            var user = _userInfoRepository.Get(id);

            _userInfoRepository.Delete(user);

            return Result.AsSuccess();
        }
        public Result AddUser(UserInfo model)
        {
            _userInfoRepository.Insert(model);

            return Result.AsSuccess();
        }

        public Result UserNewPassword(LocalPasswordModel model, string userName)
        {
            var userInfo = _userInfoRepository.GetSingleUser(userName);

            userInfo.Password = model.Password;

            _userInfoRepository.Update(userInfo);

            return Result.AsSuccess();
        }
    }
}
