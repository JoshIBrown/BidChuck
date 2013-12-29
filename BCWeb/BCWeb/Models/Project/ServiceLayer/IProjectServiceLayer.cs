using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCModel;
using BCModel.Projects;
using System.Linq.Expressions;

namespace BCWeb.Models.Project.ServiceLayer
{
    public interface IProjectServiceLayer : IGenericServiceLayer<BCModel.Projects.Project>
    {
        IEnumerable<BuildingType> GetBuildingTypes();
        IEnumerable<ConstructionType> GetConstructionTypes();
        //IEnumerable<ProjectType> GetProjectTypes();
        IEnumerable<State> GetStates();

        CompanyProfile GetCompanyProfile(int id);
        UserProfile GetUserProfile(int id);
        BidPackage GetMasterBidPackage(int projectId);

        IEnumerable<CompanyProfile> GetCompanyProfiles();
        IEnumerable<CompanyProfile> GetArchitects();
        IEnumerable<CompanyProfile> GetGeneralContractors();
        IEnumerable<CompanyProfile> GetArchitectsAndGenContractors();
        IEnumerable<CompanyProfile> GetCompanyProfiles(Expression<Func<CompanyProfile, bool>> predicate);

        IEnumerable<Invitation> GetInvitations(int companyId);
        IEnumerable<Invitation> GetRcvdInvitations(int projectId, int companyId);
        IEnumerable<Invitation> GetSentInvitations(int projectId, int companyId);

        IEnumerable<ProjectDocument> GetDocuments(int projectId, int companyId);

        Dictionary<int, string> GetInvitationScopes(int projectId, int invitedCompanyId);
        Dictionary<int, string> GetInvitatingCompanies(int projectId, int invitedCompanyId);
        Dictionary<int, IEnumerable<int>> GetInvitationScopesByInvitingCompany(int projectId, int invitedCompanyId);
        IEnumerable<BCModel.Projects.Project> FindDuplicate(string title, string number, int architectId);
        IEnumerable<BCModel.Projects.Project> FindDuplicate(string title, string number);

        IEnumerable<UserProfile> GetArchitectsAndGenContractorUsers();

        State GetState(int id);
    }
}
