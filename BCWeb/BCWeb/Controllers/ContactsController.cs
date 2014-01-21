using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Controllers
{
    public class ContactsController : Controller
    {
        //
        // GET: /contacts/

        public ActionResult Index()
        {
            return View();
        }

    }
}
