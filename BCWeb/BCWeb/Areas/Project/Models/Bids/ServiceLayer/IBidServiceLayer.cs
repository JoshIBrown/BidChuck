﻿using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Project.Models.Bids.ServiceLayer
{
    public interface IBidServiceLayer 
    {
        UserProfile GetUserProfile(int id);
        CompanyProfile GetCompanyProfile(int id);
        BCModel.Projects.Project GetProject(int id);
        BCModel.Projects.BidPackage GetBidPackage(int id);
        BCModel.Projects.Invitation GetInvite(int projectId, int companyId);
        IEnumerable<Invitation> GetInvites(int projectId, int companyId);
        IEnumerable<Scope> GetBidPackageScopes(int bidPackageId);
    }
}
