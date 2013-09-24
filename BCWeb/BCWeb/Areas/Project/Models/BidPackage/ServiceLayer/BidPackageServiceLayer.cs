using BCModel.Projects;
using BCWeb.Areas.Project.Models.BidPackage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BCWeb.Areas.Project.Models.BidPackage.ServiceLayer
{
    public class BidPackageServiceLayer : IBidPackageServiceLayer
    {
        private IBidPackageRepository _repo;

        public BidPackageServiceLayer(IBidPackageRepository repo)
        {
            _repo = repo;
            ValidationDic = new Dictionary<string, string>();
        }


        public BCModel.Projects.Project GetProject(int id)
        {
            return _repo.GetProject(id);
        }

        public BCModel.CompanyProfile GetCompany(int id)
        {
            return _repo.GetCompany(id);
        }

        public BCModel.Projects.BidPackageXInvitee GetInvite(int id)
        {
            return _repo.GetInvite(id);
        }

        public IEnumerable<BCModel.Scope> GetScopes()
        {
            return _repo.QueryScopes().AsEnumerable();
        }


        public IEnumerable<BCModel.Scope> GetScopes(System.Linq.Expressions.Expression<Func<BCModel.Scope, bool>> predicate)
        {
            return _repo.QueryScopes().Where(predicate).AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.BidPackageXScope> GetSelectedScopes()
        {
            return _repo.QuerySelectedScopes().AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.BidPackageXScope> GetSelectedScopes(Expression<Func<BidPackageXScope, bool>> predicate)
        {
            return _repo.QuerySelectedScopes().Where(predicate).AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.BidPackageXInvitee> GetInvites()
        {
            return _repo.QueryInvites().AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.BidPackageXInvitee> GetInvitesByCompany(int id)
        {
            return (from r in _repo.QueryInvites()
                    where r.CompanyId == id
                    select r).AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.BidPackageXInvitee> GetInvitesByBidPackage(int id)
        {
            return (from r in _repo.QueryInvites()
                    where r.BidPackageId == id
                    select r).AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.BidPackageXInvitee> GetInvitesByCompany(System.Linq.Expressions.Expression<Func<BCModel.Projects.BidPackageXInvitee, bool>> predicate)
        {
            return _repo.QueryInvites().Where(predicate).AsEnumerable();
        }

        public IEnumerable<BCModel.CompanyProfile> GetCompanies()
        {
            return _repo.QueryCompanies().AsEnumerable();
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        public bool Create(BCModel.Projects.BidPackage entity)
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

        public bool Update(BCModel.Projects.BidPackage entity)
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

        public bool Delete(BCModel.Projects.BidPackage entity)
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
            try
            {
                _repo.Delete(id);
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

        public IEnumerable<BCModel.Projects.BidPackage> GetEnumerable()
        {
            return _repo.Query().AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.BidPackage> GetByProject(int id)
        {
            return (from r in _repo.Query()
                    where r.ProjectId == id
                    select r).AsEnumerable();
        }


        public IEnumerable<BCModel.Projects.BidPackage> GetEnumerable(System.Linq.Expressions.Expression<Func<BCModel.Projects.BidPackage, bool>> predicate)
        {
            return _repo.Query().Where(predicate).AsEnumerable();
        }

        public BCModel.Projects.BidPackage Get(int id)
        {
            return _repo.Get(id);
        }

        public bool Exists(int id)
        {
            return _repo.Get(id) == null;
        }


        public BCModel.UserProfile GetUser(int id)
        {
            return _repo.GetUser(id);
        }


        public IEnumerable<BCModel.Projects.BidPackage> GetByCompany(int id)
        {
            return (from r in _repo.Query()
                    where r.CreatedById == id
                    select r).AsEnumerable();
        }


        public IEnumerable<BCModel.Projects.BidPackage> GetEnumerableByProject(int projectId)
        {
            return (from r in _repo.Query()
                    where r.ProjectId == projectId
                    select r).ToList();
        }


        public IEnumerable<BCModel.Projects.BidPackage> GetEnumerableByCompanyAndProject(int companyId, int projectId)
        {
            return (from r in _repo.Query()
                    where r.ProjectId == projectId
                    && r.CreatedById == companyId
                    select r).ToList();
        }
    }
}