using BCModel.Projects;
using BCWeb.Areas.Project.Models.BidPackage.ServiceLayer;
using BCWeb.Areas.Project.Models.BidPackage.ViewModel;
using BCWeb.Areas.Project.Models.Documents.ViewModel;
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
              
            // need to group by inviting company and bid date
            // this should handle auto-invite 3rd tier so that here are not 20 items for 1 company all with the same bid date

            viewModel.BidPackages = _service.GetBidPackagesByProjectAndInvitedCompany(projectId, companyId)
                .Select(x => new SubAndVendBidPackageListItem
                {
                    BidDateTime = x.UseProjectBidDateTime ? x.Project.BidDateTime.ToString() : x.BidDateTime.Value.ToString(), // FIXME
                    BidPackageId = x.Id,
                    InviteResponse = x.Invitees.Where(i => i.SentToId == companyId).FirstOrDefault().AcceptedDate.HasValue ? true : x.Invitees.Where(i => i.SentToId == companyId).FirstOrDefault().RejectedDate.HasValue ? false : default(bool?),
                    InvitingCompanyId = x.CreatedById,
                    InvitingCompanyName = x.CreatedBy.CompanyName,
                    SelectedScopes = x.Scopes.Select(s => s.ScopeId).ToArray(),
                    ProjectDocs = x.Project.ProjectDocuments.Where(p => p.CompanyId == x.CreatedById).AsEnumerable().Select(d => new ProjectDocLookupItem { Name = d.Name, Id = d.Id })
                }).ToArray();

            viewModel.Scopes = _service.GetInvitationScopes(projectId, companyId).ToArray();

            return viewModel;
        }
    }
}
