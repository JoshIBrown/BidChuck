using BCModel;
using BCWeb.Areas.Account.Models.Scopes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Account.Controllers
{
    [Authorize]
    public class ScopesController : Controller
    {
        //
        // GET: /Account/Scopes/

        // used by user to manage their ability scope
        public ActionResult Index()
        {
            // display chosen scopes
            return View();
        }


        public ActionResult Manage()
        {
            return View();
        }
    }
}
