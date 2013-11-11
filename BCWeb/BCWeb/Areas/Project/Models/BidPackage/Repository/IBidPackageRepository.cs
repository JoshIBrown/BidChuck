using BCWeb.Models;
using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCModel;
using Microsoft.Ajax.Utilities;
using System.Linq.Expressions;

namespace BCWeb.Areas.Project.Models.BidPackage.Repository
{
    public interface IBidPackageRepository : IGenericRepository<BCModel.Projects.BidPackage>
    {
        BCModel.Projects.Project GetProject(int id);
        CompanyProfile GetCompany(int id);
        Invitation GetInvite(int bidPackageId, int companyId);
        IQueryable<Scope> QueryScopes();
        IQueryable<BidPackageXScope> QuerySelectedScopes();
        IQueryable<Invitation> QueryInvites();
        IQueryable<CompanyProfile> QueryCompanies();
        UserProfile GetUser(int id);
    }
}
