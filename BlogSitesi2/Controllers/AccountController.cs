using System;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using Domain.Model.UserInfos;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using BlogSitesi2.Filters;
using BlogSitesi2.Models;
using Dto;
using reCAPTCHA.MVC;
using Service.AccountService;
using Service.UserInfoService;

namespace BlogSitesi2.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : AdminController2
    {
        #region Cons.
        private readonly IAccountService _accountService;
        private readonly IUserInfoService _userInfoService;

        public AccountController(IAccountService accountService, IUserInfoService userInfoService)
            : base(accountService)
        {
            _accountService = accountService;
            _userInfoService = userInfoService;
        }
        #endregion

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var error = Run(() => _accountService.IsExist(model.UserName, model.Password), () => RedirectToAction("Login", model));

            if (error != null)
            {
                ViewBag.ErrorCase = error;
                return View(model);
            }

            UserContext = Run(() => _accountService.SetGlobalUserContext(model.UserName), () => RedirectToAction("Login", model));
            FormsAuthentication.SetAuthCookie(model.UserName, true);
            WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe);

            if (model.UserName == "Admin")
            {
                return RedirectToAction("Admin", "Admin");
            }

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [CaptchaValidator]
        public ActionResult Register(RegisterModel model, bool captchaValid)
        {
            var userInfo = new UserInfo
            {
                UserName = model.UserName,
                Password = model.Password,
                Mail = model.Mail,
                Date = DateTime.Now.ToString()
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _userInfoService.AddUser(userInfo);                   
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

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
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }
            return RedirectToAction("Manage", "Manage", new { Message = message });
        }

        public ActionResult NewPassword(int? id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewPassword(Dto.LocalPasswordModel model)
        {
            _userInfoService.UserNewPassword(model, User.Identity.Name);
            return RedirectToAction("Manage", "Manage"); 
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
