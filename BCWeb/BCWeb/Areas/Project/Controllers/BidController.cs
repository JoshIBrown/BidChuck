using BCWeb.Areas.Project.Models.Bids.ServiceLayer;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Project.Controllers
{
    [Authorize]
    public class BidController : Controller
    {
        private IBidServiceLayer _service;
        private IWebSecurityWrapper _security;

        public BidController(IBidServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }
        //
        // GET: /Project/Bid/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "general_contractor,subcontractor,materials_vendor,Administrator")]
        [HttpGet]
        public ActionResult Compose(int projectId)
        {
            // if GC
            if (User.IsInRole("general_contractor"))
            {
                return View("ComposeGC");
            }
            // else if sub or vendor
            if (User.IsInRole("subcontractor") || User.IsInRole("materials_vendor"))
            {
                return View("ComposeSubAndVend");
            }

            return View();
        }

    }
}
