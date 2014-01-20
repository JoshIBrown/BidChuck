using BCModel;
using BCModel.SocialNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Company.Models.ServiceLayer
{
    public interface INetworkServiceLayer
    {
        bool SendNetworkRequest(NetworkRequest request);
        bool RespondToNetworkRequest(Guid id, RequestResponse response);
        bool CreateNetworkConnection(NetworkConnection connection);
        bool DeleteNetworkConnection(int companyA, int companyB);
        NetworkConnection GetNetworkConnection(int left, int right);
        NetworkRequest GetNetworkRequest(Guid id);
        IEnumerable<NetworkConnection> GetCompaniesConnections(int companyId);
        IEnumerable<NetworkRequest> GetSentRequests(int companyId);
        IEnumerable<NetworkRequest> GetReceivedRequests(int companyId);

        UserProfile GetUserProfile(int id);
        CompanyProfile GetCompanyProfile(int id);
        Dictionary<string, string> ValidationDic { get; }
    }
}
