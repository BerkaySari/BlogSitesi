using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using BlogSitesi2.Models;
using reCAPTCHA.MVC;
using Service.AccountService;
using Service.BlogPostService;
using Service.CommentService;
using Service.UserInfoService;
using Domain.Model.Comments;

namespace BlogSitesi2.Controllers
{
    public class HomeController :  AdminController2
    {
        #region Cons.
        private readonly IBlogPostService _blogPostService;
        private readonly IAccountService _accountService;
        private readonly IUserInfoService _userInfoService;
        private readonly ICommentService _commentService;

        public HomeController(IBlogPostService blogPostService, IAccountService accountService, 
            IUserInfoService userInfoService, ICommentService commentService)
            : base(accountService)
        {
            _accountService = accountService;
            _blogPostService = blogPostService;
            _userInfoService = userInfoService;
            _commentService = commentService;
        }
        #endregion

        public ActionResult Index()
        {
            ViewData["ActiveUserPosts"] = Run(() => _blogPostService.GetActivePostsAndOrderBy());
            ViewData["Users"] = Run(() => _userInfoService.GetUsersAndOrderBy());
            return View();
        }

        [HttpPost]
        [CaptchaValidator]
        public ActionResult Index(RegisterModel registerModel, bool captchaValid)
        {
            if (ModelState.IsValid)
            {
            }
            return View(registerModel);
        }
        public ActionResult Icerik(Guid id)
        {
            ViewData["ActiveUserPosts"] = Run(() => _blogPostService.GetActivePosts(id));
            ViewData["UserComment"] = Run(() => _commentService.GetComment(id));
            return View();
        }

        [HttpPost]

        public ActionResult Comment(Comment model)
        {
            
            _commentService.AddComment(model, User.Identity.Name);
            return RedirectToAction("Icerik", new {id = model.PostId});
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