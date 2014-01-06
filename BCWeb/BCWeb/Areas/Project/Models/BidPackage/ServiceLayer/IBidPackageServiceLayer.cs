using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Project.Models.BidPackage.ServiceLayer
{
    public interface IBidPackageServiceLayer : IGenericServiceLayer<BCModel.Projects.BidPackage>
    {
        BCModel.Projects.Project GetProject(int id);
        CompanyProfile GetCompany(int id);
        Invitation GetInvite(int bidPackageId, int companyId);
        IEnumerable<Scope> GetScopes();
        IEnumerable<Scope> GetScopes(Expression<Func<Scope, bool>> predicate);
        IEnumerable<BidPackageXScope> GetSelectedScopes();
        IEnumerable<BidPackageXScope> GetSelectedScopes(Expression<Func<BidPackageXScope, bool>> predicate);
        IEnumerable<Invitation> GetInvites();
        IEnumerable<Invitation> GetCompanyInvitesForProject(int projectId, int invitedCompanyId);
        IEnumerable<CompanyProfile> GetCompanies();
        IEnumerable<Invitation> GetInvitesByCompany(int id);
        IEnumerable<Invitation> GetInvitesByBidPackage(int id);
        IEnumerable<Invitation> GetInvitesByCompany(Expression<Func<Invitation, bool>> predicate);
        IEnumerable<BCModel.Projects.BidPackage> GetByProject(int id);
        IEnumerable<BCModel.Projects.BidPackage> GetByCompany(int id);
        UserProfile GetUser(int id);
        IEnumerable<BCModel.Projects.BidPackage> GetEnumerableByProject(int projectId);
        IEnumerable<BCModel.Projects.BidPackage> GetEnumerableByProjectAndCreatingCompany(int projectId, int creatingCompanyId);
        IEnumerable<BCModel.Projects.BidPackage> GetBidPackagesByProjectAndInvitedCompany(int projectId, int invitedCompanyId);
        Dictionary<int, string> GetInvitationScopes(int projectId, int companyId);

        
    }
}
