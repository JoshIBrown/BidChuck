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
        BidPackageXInvitee GetInvite(int id);
        IEnumerable<Scope> GetScopes();
        IEnumerable<Scope> GetScopes(Expression<Func<Scope, bool>> predicate);
        IEnumerable<BidPackageXScope> GetSelectedScopes();
        IEnumerable<BidPackageXScope> GetSelectedScopes(Expression<Func<BidPackageXScope, bool>> predicate);
        IEnumerable<BidPackageXInvitee> GetInvites();
        IEnumerable<CompanyProfile> GetCompanies();
        IEnumerable<BidPackageXInvitee> GetInvitesByCompany(int id);
        IEnumerable<BidPackageXInvitee> GetInvitesByBidPackage(int id);
        IEnumerable<BidPackageXInvitee> GetInvitesByCompany(Expression<Func<BidPackageXInvitee, bool>> predicate);
        IEnumerable<BCModel.Projects.BidPackage> GetByProject(int id);
        IEnumerable<BCModel.Projects.BidPackage> GetByCompany(int id);
        UserProfile GetUser(int id);
    }
}
