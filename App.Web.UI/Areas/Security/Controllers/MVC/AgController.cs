using App.Membership.Services;
using System.Web;
using System.Web.Mvc;

namespace App.Web.UI.Areas.Security.Controllers.MVC
{
    public class AgController : Controller
    {
        public AgController()
        {

        }
        protected override void OnAuthorization(AuthorizationContext context)
        {
            base.OnAuthorization(context);
            ///User.Identity.HasAccess(context.ActionDescriptor.ActionName);


            //if (context.HttpContext.User.Identity.IsAuthenticated)
            //  {
            //     var url = new UrlHelper(context.RequestContext);
            //     var logonUrl = url.Action("LogOn", "SSO", new { reason = "youAreAuthorisedButNotAllowedToViewThisPage" });
            //     context.Result = new RedirectResult(logonUrl);
            //     return;
            //  }
        }
    }

    public class AgAuthorizationAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);


            //object[] methodAttr = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AgPermissionAttribute), true);
            //if (methodAttr.Length > 0)
            //{
            //    string ParamNames = ((AgPermissionAttribute)methodAttr.First()).paramNames;
            //}

            if (!filterContext.HttpContext.User.Identity.HasAccess(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "." + filterContext.ActionDescriptor.ActionName))
            {
                //HandleUnauthorizedRequest(filterContext);
                throw new HttpException(403, "Access Denied");
            }

        }
    }
}