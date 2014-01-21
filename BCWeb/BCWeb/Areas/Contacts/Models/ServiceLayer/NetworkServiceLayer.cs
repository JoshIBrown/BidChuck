using BCModel.SocialNetwork;
using BCWeb.Areas.Contacts.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Contacts.Models.ServiceLayer
{
    public enum RequestResponse
    {
        Accept, Decline
    }

    public class NetworkServiceLayer : INetworkServiceLayer
    {
        private INetworkRepository _repo;

        public NetworkServiceLayer(INetworkRepository repo)
        {
            _repo = repo;
            ValidationDic = new Dictionary<string, string>();
        }


        private bool validateRequest(ConnectionRequest request)
        {
            bool valid = true;
            ValidationDic.Clear();

            // get requests sent by company a to company b that have not been responded to yet
            int openRequests = _repo.QueryNetworkRequests()
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

        public bool SendNetworkRequest(BCModel.SocialNetwork.ConnectionRequest request)
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


        public bool UpdateNetworkRequest(ConnectionRequest request)
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

        private bool validateConnection(NetworkConnection connection)
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

        public bool CreateNetworkConnection(BCModel.SocialNetwork.NetworkConnection connection)
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
                NetworkConnection connection = _repo.FindNetworkConnection(companyA, companyB);

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

        public BCModel.SocialNetwork.NetworkConnection GetNetworkConnection(int left, int right)
        {

            NetworkConnection conn = _repo.FindNetworkConnection(left, right);

            if (conn == null)
                conn = _repo.FindNetworkConnection(right,left);

            return conn;
        }

        public BCModel.SocialNetwork.ConnectionRequest GetNetworkRequest(Guid id)
        {
            return _repo.FindNetworkRequest(id);
        }

        public IEnumerable<BCModel.SocialNetwork.NetworkConnection> GetCompaniesConnections(int companyId)
        {
            return _repo.QueryNetworkConnections().Where(c => c.RightId == companyId || c.LeftId == companyId);
        }

        public IEnumerable<BCModel.SocialNetwork.ConnectionRequest> GetSentRequests(int companyId)
        {
            return _repo.QueryNetworkRequests().Where(r => r.SenderId == companyId);
        }

        public IEnumerable<BCModel.SocialNetwork.ConnectionRequest> GetReceivedRequests(int companyId)
        {
            return _repo.QueryNetworkRequests().Where(r => r.RecipientId == companyId);
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
    }
}