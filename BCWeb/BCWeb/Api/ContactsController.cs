using BCWeb.Models.Contacts.ServiceLayer;
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
    [Authorize]
    public class ContactsController : ApiController
    {
        private IContactServiceLayer _service;
        private IWebSecurityWrapper _security;
        private INotificationSender _notice;

        public ContactsController(IContactServiceLayer service, IWebSecurityWrapper security, INotificationSender notice)
        {
            _service = service;
            _security = security;
            _notice = notice;
        }


        public HttpResponseMessage Delete(HttpRequestMessage request, int companyId)
        {
            return request.CreateErrorResponse(HttpStatusCode.NotImplemented, "not implemented yet");
        }


    }
}
