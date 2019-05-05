using Adz.BLL.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Adz.User.CMS.Controllers
{
    public class BaseAdminController : Controller
    {
        protected override void ExecuteCore()
        {
            if (Request.Path.ToLower().Contains("/admin/login"))
            {
                if (Request.IsAuthenticated)
                {
                    View("Index").ExecuteResult(ControllerContext);
                }
                else
                {
                    base.ExecuteCore();
                }
            }
            else
            {
                if (Request.IsAuthenticated && AdminService.Admin().Result)
                {
                    base.ExecuteCore();
                }
                else
                {
                    View("Unauthenticated").ExecuteResult(ControllerContext);
                }
            }
        }

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model)
            };
            filterContext.ExceptionHandled = true;
            string ip = GetUserIP();
            string url = HttpContext.Request.Url.AbsoluteUri;
            General.LoggingException(ip, url, model.Exception.ToString(), model.Exception.Message, "");

            logger.Error(ip + "\t" + url + "\t" + model.Exception.GetType() + "\t" + model.Exception.Message + "\t" + Thread.CurrentPrincipal.Identity.Name + Environment.NewLine + model.Exception.StackTrace);
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
        
        protected void CustomException(Exception ex)
        {
            string ip = GetUserIP();
            string url = HttpContext.Request.Url.AbsoluteUri;
            General.LoggingException(ip, url, ex.ToString(), ex.Message, "");

            logger.Error(ip + "\t" + url + "\t" + ex.GetType() + "\t" + ex.Message + "\t" + Thread.CurrentPrincipal.Identity.Name + Environment.NewLine + ex.StackTrace);
        }
        private string GetUserIP()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        public class AdminAuthorizeAttribute : AuthorizeAttribute
        {
            private readonly string Module;
            private readonly string Access;

            public AdminAuthorizeAttribute(params string[] parameter)
            {
                this.Module = parameter[0];
                this.Access = parameter[1];
            }
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                bool authorize = false;
                if (AdminService.IsAuthorized(Module, Access).Result == true)
                {
                    authorize = true;
                }

                return authorize;
            }
            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                filterContext.Result = new ViewResult { ViewName = "Unauthorized" };
                //throw new CustomException(CustomErrorType.Unauthorized);
            }
        }  
	}
}