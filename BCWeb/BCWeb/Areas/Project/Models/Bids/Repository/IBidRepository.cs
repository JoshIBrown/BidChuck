using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Project.Models.Bids.Repository
{
    public interface IBidRepository : IGenericRepository<Bid>
    {
        BCModel.Projects.Project GetProject(int id);
        BCModel.Projects.BidPackage GetBidPackage(int id);
        Invitation GetInvite(int id);
        IQueryable<Invitation> QueryInvites();
        IQueryable<BCModel.Projects.BidPackage> QueryBidPackages();
        UserProfile GetUserProfile(int id);
        CompanyProfile GetCompanyProfile(int id);
    }
}
