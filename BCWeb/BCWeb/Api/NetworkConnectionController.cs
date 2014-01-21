using BCWeb.Areas.Contacts.Models.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.Notifications.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    public class NetworkConnectionController : ApiController
    {
        private INetworkServiceLayer _service;
        private IWebSecurityWrapper _security;
        private INotificationSender _notice;
    }
}
