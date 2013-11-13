using BCModel.Projects;
using BCWeb.Areas.Project.Models.BidPackage.ServiceLayer;
using BCWeb.Areas.Project.Models.BidPackage.ViewModel;
using BCWeb.Models;
using BCWeb.Models.GenericViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    [Authorize]
    public class BidPackageController : ApiController
    {
        private IBidPackageServiceLayer _service;
        private IWebSecurityWrapper _security;
        public BidPackageController(IBidPackageServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        public IEnumerable<BidPackage> GetMyCompanyBidPackages()
        {
            var user = _service.GetUser(_security.GetUserId(User.Identity.Name));
            //var company = _service.GetCompany(user.CompanyId);
            var packages = _service.GetByCompany(user.CompanyId);

            return packages;
        }

        
        public SubAndVendBidPackageAngularModel GetInvitedPackagesForProject(int projectId)
        {
            int companyId = _service.GetUser(_security.GetUserId(User.Identity.Name)).CompanyId;

            SubAndVendBidPackageAngularModel viewModel = new SubAndVendBidPackageAngularModel();

            viewModel.BidPackages = _service.GetEnumerableByProjectAndInvitedCompany(projectId, companyId)
                .Select(x => new SubAndVendBidPackageListItem
                {
                    BidDateTime = x.BidDateTime,
                    BidPackageId = x.Id,
                    InviteResponse = x.Invitees.Where(i => i.SentToId == companyId).FirstOrDefault().AcceptedDate.HasValue ? true : x.Invitees.Where(i => i.SentToId == companyId).FirstOrDefault().RejectedDate.HasValue ? false : default(bool?),
                    InvitingCompanyId = x.CreatedById,
                    InvitingCompanyName = x.CreatedBy.CompanyName,
                    SelectedScopes = x.Scopes.Select(s => s.ScopeId).ToArray()
                }).ToArray();

            viewModel.Scopes = _service.GetInvitationScopes(projectId, companyId).ToArray();

            //IEnumerable<Invitation> invites = _service.GetInvitations(theProject.Id, user.CompanyId);
            //Dictionary<int, string> bidDates = invites.ToDictionary(i => i.BidPackage.CreatedById, i => i.BidPackage.BidDateTime.ToShortDateString());
            //Dictionary<int, IEnumerable<int>> scopeselection = _service.GetInvitationScopesByInvitingCompany(theProject.Id, user.CompanyId);
            //Dictionary<int, string> inviters = _service.GetInvitatingCompanies(theProject.Id, user.CompanyId);
            //Dictionary<int, string> scopes = _service.GetInvitationScopes(theProject.Id, user.CompanyId);
            //Dictionary<int, bool?> inviteResponses = invites.ToDictionary(i => i.BidPackage.CreatedById, i => i.AcceptedDate.HasValue ? true : i.RejectedDate.HasValue ? false : default(bool?));
            return viewModel;
        }
    }
}
