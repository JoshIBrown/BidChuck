using BCModel;
using BCModel.SocialNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models.Contacts.ServiceLayer
{
    public interface IContactServiceLayer
    {
        bool SendNetworkRequest(ContactRequest request);
        bool UpdateNetworkRequest(ContactRequest request);
        bool CancelNetworkRequest(ContactRequest openInvite);
        bool AddNetworkConnection(ContactConnection connection);
        bool RemoveNetworkConnection(ContactConnection connection);
        bool BlackListCompany(BlackList entity);
        bool UnblackListCompany(BlackList entity);

        ContactConnection GetNetworkConnection(int left, int right);
        ContactRequest GetNetworkRequest(Guid id);
        IEnumerable<ContactConnection> GetCompaniesConnections(int companyId);
        IEnumerable<ContactRequest> GetSentRequests(int companyId);
        IEnumerable<ContactRequest> GetReceivedRequests(int companyId);

        UserProfile GetUserProfile(int id);
        CompanyProfile GetCompanyProfile(int id);
        Dictionary<string, string> ValidationDic { get; }

        ContactRequest GetOpenNetworkRequest(int recipientId, int senderId);

        BlackList GetBlackListItem(int companyId, int blockedCompanyId);
        IEnumerable<BlackList> GetBlackListForCompany(int companyId);
        ConnectionStatus GetConnectionStatus(int currentCompany, int queriedCompany);

        
    }
}
