using System.Web.Mvc;

namespace App.Web.UI.Areas.Security.Controllers.MVC
{
    public class HomeController : Controller
    {
        // GET: Security/home
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}