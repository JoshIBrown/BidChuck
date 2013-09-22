using BCWeb.Areas.Projects.Models.BidPackage.ServiceLayer;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Projects.Controllers
{
    public class BidPackageController : Controller
    {

        private IBidPackageServiceLayer _service;
        private IWebSecurityWrapper _security;

        public BidPackageController(IBidPackageServiceLayer service,IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }


        //
        // GET: /Projects/BidPackage/

        public ActionResult Index()
        {
            return View();
        }



    }
}
