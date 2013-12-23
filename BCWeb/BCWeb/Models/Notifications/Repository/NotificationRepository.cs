using BCModel;
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

        public NotificationRepository()
        {
            _notes = _context.Notifications;
            _templates = _context.NotificationTemplates;
            _users = _context.UserProfiles;
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
    }
}