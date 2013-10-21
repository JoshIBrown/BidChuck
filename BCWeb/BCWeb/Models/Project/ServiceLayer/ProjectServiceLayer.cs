﻿using BCModel.Projects;
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
            ValidationDic = new Dictionary<string, string>();
        }

        public IEnumerable<BCModel.Projects.BuildingType> GetBuildingTypes()
        {
            return _repo.QueryBuildingType().AsEnumerable();
        }

        public IEnumerable<BCModel.Projects.ConstructionType> GetConstructionTypes()
        {
            return _repo.QueryConstructionType().AsEnumerable();
        }

        //public IEnumerable<BCModel.Projects.ProjectType> GetProjectTypes()
        //{
        //    return _repo.QueryProjectType().AsEnumerable();
        //}

        public IEnumerable<BCModel.State> GetStates()
        {
            return _repo.QueryStates().AsEnumerable();
        }

        public Dictionary<string, string> ValidationDic
        {
            get;
            private set;
        }

        private bool validateEntity(BCModel.Projects.Project entity)
        {
            bool valid = true;
            ValidationDic.Clear();

            // TODO: ADD LOGIC
            // if architect, address and city, title combo already exists
            if (_repo.Query().Where(r => r.Address.ToLower().Trim() == entity.Address.ToLower().Trim()
                && r.City.ToLower().Trim() == entity.City.ToLower().Trim()
                //&& r.Architect.Trim().ToLower() == entity.Architect.Trim().ToLower()
                && r.Title.ToLower().Trim() == entity.Title.ToLower().Trim()).Count() > 0)
            {
                valid = false;
                ValidationDic.Add("City", "");
                ValidationDic.Add("Title", "");
                ValidationDic.Add("Architect", "");
                ValidationDic.Add("Address", "");
                ValidationDic.Add("unique", "A Project by this architect, with this title at this location already exists");
            }



            return valid;
        }

        public bool Create(BCModel.Projects.Project entity)
        {
            try
            {
                if (validateEntity(entity))
                {
                    _repo.Create(entity);
                    _repo.Save();
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


        public BCModel.CompanyProfile GetCompanyProfile(int id)
        {
            return _repo.GetCompanyProfile(id);
        }

        public BCModel.UserProfile GetUserProfile(int id)
        {
            return _repo.GetUserProfile(id);
        }

        public IEnumerable<BCModel.CompanyProfile> GetCompanyProfiles()
        {
            return _repo.GetCompanyProfiles();
        }

        public IEnumerable<BCModel.CompanyProfile> GetCompanyProfiles(System.Linq.Expressions.Expression<Func<BCModel.CompanyProfile, bool>> predicate)
        {
            return _repo.GetCompanyProfiles().Where(predicate);
        }


        public IEnumerable<BCModel.Projects.BidPackageXInvitee> GetInvitations(int projectId, int companyId)
        {
            IEnumerable<BidPackageXInvitee> Invites = from r in _repo.QueryInvites()
                                                      where r.BidPackage.ProjectId == projectId
                                                      && r.CompanyId == companyId
                                                      select r;
            return Invites.ToList();
        }


        public IEnumerable<BidPackageXInvitee> GetInvitations(int companyId)
        {
            IEnumerable<BidPackageXInvitee> Invites = from r in _repo.QueryInvites()
                                                      where r.CompanyId == companyId
                                                      select r;
            return Invites.ToList();
        }


        public Dictionary<int, string> GetInvitationScopes(int projectId, int invitedCompanyId)
        {
            var output = (from i in _repo.QueryInvites()
                          join b in _repo.QueryBidPackages() on i.BidPackageId equals b.Id
                          join s in _repo.QueryBidPackageScopes() on b.Id equals s.BidPackageId
                          where b.ProjectId == projectId
                          && i.CompanyId == invitedCompanyId
                          select new { s.ScopeId, Description = s.Scope.CsiNumber + " " + s.Scope.Description }).Distinct();
                          

            return output.ToDictionary(x => x.ScopeId, y => y.Description);
        }


        public Dictionary<int, IEnumerable<int>> GetInvitationScopesByInvitingCompany(int projectId, int invitedCompanyId)
        {
            Dictionary<int, IEnumerable<int>> output = (from b in _repo.QueryBidPackages()
                          from s in b.Scopes
                          join i in _repo.QueryInvites() on b.Id equals i.BidPackageId
                          where b.ProjectId == projectId
                          && i.CompanyId == invitedCompanyId
                          group b by s.ScopeId into bsi
                          select new
                          {
                              ScopeId = bsi.Key,
                              InvitingCompanies = bsi.Select( x => x.CreatedById).AsEnumerable()
                          }).AsEnumerable()
                          .ToDictionary(x => x.ScopeId, y => y.InvitingCompanies);
            return output;
        }


        public Dictionary<int, string> GetInvitatingCompanies(int projectId, int invitedCompanyId)
        {
            var output = (from i in _repo.QueryInvites()
                          join b in _repo.QueryBidPackages() on i.BidPackageId equals b.Id
                          where b.ProjectId == projectId
                          && i.CompanyId == invitedCompanyId
                          select new { b.CreatedById, b.CreatedBy.CompanyName }).ToDictionary(x => x.CreatedById, y => y.CompanyName);

            return output;
        }


        public BidPackage GetMasterBidPackage(int projectId)
        {
            return (from b in _repo.QueryBidPackages()
                    where b.ProjectId == projectId
                    && b.IsMaster == true
                    select b).FirstOrDefault();
        }
    }
}