using BCModel;
using BCWeb.Areas.Account.Models.Users.ServiceLayer;
using BCWeb.Areas.Admin.Models.Users;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private IWebSecurityWrapper _security;
        private IUserProfileServiceLayer _service;
        public UserController(IUserProfileServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        //
        // GET: /Admin/User/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Create(UserProfileEditModel user)
        {
            return View();
        }
    }
}
