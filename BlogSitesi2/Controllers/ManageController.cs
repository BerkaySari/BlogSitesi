using System;
using System.Web.Mvc;
using BlogSitesi2.Models;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using BlogSitesi2.Filters;
using System.Transactions;
using Service.AccountService;
using Service.BlogPostService;
using Domain.Model.BlogPosts;

namespace BlogSitesi2.Controllers
{

    [Authorize]
    [InitializeSimpleMembership]
    public class ManageController :  AdminController2
    {
        #region Cons.
        private readonly IBlogPostService _blogPostService;
        private readonly IAccountService _accountService;

        public ManageController(IBlogPostService blogPostService, IAccountService accountService)
            : base(accountService)
        {
            _accountService = accountService;
            _blogPostService = blogPostService;
        }
        #endregion

        public ActionResult Manage(Guid id)
        {
            ViewData["ActiveUserPosts"] = Run(() => _blogPostService.GetPosts(User.Identity.Name));
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            return View(model);
        }

        public ActionResult ManageContent(Guid id)
        {
            ViewData["ActiveUserPosts"] = Run(() => _blogPostService.GetPosts(id, User.Identity.Name));
            return View();
        }

        public ActionResult ManageEdit(Guid id)
        {
            ViewData["EditUserPosts"] = Run(() => _blogPostService.GetPosts(id, User.Identity.Name));
            return View();
        }

        [HttpPost]
        public ActionResult ManageEdit(Guid id, BlogPost model)
        {
            _blogPostService.UpdatePost(id, model, User.Identity.Name);
            return RedirectToAction("Manage");
        }

        public ActionResult ManageDelete(Guid id)
        {
            ViewData["DeleteUserPosts"] = Run(() => _blogPostService.GetPosts(id));
            return View();
        }

        [HttpPost]
        public ActionResult ManageDelete(Guid id, BlogPost model)
        {
            _blogPostService.DeletePost(id);
            return RedirectToAction("Manage");
        }

        public ActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPost(BlogPost model)
        {
            if (model.Title == null && model.Contents == null)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                _blogPostService.AddPost(model, User.Identity.Name);
                return RedirectToAction("Index","Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            AccountController.ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = AccountController.ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }
            return RedirectToAction("Manage", "Manage", new { Message = message });
        }
    }
}
