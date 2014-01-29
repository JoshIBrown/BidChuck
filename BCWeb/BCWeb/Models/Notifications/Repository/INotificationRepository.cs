using BCModel;
using BCModel.Projects;
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

        IQueryable<Invitation> QueryInvites();

        IQueryable<BidPackage> QueryBidPackages();

        BidPackage FindBidPackage(int bidPackageId);

        CompanyProfile GetCompanyProfile(int id);
    }
}
