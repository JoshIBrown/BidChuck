using System.Web.Mvc;

namespace BCWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "Welcome to BidChuck!";

            return View();
        }

        public ActionResult About()
        {
            //ViewBag.Message = "About BidChuck...";

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Contact()
        {
            //ViewBag.Message = "Contact BidChuck...";

            return View();
        }
    }
}