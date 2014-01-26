using BCModel.SocialNetwork;
using BCWeb.Models.Contacts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Contacts.ServiceLayer
{
    public enum RequestResponse
    {
        Accept, Decline
    }

    public class ContactServiceLayer : IContactServiceLayer
    {
        private IContactRepository _repo;

        public ContactServiceLayer(IContactRepository repo)
        {
            _repo = repo;
            ValidationDic = new Dictionary<string, string>();
        }


        private bool validateRequest(ContactRequest request)
        {
            bool valid = true;
            ValidationDic.Clear();

            // get requests sent by company a to company b that have not been responded to yet
            int openRequests = _repo.QueryContactRequests()
                                    .Where(r => ((r.SenderId == request.SenderId && r.RecipientId == request.RecipientId) || (r.SenderId == request.RecipientId && r.RecipientId == request.SenderId)) &&
                                        !r.AcceptDate.HasValue &&
                                        !r.DeclineDate.HasValue)
                                    .Count();

            if (openRequests > 0)
            {
                valid = false;
                ValidationDic.Add("Request", "There is still a pending request");
            }

            // check if two companies are already connected
            int existingConnection = _repo.QueryNetworkConnections()
                                            .Where(c => (c.LeftId == request.SenderId && c.RightId == request.RecipientId) ||
                                                (c.LeftId == request.RecipientId && c.RightId == request.SenderId))
                                            .Count();

            if (existingConnection > 0)
            {
                valid = false;
                ValidationDic.Add("Connection", "Already connected to company");
            }

            return valid;
        }

        public bool SendNetworkRequest(BCModel.SocialNetwork.ContactRequest request)
        {
            try
            {
                if (validateRequest(request))
                {
                    _repo.AddNetworkRequest(request);
                    _repo.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }


        public bool UpdateNetworkRequest(ContactRequest request)
        {
            try
            {
                _repo.UpdateNetworkRequest(request);
                _repo.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        private bool validateConnection(ContactConnection connection)
        {
            bool valid = true;
            ValidationDic.Clear();

            int existingConnections = _repo.QueryNetworkConnections().Where(c => (c.LeftId == connection.LeftId && c.RightId == connection.RightId) || (c.RightId == connection.LeftId && c.LeftId == connection.RightId)).Count();

            if (existingConnections > 0)
            {
                valid = false;
                ValidationDic.Add("Connection", "These Companies are already connected.");
            }

            return valid;
        }

        public bool CreateNetworkConnection(BCModel.SocialNetwork.ContactConnection connection)
        {
            try
            {

                if (validateConnection(connection))
                {
                    _repo.AddNetworkConnection(connection);
                    _repo.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool DeleteNetworkConnection(int companyA, int companyB)
        {
            try
            {
                ContactConnection connection = _repo.FindNetworkConnection(companyA, companyB);

                if (connection == null)
                    connection = _repo.FindNetworkConnection(companyB, companyA);

                if (connection == null)
                {
                    ValidationDic.Clear();
                    ValidationDic.Add("Connection", "No connection to delete");
                    return false;
                }
                else
                {
                    _repo.DeleteNetworkConnection(connection);
                    _repo.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public BCModel.SocialNetwork.ContactConnection GetNetworkConnection(int left, int right)
        {

            ContactConnection conn = _repo.FindNetworkConnection(left, right);

            if (conn == null)
                conn = _repo.FindNetworkConnection(right, left);

            return conn;
        }

        public BCModel.SocialNetwork.ContactRequest GetNetworkRequest(Guid id)
        {
            return _repo.FindNetworkRequest(id);
        }

        public IEnumerable<BCModel.SocialNetwork.ContactConnection> GetCompaniesConnections(int companyId)
        {
            return _repo.QueryNetworkConnections().Where(c => c.RightId == companyId || c.LeftId == companyId);
        }

        public IEnumerable<BCModel.SocialNetwork.ContactRequest> GetSentRequests(int companyId)
        {
            return _repo.QueryContactRequests().Where(r => r.SenderId == companyId);
        }

        public IEnumerable<BCModel.SocialNetwork.ContactRequest> GetReceivedRequests(int companyId)
        {
            return _repo.QueryContactRequests().Where(r => r.RecipientId == companyId);
        }

        public BCModel.UserProfile GetUserProfile(int id)
        {
            return _repo.FindUserProfile(id);
        }

        public BCModel.CompanyProfile GetCompanyProfile(int id)
        {
            return _repo.FindCompanyProfile(id);
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }


        public ContactRequest GetOpenNetworkRequest(int recipientId, int senderId)
        {
            return _repo.QueryContactRequests().Where(r => r.RecipientId == recipientId && r.SenderId == senderId && !r.AcceptDate.HasValue && !r.DeclineDate.HasValue).FirstOrDefault();
        }


        public BlackList GetBlackListItem(int senderId, int recipientId)
        {
            _repo.QueryBlackList();
            throw new NotImplementedException();
        }


        public bool BlackListCompany(int companyId, int companyToBlockId)
        {
            throw new NotImplementedException();
        }

        public bool UnblackListCompany(int companyId, int blockedCompanyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BlackList> GetBlackListForCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        public ConnectionStatus GetConnectionStatus(int currentCompany, int queriedCompany)
        {
            var connection = _repo.QueryNetworkConnections()
                .Where(x => (x.LeftId == currentCompany && x.RightId == queriedCompany) || (x.RightId == currentCompany && x.LeftId == queriedCompany))
                .SingleOrDefault();

            if (connection != null)
                return ConnectionStatus.Connected;

            var sentInvite = _repo.QueryContactRequests()
                .Where(x => x.SenderId == currentCompany && x.RecipientId == queriedCompany && !x.AcceptDate.HasValue && !x.DeclineDate.HasValue)
                .SingleOrDefault();

            if (sentInvite != null)
                return ConnectionStatus.InvitationSent;

            var recvdInvite = _repo.QueryContactRequests()
                .Where(x => x.RecipientId == currentCompany && x.SenderId == queriedCompany && !x.AcceptDate.HasValue && !x.DeclineDate.HasValue)
                .SingleOrDefault();

            if (recvdInvite != null)
                return ConnectionStatus.InvitationPending;

            var blackList = _repo.QueryBlackList()
                .Where(x => (x.BlackListedCompanyId == currentCompany && x.CompanyId == queriedCompany) || (x.CompanyId == currentCompany && x.BlackListedCompanyId == queriedCompany))
                .FirstOrDefault();

            if (blackList != null)
                return ConnectionStatus.BlackListed;

            if (currentCompany == queriedCompany)
                return ConnectionStatus.Self;

            return ConnectionStatus.NotConnected;
        }
    }
}