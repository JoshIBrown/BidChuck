using BCModel;
using BCModel.Projects;
using BCWeb.Areas.Account.Models.Company.ViewModel;
using BCWeb.Areas.Project.Models.Invitations.ServiceLayer;
using BCWeb.Helpers;
using BCWeb.Models;
using BCWeb.Models.GenericViewModel;
using BCWeb.Models.Notifications.ServiceLayer;
using BCWeb.Models.Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Attributes;

namespace BCWeb.Api
{
    [Authorize]
    public class InvitationController : ApiController
    {
        private IInvitationServiceLayer _service;
        private IWebSecurityWrapper _security;
        private INotificationSender _notice;

        public InvitationController(IInvitationServiceLayer service, IWebSecurityWrapper security, INotificationSender notice)
        {
            _service = service;
            _security = security;
            _notice = notice;
        }


        public IEnumerable<CompanyScopeItem> GetCompaniesToInvite(int bidPackageId)
        {
            int[] bpScopes = _service.GetBidPackageScopesDeepestScopes(bidPackageId).Select(s => s.Id).ToArray();
            IEnumerable<CompanyScopeItem> result = _service.GetBestFitCompanies(bidPackageId, true)
                .Select(c => new CompanyScopeItem
                {
                    BusinessType = c.BusinessType.ToDescription(),
                    CompanyId = c.Id,
                    CompanyName = c.CompanyName,
                    ScopesOfWork = c.Scopes.Where(s => bpScopes.Contains(s.ScopeId))
                        .Select(s => s.Scope.CsiNumber + " " + s.Scope.Description)
                })
                .OrderByDescending(o => o.ScopesOfWork.Count());




            return result;
        }
    }
}
