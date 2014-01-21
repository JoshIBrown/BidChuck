using BCModel;
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
        IQueryable<NetworkConnection> QueryNetworkConnections();

        UserProfile GetUserProfile(int id);

        IQueryable<ConnectionRequest> QueryConnectionRequests();
        IQueryable<BlackList> QueryBlackLists();
    }
}
