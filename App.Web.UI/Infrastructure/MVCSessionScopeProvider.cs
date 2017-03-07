using App.Core.Infrastructure.Abstract;
using System;
using System.Security.Principal;
using System.Web;

namespace App.Web.UI.Infrastructure
{
    public class MVCSessionScopeProvider : ISessionScopeProvider
    {
        public object Scope
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Handler != null && HttpContext.Current.Request != null)
                    return HttpContext.Current.Request;
                else
                    return null;
            }
        }

        public IPrincipal Principal
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null)
                    return HttpContext.Current.User;
                else
                    return null;
            }
        }

        

        public bool UserHasAccess(string right)
        {
            throw new NotImplementedException("Not implemented");
            //return HttpContext.Current.User.Identity.HasAccess(right);
        }
    }
}
