using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Project.Models.Invitation.ServiceLayer
{
    public interface IInvitationServiceLayer : IGenericServiceLayer<BidPackageXInvitee>
    {

        UserProfile GetUerProfile(int id);
        CompanyProfile GetCompanyProfile(int id);

        BCModel.Projects.BidPackage GetBidPackage(int id);
        BCModel.Projects.Project GetProject(int id);
    }
}
