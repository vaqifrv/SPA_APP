using System;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace App.Web.UI.Infrastructure
{
    public abstract class BaseController : Controller
    {
        public JsonResult Json(object data, string nameOfSet = "")
        {
            return new JsonNetResult
            {
                Data = data,
                NameOfSet = nameOfSet
            };
        }

        public JsonResult Json(object data, JsonRequestBehavior behavior, string nameOfSet = "")
        {
            return new JsonNetResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                NameOfSet = nameOfSet
            };
        }

        protected override JsonResult Json(object data, string contentType,
            Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            try
            {
                string cultureName = null;
                // Culture cookieden gelen request
                HttpCookie cultureCookie = Request.Cookies["_culture"];
                if (cultureCookie != null)
                {
                    cultureName = cultureCookie.Value;
                    //else
                    //    cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                    //            Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                    //            null;
                    // Validate culture name.Vacib deyil
                    //cultureName = CultureHelper.GetImplementedCulture(cultureName);

                    // Modify current thread's cultures            
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                }
                return base.BeginExecuteCore(callback, state);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

    }

}