using BCWeb.Models;
using BCWeb.Models.Notifications.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Controllers
{
    public class NotificationsController : Controller
    {
        private INotificationServiceLayer _service;
        private IWebSecurityWrapper _security;


        public NotificationsController(INotificationServiceLayer service, IWebSecurityWrapper security)
        {
            _security = security;
            _service = service;
        }
        //
        // GET: /Notification/

        public ActionResult Index()
        {
            return View();
        }

    }
}
