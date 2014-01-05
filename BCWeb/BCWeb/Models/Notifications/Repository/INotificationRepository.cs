using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models.Notifications.Repository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        IQueryable<UserProfile> QueryUserProfiles();

        BCModel.Projects.Project GetProject(int id);
    }
}
