using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Invitations.Repository
{
    public interface IInvitationRepository : IGenericRepository<Invitation>
    {

        UserProfile GetUerProfile(int id);
        CompanyProfile GetCompanyProfile(int id);

        BCModel.Projects.BidPackage GetBidPackage(int id);
        BCModel.Projects.Project GetProject(int id);
    }
}