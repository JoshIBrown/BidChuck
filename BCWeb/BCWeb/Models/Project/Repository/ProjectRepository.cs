﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Project.Repository
{
    public class ProjectRepository : RepositoryBase, IProjectRepository
    {
        private DbSet<BCModel.Projects.Project> _projects;

        public ProjectRepository()
        {
            _projects = _context.Projects;
        }
        public IQueryable<BCModel.Projects.ConstructionType> QueryConstructionType()
        {
            return _context.ConstructionTypes;
        }

        public IQueryable<BCModel.Projects.BuildingType> QueryBuildingType()
        {
            return _context.BuildingTypes;
        }

        public IQueryable<BCModel.Projects.ProjectType> QueryProjectType()
        {
            return _context.ProjectTypes;
        }

        public void Create(BCModel.Projects.Project entity)
        {
            _projects.Add(entity);
        }

        public void Update(BCModel.Projects.Project entity)
        {
            var current = _projects.Find(entity.Id);
            _context.Entry<BCModel.Projects.Project>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(int id)
        {
            Delete(_projects.Find(id));
        }

        public void Delete(BCModel.Projects.Project entity)
        {
            _projects.Remove(entity);
        }

        public BCModel.Projects.Project Get(int id)
        {
            return _projects.Find(id);
        }

        public IQueryable<BCModel.Projects.Project> Query()
        {
            return _projects;
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public IQueryable<BCModel.State> QueryStates()
        {
            return _context.States;
        }
    }
}