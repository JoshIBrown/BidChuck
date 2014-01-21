using BCModel;
using BCModel.SocialNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Contacts.Models.ServiceLayer
{
    public interface INetworkServiceLayer
    {
        bool SendNetworkRequest(ConnectionRequest request);
        bool UpdateNetworkRequest(ConnectionRequest request);
        bool CreateNetworkConnection(NetworkConnection connection);
        bool DeleteNetworkConnection(int companyA, int companyB);
        NetworkConnection GetNetworkConnection(int left, int right);
        ConnectionRequest GetNetworkRequest(Guid id);
        IEnumerable<NetworkConnection> GetCompaniesConnections(int companyId);
        IEnumerable<ConnectionRequest> GetSentRequests(int companyId);
        IEnumerable<ConnectionRequest> GetReceivedRequests(int companyId);

        UserProfile GetUserProfile(int id);
        CompanyProfile GetCompanyProfile(int id);
        Dictionary<string, string> ValidationDic { get; }
    }
}
