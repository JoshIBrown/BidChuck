using BCWeb.Areas.Project.Models.Documents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Documents.ServiceLayer
{
    public class ProjectDocServiceLayer : IProjectDocServiceLayer
    {
        private IProjectDocRepository _repo;

        public ProjectDocServiceLayer(IProjectDocRepository repo)
        {
            _repo = repo;
            ValidationDic = new Dictionary<string, string>();
        }

        public BCModel.Projects.Project GetProject(int id)
        {
            return _repo.FindProject(id);
        }

        public BCModel.CompanyProfile GetCompany(int id)
        {
            return _repo.FindCompanyProfile(id);
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        public bool Create(BCModel.Projects.ProjectDocument entity)
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

        public bool Update(BCModel.Projects.ProjectDocument entity)
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

        public bool Delete(BCModel.Projects.ProjectDocument entity)
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

        public bool Delete(params object[] key)
        {
            try
            {
                _repo.Delete(key);
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

        public IEnumerable<BCModel.Projects.ProjectDocument> GetEnumerable()
        {
            return _repo.Query().AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.ProjectDocument> GetEnumerable(System.Linq.Expressions.Expression<Func<BCModel.Projects.ProjectDocument, bool>> predicate)
        {
            return _repo.Query().Where(predicate).AsEnumerable();
        }

        public BCModel.Projects.ProjectDocument Get(params object[] key)
        {
            return _repo.Get(key);
        }

        public bool Exists(params object[] key)
        {
            return _repo.Get(key) == null;
        }


        public BCModel.UserProfile GetUser(int id)
        {
            return _repo.FindUser(id);
        }



        public bool UserIsInvitedToProject(int companyId,int projectId)
        {
            // should only check the master bid package
            // because that is where the invite for a general contractor will be
            var result = from i in _repo.QueryInvites()
                         where i.BidPackage.ProjectId == projectId
                         && i.SentToId == companyId
                         select i;

            return result != null || result.Count() > 0;
        }
    }
}