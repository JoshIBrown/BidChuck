using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Projects.Models.Invitation.Repository
{
    public interface IInvitationRepository : IGenericRepository<BidPackageXInvitee>
    {

        UserProfile GetUerProfile(int id);
        CompanyProfile GetCompanyProfile(int id);

        BCModel.Projects.BidPackage GetBidPackage(int id);
        Project GetProject(int id);
    }
}