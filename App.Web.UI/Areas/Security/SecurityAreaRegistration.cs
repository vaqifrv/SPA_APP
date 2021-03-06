﻿using System.Web.Mvc;

namespace App.Web.UI.Areas.Security
{
    public class SecurityAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Security";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Security_default",
                "Security/{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "App.Web.UI.Areas.Security.Controllers.MVC" }
            );
        }
    }
}