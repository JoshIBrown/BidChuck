using BCModel.SocialNetwork;
using BCWeb.Models.Contacts.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.Notifications.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Attributes;
using BCWeb.Models.Contacts.ViewModel;

namespace BCWeb.Api
{
    [Authorize]
    public class ContactRequestController : ApiController
    {
        private IContactServiceLayer _service;
        private IWebSecurityWrapper _security;
        private INotificationSender _notice;

        public ContactRequestController(IContactServiceLayer service, IWebSecurityWrapper security, INotificationSender notice)
        {
            _service = service;
            _security = security;
            _notice = notice;
        }


        // GET: return a list of connection requests
        // /api/ConnectionRequest
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            List<ContactRequestItem> result = new List<ContactRequestItem>();

            int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            List<ContactRequestItem> sent = _service.GetSentRequests(companyId).Select(s => new ContactRequestItem { CompanyId = s.RecipientId, CompanyName = s.Recipient.CompanyName, Type = ContactRequestType.sent }).ToList();

            if (sent != null)
                result.AddRange(sent);

            List<ContactRequestItem> rcvd = _service.GetReceivedRequests(companyId).Select(s => new ContactRequestItem { CompanyId = s.SenderId, CompanyName = s.Sender.CompanyName, Type = ContactRequestType.recvd }).ToList();

            if (rcvd != null)
                result.AddRange(rcvd);


            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        // PUT
        // /api/ConnectionRequest/id?accept=true/false
        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage Put(HttpRequestMessage request, int senderId, bool accept)
        {
            HttpResponseMessage response;

            if (senderId == null || accept == null)
            {
                response = request.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            int recipientId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            ContactRequest friendRequest = _service.GetOpenNetworkRequest(recipientId, senderId);
            ContactConnection connection = _service.GetNetworkConnection(senderId, recipientId);

            // if companies are already connected
            if (connection != null)
            {
                response = request.CreateResponse(HttpStatusCode.Conflict, "already connected to this company");
                return response;
            }

            // if trying to connect to self
            if (recipientId == senderId)
            {
                response = request.CreateResponse(HttpStatusCode.Conflict, "cannot connect to self");
            }

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
                    // if declined to connect or accpeted connection and network connection was created succesfully
                    if (!accept || (accept && _service.AddNetworkConnection(new ContactConnection { LeftId = friendRequest.SenderId, RightId = friendRequest.RecipientId, CreateDate = friendRequest.AcceptDate.Value })))
                    {
                        // send notification
                        _notice.SendNotification(friendRequest.SenderId, RecipientType.company, BCModel.NotificationType.ConnectionAccepted, friendRequest.RecipientId, BCModel.EntityType.Company);

                        response = request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.Conflict, _service.ValidationDic);
                }
            }// else request was not found
            else
            {
                response = request.CreateResponse(HttpStatusCode.BadRequest, "Contact Request was not found");
            }

            return response;
        }


        // POST
        // /api/ConnectionRequest/id
        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage Post(HttpRequestMessage request, int recipientId)
        {
            HttpResponseMessage response;

            int senderId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            ContactRequest existingRequest = _service.GetOpenNetworkRequest(recipientId, senderId);
            ContactConnection connection = _service.GetNetworkConnection(senderId, recipientId);
            BlackList blacklistThisSide = _service.GetBlackListItem(senderId, recipientId);

            // if either company has blocked the other
            if (blacklistThisSide != null)
            {
                response = request.CreateResponse(HttpStatusCode.Conflict, "must de-blacklist company first before sending a contact request");
                return response;
            }

            if (existingRequest != null)
            {
                response = request.CreateResponse(HttpStatusCode.Conflict, "reqeust has already been sent");
                return response;
            }

            if (connection != null)
            {
                response = request.CreateResponse(HttpStatusCode.Conflict, "already connected to this contact");
                return response;
            }

            if (senderId == recipientId)
            {
                response = request.CreateResponse(HttpStatusCode.Conflict, "cannot connect to self");
                return response;
            }

            ContactRequest friendRequest = new ContactRequest
            {
                RecipientId = recipientId,
                SenderId = senderId,
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

        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage Delete(HttpRequestMessage request, int recipientId)
        {
            int senderId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
            ContactRequest pendingInvite = _service.GetOpenNetworkRequest(recipientId, senderId);

            if (pendingInvite.AcceptDate.HasValue || pendingInvite.DeclineDate.HasValue)
            {
                return request.CreateResponse(HttpStatusCode.NoContent, "invite has already been responded to. cannot delete");
            }

            if (_service.CancelNetworkRequest(pendingInvite))
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
