using BCModel;
using BCModel.SocialNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Contacts.Models.Repository
{
    public interface INetworkRepository
    {
        void AddNetworkConnection(NetworkConnection entity);
        void AddNetworkRequest(ConnectionRequest entity);
        void UpdateNetworkRequest(ConnectionRequest entity);
        void DeleteNetworkConnection(NetworkConnection entity);
        NetworkConnection FindNetworkConnection(params object[] key);
        ConnectionRequest FindNetworkRequest(Guid id);
        IQueryable<NetworkConnection> QueryNetworkConnections();
        IQueryable<ConnectionRequest> QueryNetworkRequests();

        CompanyProfile FindCompanyProfile(int id);
        IQueryable<CompanyProfile> QueryCompanyProfiles();
        UserProfile FindUserProfile(int id);

        void SaveChanges();
    }
}
