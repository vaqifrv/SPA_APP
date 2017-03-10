using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Web;
using App.Membership.Services.Login;

namespace App.Membership.Services
{
    public class AgPermission : IPermission
    {
        public AgPermission()
        {
        }

        public IPermission Copy()
        {
            return new AgPermission();
        }

        public void Demand()
        {

            ClassFactory.Current.GetService<ILogin>().SetUserOnlineStatus();

            string userName = "";
            if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null && !String.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name))
            {
                userName = Thread.CurrentPrincipal.Identity.Name;
            }
            else
                throw new ArgumentNullException("Principal not set");

            StackTrace st = new StackTrace();
            string methodName = "";
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                object[] methodAttr = sf.GetMethod().GetCustomAttributes(typeof(AgPermissionAttribute), true);
                if (methodAttr.Length > 0)
                {

                    string ParamNames = ((AgPermissionAttribute)methodAttr.First()).ParamNames;

                    MethodBase method = sf.GetMethod();
                    Type tp = method.DeclaringType;

                    //checks if attribute has params defined because Right may be as: DynamicForms.Edit(formName=cities, otherparam=...)
                    string paramValue="";
                    if (!string.IsNullOrEmpty(ParamNames))
                    {
                        string[] paramNames = ParamNames.Split(',');
                        string[] paramValues = new string[paramNames.Length];

                        for (int j = 0; j < paramNames.Length; j++)
                        {
                            paramValues[j] = (string)HttpContext.Current.Request.RequestContext.RouteData.Values[paramNames[j]];
                            if (string.IsNullOrEmpty(paramValues[j]))
                                paramValues[j] = HttpContext.Current.Request.Params[paramNames[j]];
                            paramValues[j] = string.IsNullOrEmpty(paramValues[j]) ? paramNames[j] : paramNames[j] + "=" + paramValues[j];
                        }

                        paramValue = "(" + String.Join(",", paramValues) + ")";
                    }
                        
                    methodName = tp.Name + "." + method.Name + paramValue;
                    break;
                    
                }
            }

            //if (!AuthorizationHelper.HasAccess(userName, methodName))
            //{
            //    throw new SecurityException("Access denied");
            //}


            /*
                             LoggingSection config = (LoggingSection)System.Configuration.ConfigurationManager.GetSection("LoggingGroup/Logging");
                string platform = config.Platform.Name;
                return new SecurityPermission(PermissionState.None);
                if (platform == "ASPNET")

             */
        }

        public IPermission Intersect(IPermission target)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IPermission target)
        {
            return false;
        }

        public IPermission Union(IPermission target)
        {
            return this;
        }

        public void FromXml(SecurityElement e)
        {
            throw new NotImplementedException();
        }

        public SecurityElement ToXml()
        {
            throw new NotImplementedException();
        }
    }
}
