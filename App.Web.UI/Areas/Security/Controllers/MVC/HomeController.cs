﻿using System.Web.Mvc;

namespace App.Web.UI.Areas.Security.Controllers.MVC
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Security/home
       
        public ActionResult Index()
        {
            return View();
        }
    }
}