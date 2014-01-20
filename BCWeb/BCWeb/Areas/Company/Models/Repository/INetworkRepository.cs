using BCModel;
using BCModel.SocialNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Company.Models.Repository
{
    public interface INetworkRepository
    {
        void AddNetworkConnection(NetworkConnection entity);
        void AddNetworkRequest(NetworkRequest entity);
        void UpdateNetworkRequest(NetworkRequest entity);
        void DeleteNetworkConnection(NetworkConnection entity);
        NetworkConnection FindNetworkConnection(params object[] key);
        NetworkRequest FindNetworkRequest(Guid id);
        IQueryable<NetworkConnection> QueryNetworkConnections();
        IQueryable<NetworkRequest> QueryNetworkRequests();

        CompanyProfile FindCompanyProfile(int id);
        IQueryable<CompanyProfile> QueryCompanyProfiles();
        UserProfile FindUserProfile(int id);

        void SaveChanges();
    }
}
