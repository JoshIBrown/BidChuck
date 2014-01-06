using BCModel;
using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Notifications.Repository
{
    public class NotificationRepository : RepositoryBase, INotificationRepository
    {
        private DbSet<Notification> _notes;
        private DbSet<NotificationTemplate> _templates;
        private DbSet<UserProfile> _users;
        private DbSet<BCModel.Projects.Project> _projects;
        private DbSet<Invitation> _invites;
        private DbSet<BidPackage> _bidPackages;

        public NotificationRepository()
        {
            _notes = _context.Notifications;
            _templates = _context.NotificationTemplates;
            _users = _context.UserProfiles;
            _projects = _context.Projects;
            _invites = _context.Invitations;
            _bidPackages = _context.BidPackages;
        }

        public void Create(BCModel.Notification entity)
        {
            _notes.Add(entity);
        }

        public void Update(BCModel.Notification entity)
        {
            var current = _notes.Find(entity.Id);
            _context.Entry<Notification>(current).CurrentValues.SetValues(entity);
        }

        public void Delete(params object[] key)
        {
            Delete(_notes.Find(key));
        }

        public void Delete(BCModel.Notification entity)
        {
            _notes.Remove(entity);
        }

        public BCModel.Notification Get(params object[] key)
        {
            return _notes.Find(key);
        }

        public IQueryable<BCModel.Notification> Query()
        {
            return _notes.Include(n => n.Recipient);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IQueryable<UserProfile> QueryUserProfiles()
        {
            return _users.Include(u => u.Company);
        }


        public BCModel.Projects.Project GetProject(int id)
        {
            return _projects.Find(id);
        }


        public IQueryable<BCModel.Projects.Invitation> QueryInvites()
        {
            return _invites;
        }


        public IQueryable<BidPackage> QueryBidPackages()
        {
            return _bidPackages;
        }
    }
}