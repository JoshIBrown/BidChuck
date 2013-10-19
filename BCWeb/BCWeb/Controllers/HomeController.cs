using System.Web.Mvc;

namespace BCWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated && User.IsInRole("Administrator"))
                return RedirectToRoute("Default", new { controller = "Admin", action = "Index" });

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}