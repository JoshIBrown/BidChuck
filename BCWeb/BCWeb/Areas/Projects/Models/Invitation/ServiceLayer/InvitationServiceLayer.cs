using BCWeb.Areas.Projects.Models.Invitation.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Projects.Models.Invitation.ServiceLayer
{
    public class InvitationServiceLayer : IInvitationServiceLayer
    {

        private IInvitationRepository _repo;

        public InvitationServiceLayer(IInvitationRepository repo)
        {
            _repo = repo;
            ValidationDic = new Dictionary<string, string>();
        }


        public BCModel.UserProfile GetUerProfile(int id)
        {
            return _repo.GetUerProfile(id);
        }

        public BCModel.CompanyProfile GetCompanyProfile(int id)
        {
            return _repo.GetCompanyProfile(id);
        }

        public BCModel.Projects.BidPackage GetBidPackage(int id)
        {
            return _repo.GetBidPackage(id);
        }

        public BCModel.Projects.Project GetProject(int id)
        {
            return _repo.GetProject(id);
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        public bool Create(BCModel.Projects.BidPackageXInvitee entity)
        {
            try
            {
                _repo.Create(entity);
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool Update(BCModel.Projects.BidPackageXInvitee entity)
        {
            try
            {
                _repo.Update(entity);
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool Delete(BCModel.Projects.BidPackageXInvitee entity)
        {
            try
            {
                _repo.Delete(entity);
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {
                ValidationDic.Clear();
                ValidationDic.Add("Exception", ex.Message);
                return false;
            }
        }

        public bool Delete(int id)
        {
            return Delete(_repo.Get(id));
        }

        public IEnumerable<BCModel.Projects.BidPackageXInvitee> GetEnumerable()
        {
            return _repo.Query().AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.BidPackageXInvitee> GetEnumerable(System.Linq.Expressions.Expression<Func<BCModel.Projects.BidPackageXInvitee, bool>> predicate)
        {
            return _repo.Query().Where(predicate).AsEnumerable();
        }

        public BCModel.Projects.BidPackageXInvitee Get(int id)
        {
            return _repo.Get(id);
        }

        public bool Exists(int id)
        {
            return _repo.Get(id) == null;
        }
    }
}