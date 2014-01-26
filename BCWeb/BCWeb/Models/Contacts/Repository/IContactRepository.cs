using BCModel;
using BCModel.SocialNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models.Contacts.Repository
{
    public interface IContactRepository
    {
        void AddNetworkConnection(ContactConnection entity);
        void AddNetworkRequest(ContactRequest entity);
        void UpdateNetworkRequest(ContactRequest entity);
        void DeleteNetworkConnection(ContactConnection entity);
        void AddBlackList(BlackList entity);
        void DeleteBlackList(BlackList entity);
        ContactConnection FindNetworkConnection(params object[] key);
        ContactRequest FindNetworkRequest(Guid id);
        IQueryable<ContactConnection> QueryNetworkConnections();
        IQueryable<ContactRequest> QueryContactRequests();

        CompanyProfile FindCompanyProfile(int id);
        IQueryable<CompanyProfile> QueryCompanyProfiles();
        UserProfile FindUserProfile(int id);

        void SaveChanges();

        BlackList FindBlackList(params object[] key);
        IQueryable<BlackList> QueryBlackList();
    }
}
