using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Service.BlogPostService;
using Service.AccountService;
using Service.CommentService;
using Service.UserInfoService;

namespace BlogSitesi2.Controllers
{
    public class AdminController :  AdminController2
    {
        #region Cons.
        private readonly IBlogPostService _blogPostService;
        private readonly IAccountService _accountService;
        private readonly IUserInfoService _userInfoService;
        private readonly ICommentService _commentService;

        public AdminController(IBlogPostService blogPostService, IAccountService accountService, 
            IUserInfoService userInfoService, ICommentService commentService)
            : base(accountService)
        {
            _accountService = accountService;
            _blogPostService = blogPostService;
            _userInfoService = userInfoService;
            _commentService = commentService;
        }
        #endregion

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult AdminUsers()
        {
            ViewData["UserInformations"] = Run(() => _userInfoService.GetUsers());
            return View();
        }

        public ActionResult AdminNotActivePosts()
        {
            ViewData["NotActiveUserPosts"] = Run(() => _blogPostService.GetNotActivePosts());
            return View();
        }

        public ActionResult AdminActivePosts()
        {
            ViewData["ActiveUserPosts"] = Run(() => _blogPostService.GetActivePosts());
            return View();
        }
        public ActionResult DeleteUserAdmin(Guid id)
        {
            _userInfoService.DeleteUser(id);
            return RedirectToAction("AdminUsers");
        }

        public ActionResult ActivatePostAdmin(Guid id)
        {
            _blogPostService.ChangePostActivate(id);
            return RedirectToAction("AdminNotActivePosts");
        }

        public ActionResult NotActivePostViewAdmin(Guid id)
        {
            ViewData["NotActiveUserPosts"] = Run(() => _blogPostService.GetNotActivePosts(id));
            ViewData["UserComment"] = Run(() => _commentService.GetComment(id));
            return View();
        }

        public ActionResult NotActivePostDelete(Guid id)
        {
            _blogPostService.DeletePost(id);
            return RedirectToAction("AdminNotActivePosts");
        }

        public ActionResult DeactivePostAdmin(Guid id)
        {
            _blogPostService.ChangePostActivate(id);
            return RedirectToAction("AdminActivePosts");
        }

        public ActionResult ActivePostDelete(Guid id)
        {
            _blogPostService.DeletePost(id);
            return RedirectToAction("AdminActivePosts");
        }

        public ActionResult AdminView(Guid id)
        {
            ViewData["ActiveUserPosts"] = Run(() => _blogPostService.GetActivePosts(id));
            ViewData["UserComment"] = Run(() => _commentService.GetComment(id));
            return View();
        }

        [HttpGet]
        public ActionResult AdminCommentTitle(Guid id)
        {
            ViewData["Comments"] = Run(() => _commentService.GetComment(id));
            return View();
        }

        public ActionResult AdminCommentDelete(Guid id)
        {
            return RedirectToAction("AdminCommentTitle", new{id = _commentService.DeleteComment(id)});
        }

        #region Slug generating
        public string GenerateSlug(string title)
        {
            string phrase = string.Format("{0}", title);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 50 ? str.Length : 50).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
        #endregion
    }
}
