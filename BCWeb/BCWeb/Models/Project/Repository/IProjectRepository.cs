using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCModel;
using BCModel.Projects;

namespace BCWeb.Models.Project.Repository
{
    public interface IProjectRepository : IGenericRepository<BCModel.Projects.Project>
    {
        IQueryable<ConstructionType> QueryConstructionType();
        IQueryable<BuildingType> QueryBuildingType();
        //IQueryable<ProjectType> QueryProjectType();
        IQueryable<State> QueryStates();

        UserProfile GetUserProfile(int id);

        CompanyProfile GetCompanyProfile(int id);
        IQueryable<CompanyProfile> QueryCompanyProfiles();

        IQueryable<Invitation> QueryInvites();
        IQueryable<BidPackage> QueryBidPackages();
        IQueryable<BidPackageXScope> QueryBidPackageScopes();
        IQueryable<Scope> QueryScopes();
        IQueryable<ProjectDocument> QueryDocuments();
        IQueryable<UserProfile> QueryUserProfiles();
        
    }
}
