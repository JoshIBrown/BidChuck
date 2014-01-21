using BCModel.SocialNetwork;
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
    [Authorize]
    public class ConnectionRequestController : ApiController
    {
        private INetworkServiceLayer _service;
        private IWebSecurityWrapper _security;
        private INotificationSender _notice;

        public ConnectionRequestController(INetworkServiceLayer service, IWebSecurityWrapper security,INotificationSender notice)
        {
            _service = service;
            _security = security;
            _notice = notice;
        }


        // GET
        // /api/ConnectionRequest
        public object Get()
        {
            throw new NotImplementedException();
        }

        // GET
        // /api/ConnectionRequest/id
        public object Get(Guid id)
        {
            throw new NotImplementedException();
        }


        // PUT
        // /api/ConnectionRequest/id?accept=true/false

        public HttpResponseMessage Put(HttpRequestMessage request, Guid id, bool accept)
        {
            HttpResponseMessage response;

            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            ConnectionRequest friendRequest = _service.GetNetworkRequest(id);

            // if networking request was found
            if (friendRequest != null)
            {
                // make sure request has not been responded to yet
                if (friendRequest.DeclineDate.HasValue || friendRequest.AcceptDate.HasValue)
                {
                    response = request.CreateResponse(HttpStatusCode.Conflict, "request has already been responded to.");
                    return response;
                }

                // set accept/decline
                if (accept)
                    friendRequest.AcceptDate = DateTime.Now;
                else
                    friendRequest.DeclineDate = DateTime.Now;

                // try to set response
                if (_service.UpdateNetworkRequest(friendRequest))
                {
                    response = request.CreateResponse(HttpStatusCode.OK);
                    
                        
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.Conflict, _service.ValidationDic);
                }
            }// else request was not found
            else
            {
                response = request.CreateResponse(HttpStatusCode.BadRequest, "Connection Request was not found");
            }

            return response;
        }


        // POST
        // /api/ConnectionRequest/id

        public HttpResponseMessage Post(HttpRequestMessage request, int recipientId)
        {
            HttpResponseMessage response;

            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            ConnectionRequest friendRequest = new ConnectionRequest
            {
                RecipientId = recipientId,
                SenderId = companyId,
                SentDate = DateTime.Now
            };
            
            if (_service.SendNetworkRequest(friendRequest))
            {
                response = request.CreateResponse(HttpStatusCode.Created, friendRequest);
            }
            else
            {
                response = request.CreateResponse(HttpStatusCode.Conflict);
            }

            return response;
        }
    }
}
