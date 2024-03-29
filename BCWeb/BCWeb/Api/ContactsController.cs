﻿using BCWeb.Models.Contacts.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.Notifications.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BCModel.SocialNetwork;
using Web.Attributes;
using BCWeb.Models.Contacts.ViewModel;

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


        public HttpResponseMessage Get(HttpRequestMessage request, int companyId)
        {
            var contacts = _service.GetCompaniesConnections(companyId)
                .Select(s => new ContactItem
                {
                    CompanyId = s.LeftId == companyId ? s.RightId : s.LeftId,
                    CompanyName = s.LeftId == companyId ? s.Right.CompanyName : s.Left.CompanyName
                });

            return request.CreateResponse(HttpStatusCode.OK, contacts);
        }

        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage Delete(HttpRequestMessage request, int companyToDeleteId)
        {
            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            ContactConnection contact = _service.GetNetworkConnection(companyId, companyToDeleteId);

            if (contact == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (_service.RemoveNetworkConnection(contact))
            {
                return request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, _service.ValidationDic);
            }
        }


    }
}
