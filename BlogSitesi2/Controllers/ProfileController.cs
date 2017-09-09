using System;
using System.Web.Mvc;
using Service.AccountService;
using Service.BlogPostService;
using Service.UserInfoService;

namespace BlogSitesi2.Controllers
{
    public class ProfileController :  AdminController2
    {
        #region Cons.
        private readonly IBlogPostService _blogPostService;
        private readonly IAccountService _accountService;
        private readonly IUserInfoService _userInfoService;

        public ProfileController(IBlogPostService blogPostService, IAccountService accountService, 
            IUserInfoService userInfoService)
            : base(accountService)
        {
            _accountService = accountService;
            _blogPostService = blogPostService;
            _userInfoService = userInfoService;
        }
        #endregion

        [HttpGet]
        public ActionResult UserProfile(int? id, string UserName)
        {
            ViewData["ActiveUserPosts"] = Run(() => _blogPostService.GetPosts(UserName));
            ViewData["UserData"] = Run(() => _userInfoService.GetUsers(UserName));
            return View();
        }

        public ActionResult ProfileContent(Guid id, string userName)
        {
            ViewData["ActiveUserPosts"] = Run(() => _blogPostService.GetPosts(id, userName));
            return View();
        }
    }
}
