using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using Core.Common;
using Service.Setup;
using Domain.Model.UserInfos;
using Service.AccountService;
//using Domain.Model.Voters;
//using EVoting.Models;

namespace BlogSitesi2.Controllers
{

    public class AdminController2 : CustomControllerBase
    {
        public AdminController2(IAccountService accountService)
            : base(accountService)
        {
        }
    }
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(UserInfo userContext)
        {
            //if (userContext.IsAdmin)
            //{

            //    Name = "(Admin) " + userContext.Name + " " + userContext.LastName;
            //}
            //else
            //{
               // Name = userContext.Name + " " + userContext.LastName;
           // }
            Name = userContext.UserName;
        }

        public string Name { get; private set; }
        public string AuthenticationType { get { return "AdminMemberProvider"; } }
        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
        public IList<string> Roles { get; set; }
    }

    public class CustomPrincipal : IPrincipal
    {

        public CustomPrincipal(CustomIdentity customIdentity)
        {
            Identity = customIdentity;
        }

        public bool IsInRole(string role)
        {
            return ((CustomIdentity)Identity).Roles.Contains(role);
        }

        public IIdentity Identity { get; private set; }
    }

    public class CustomControllerBase : DefaultController
    {
        public CustomControllerBase(IAccountService accountService)
            : base(accountService)
        {
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/PageNotFound.cshtml"
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Status = "404 Not Found";
            filterContext.HttpContext.Response.StatusCode = 404;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var returnType = typeof(ActionResult);
            var descriptor = SetReflectedActionDescriptor(filterContext, ref returnType);
            var actionMethod = descriptor.MethodInfo;
            var actionName = actionMethod.Name;
            var controllerName = actionMethod.ReflectedType.Name.Replace("Controller", "");



            if (UserContext == null && User.Identity.IsAuthenticated && !(controllerName == "Account" && actionName == "Login"))
            {
                UserContext = Run(() => AccountService.SetGlobalUserContext(User.Identity.Name), () =>
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login", "Account");
                });
            }

            //if (HttpContext.Cache[BoundaryKeys.MenuSessionKey] == null)
            //{
            //    var menuModel = CategoryService.GetMenuForLayout().Data;
            //    HttpContext.Cache[BoundaryKeys.MenuSessionKey] = menuModel;
            //}

            //if (UserContext != null)
            //{
            //    Session[BoundaryKeys.Notifications] = CategoryService.GetUserNotifications(UserContext.Id).Data;
            //}

            filterContext.RequestContext.HttpContext.Request.Headers.Add("X-Esi", "1");
            //Request.Headers.Add("X-Esi", "1");

            base.OnActionExecuting(filterContext);
        }

        public ActionResult ReturnToLogin(bool isAuthenticated)
        {
            if (!isAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (UserContext != null)
                filterContext.HttpContext.User = new CustomPrincipal(new CustomIdentity(UserContext));
            base.OnAuthorization(filterContext);
        }
    }

    public class DefaultController : Controller
    {
        protected UserInfo UserContext
        {
            get { return Session[BoundaryKeys.UserLoginKey] as UserInfo; }
            set { Session[BoundaryKeys.UserLoginKey] = value; }
        }

        protected IAccountService AccountService;

        public DefaultController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        protected ActionResult ErrorResult;

        protected virtual void FlashSuccessMessage(string message)
        {
            TempData[BoundaryKeys.FlashMessageKey] = message;
            TempData[BoundaryKeys.FlashMessageTypeKey] = "success";
        }

        protected virtual void FlashErrorMessage(string message)
        {
            TempData[BoundaryKeys.FlashMessageKey] = message;
            TempData[BoundaryKeys.FlashMessageTypeKey] = "error";
        }

        protected static ReflectedActionDescriptor SetReflectedActionDescriptor(ActionExecutingContext filterContext, ref Type returnType)
        {
            var descriptor = ((ReflectedActionDescriptor)(filterContext.ActionDescriptor));
            if (descriptor.MethodInfo.ReturnParameter != null)
            {
                returnType = descriptor.MethodInfo.ReturnParameter.ParameterType;
            }
            return descriptor;
        }

        #region Run  methods

        public virtual Result<T> RunAjax<T>(Func<Result<T>> serviceMethod)
        {
            if (!ModelState.IsValid)
            {
                var errorModel = ModelState.Values.First(p => p.Errors.Any());
                ErrorResult = JsonError(errorModel.Errors.First().ErrorMessage);
                throw new ServiceCustomResultException();
            }
            var result = serviceMethod.Invoke();
            if (result.IsSuccess)
            {
                ErrorResult = Json(result);
                return result;
            }
            ErrorResult = JsonError(result.Message);
            throw new ServiceCustomResultException();
        }
        protected virtual JsonResult JsonError(string errorList)
        {
            return Json(Result.AsError(errorList), JsonRequestBehavior.AllowGet);
        }
        protected virtual JsonResult JsonSuccess(object data)
        {
            return Json(new Result<dynamic> { IsSuccess = true, Data = data }, JsonRequestBehavior.AllowGet);
        }
        protected virtual JsonResult JsonSuccess()
        {
            return Json(Result.AsSuccess(), JsonRequestBehavior.AllowGet);
        }

        public virtual T Run<T>(Func<Result<T>> serviceMethod, Func<RedirectToRouteResult> errorRedirectAction)
        {
            var result = serviceMethod.Invoke();
            if (result.IsSuccess)
            {
                return result.Data;
            }
            FlashErrorMessage(result.Message);
            ErrorResult = errorRedirectAction.Invoke();
            throw new ServiceCustomResultException();
        }
        public virtual void Run(Func<Result> serviceMethod, Func<ActionResult> errorAction)
        {
            Run(serviceMethod, errorAction, "");
        }
        public virtual void Run(Func<Result> serviceMethod, Func<ActionResult> errorAction, string successMessage)
        {
            if (!ModelState.IsValid)
            {
                ErrorResult = errorAction.Invoke();

                throw new ServiceCustomResultException();
            }
            var result = serviceMethod.Invoke();
            if (result.IsSuccess)
            {
                if (!string.IsNullOrWhiteSpace(successMessage))
                    FlashSuccessMessage(successMessage);
                return;
            }
            FlashErrorMessage(result.Message);
            ErrorResult = errorAction.Invoke();
            throw new ServiceCustomResultException();
        }
        public virtual T Run<T>(Func<Result<T>> serviceMethod, Func<PartialViewResult> errorPartialView)
        {
            var result = serviceMethod.Invoke();
            if (result.IsSuccess)
            {
                return result.Data;
            }
            FlashErrorMessage(result.Message);
            ErrorResult = errorPartialView.Invoke();
            throw new ServiceCustomResultException();
        }

        public virtual T Run<T>(Func<Result<T>> serviceMethod)
        {
            var result = serviceMethod.Invoke();
            if (result.IsSuccess)
            {
                return result.Data;
            }
            throw new ServiceCustomResultException();
        }

        public virtual T Run<T>(Func<Result<T>> serviceMethod, Func<ViewResult> errorPartialView)
        {
            var result = serviceMethod.Invoke();
            if (result.IsSuccess)
            {
                if (!string.IsNullOrEmpty(result.Message))
                    FlashSuccessMessage(result.Message);
                return result.Data;
            }
            FlashErrorMessage(result.Message);
            ErrorResult = errorPartialView.Invoke();
            throw new ServiceCustomResultException();
        }
        #endregion

    }

}

