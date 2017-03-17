using App.Membership.Repositories.Abstract;
using App.Membership.Repositories.NHibernate;
using App.Membership.Services;
using App.Membership.Services.Login;
using App.Membership.Services.SSO;
using App.Membership.Services.SSO.Configuration;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Web.UI.Areas.Security.Controllers.MVC
{
    public class LoginController : AgController
    {

        private readonly IRightRepository _rightRepository;

        //
        // GET: /Login/
        private string SSOServer
        {
            get
            {
                object section = System.Configuration.ConfigurationManager.GetSection("SingleSignOn");
                if (section != null)
                    return ((SingleSignOnSection)section).Settings.SsoServer;
                else
                    return "";
            }
        }
        private ILogin<LoginStatus> loginProvider;

        public LoginController()
        {
            var factory = new ClassFactory();
            loginProvider = factory.GetService<ILogin<LoginStatus>>();

            _rightRepository = new RightRepository();
            //_logActionRepository = new Logc();
        }



        [HttpGet]
        public ActionResult SSOLogIn(string token)
        {
            var service = new SsoService();
            SecurityToken secToken = service.GetToken(token);

            if (String.IsNullOrEmpty(secToken.Username))
            {
                string newTokenId = service.CreateToken(new SecurityToken { Username = User.Identity.Name, ReturnUrl = secToken.ReturnUrl });
                string url = UrlUtils.AddParamsToUrl(secToken.ReturnUrl, new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("token", newTokenId) });

                if (!User.Identity.IsAuthenticated)
                    return Redirect(UrlUtils.AddParamsToUrl(url, new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("sso", "true") }));

                return Redirect(url);
            }
            else
            {
                if (!User.Identity.IsAuthenticated)
                    loginProvider.LogIn(secToken.Username);
                return Redirect(secToken.ReturnUrl);
            }
        }

        [HttpGet]
        public ActionResult SSOLogOff(string token)
        {
            SsoService service = new SsoService();
            SecurityToken secToken = service.GetToken(token);
            loginProvider.LogOut();
            return Redirect(secToken.ReturnUrl);
        }

        [HttpGet]
        public ActionResult Index(string returnUrl, string token, bool sso = false)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            ViewBag.LoginProcessing = true;

            #region for login with sso
            var service = new SsoService();
            SecurityToken ssoResult = service.GetLoginBySso(returnUrl, token, sso);
            if (!String.IsNullOrEmpty(ssoResult.Username))
            {
                //for asp web.forms must be  System.Web.Security.FormsAuthentication.SetAuthCookie(userName, false);
                var loginStatus = loginProvider.LogIn(ssoResult.Username);
                if (loginStatus == LoginStatus.Successful)
                    return RedirectToLocal(ssoResult.ReturnUrl);
            }
            else if (!String.IsNullOrEmpty(ssoResult.ReturnUrl))
                return RedirectToLocal(ssoResult.ReturnUrl);
            #endregion

            ViewBag.returnUrl = returnUrl;

            return View("Login");
        }


        [HttpPost]
        public ActionResult Index(string userName, string password, string returnUrl)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            var loginStatus = loginProvider.LogIn(userName, password);
            if (loginStatus == LoginStatus.AttemptsExceed)
            {
                ModelState.AddModelError("", "Attempts exceeded");
                ViewBag.returnUrl = returnUrl;

                return View("Login");
            }
            else if (loginStatus == LoginStatus.Successful)
            {
                #region for inform sso server
                SsoService service = new SsoService();
                returnUrl = service.PostLoginToSso(returnUrl, userName);
                #endregion
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "İstifadəçi adı və ya şifrəsi yanlışdır!");
                ViewBag.returnUrl = returnUrl;

                return View("Login");
            }
        }

        public ActionResult LogOff(string returnUrl)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            loginProvider.LogOut();

            #region for logout from sso server
            SsoService service = new SsoService();
            returnUrl = service.LogOffFromSso(returnUrl);
            #endregion

            return Redirect(returnUrl);

        }

        private string NormalizeReturnUrl(string returnUrl)
        {
            if (returnUrl == null)
                returnUrl = "";
            if (returnUrl.Length < 3)
                returnUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            else if (returnUrl[0] == '/')
            {
                returnUrl = Request.Url.GetLeftPart(UriPartial.Authority) + returnUrl;
            }
            return returnUrl;
        }

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
    }
}
