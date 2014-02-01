using BCModel;
using BCModel.Projects;
using BCModel.SocialNetwork;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Account.Models.Company.Repository
{
    public interface ICompanyProfileRepository : IGenericRepository<CompanyProfile>
    {
        IQueryable<State> QueryStates();
        IQueryable<UserProfile> QueryUserProfiles();
        IQueryable<ContactConnection> QueryNetworkConnections();
        IQueryable<CompanyXScope> QueryCompanyScopes();
        UserProfile GetUserProfile(int id);

        IQueryable<ContactRequest> QueryConnectionRequests();
        IQueryable<BlackList> QueryBlackLists();

        BCModel.Projects.Project FindProject(int id);

        BidPackage FindBidPackage(int id);
    }
}
