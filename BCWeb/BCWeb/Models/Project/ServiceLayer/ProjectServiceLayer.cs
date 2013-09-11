using BCWeb.Models.Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Project.ServiceLayer
{
    public class ProjectServiceLayer : IProjectServiceLayer
    {
        private IProjectRepository _repo;

        public ProjectServiceLayer(IProjectRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<BCModel.Projects.BuildingType> GetBuildingTypes()
        {
            return _repo.QueryBuildingType().AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.ConstructionType> GetConstructionTypes()
        {
            return _repo.QueryConstructionType().AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.ProjectType> GetProjectTypes()
        {
            return _repo.QueryProjectType().AsEnumerable();
        }

        public IEnumerable<BCModel.State> GetStates()
        {
            return _repo.QueryStates().AsEnumerable();
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        public bool Create(BCModel.Projects.Project entity)
        {
            try
            {
                // todo: validate
                _repo.Create(entity);
                _repo.Save();
                return true;
            }
            catch (Exception ex)
            {

                ValidationDic.Clear();
                ValidationDic.Add("exception", ex.Message);
                return false;
            }
        }

        public bool Update(BCModel.Projects.Project entity)
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
                ValidationDic.Add("exception", ex.Message);
                return false;
            }
        }

        public bool Delete(BCModel.Projects.Project entity)
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
                ValidationDic.Add("exception", ex.Message);
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
                ValidationDic.Add("exception", ex.Message);
                return false;
            }
        }

        public IEnumerable<BCModel.Projects.Project> GetEnumerable()
        {
            return _repo.Query().AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.Project> GetEnumerable(System.Linq.Expressions.Expression<Func<BCModel.Projects.Project, bool>> predicate)
        {
            return _repo.Query().Where(predicate).AsEnumerable();
        }

        public BCModel.Projects.Project Get(int id)
        {
            return _repo.Get(id);
        }

        public bool Exists(int id)
        {
            return _repo.Get(id) == null;
        }
    }
}