﻿using BCWeb.Models;
using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCModel;
using Microsoft.Ajax.Utilities;
using System.Linq.Expressions;

namespace BCWeb.Areas.Projects.Models.BidPackage.Repository
{
    public interface IBidPackageRepository : IGenericRepository<BCModel.Projects.BidPackage>
    {
        Project GetProject(int id);
        CompanyProfile GetCompany(int id);
        BidPackageXInvitee GetInvite(int id);
        IQueryable<Scope> QueryScopes();
        IQueryable<BidPackageXScope> QuerySelectedScopes();
        IQueryable<BidPackageXInvitee> QueryInvites();
        IQueryable<CompanyProfile> QueryCompanies();
        UserProfile GetUser(int id);
    }
}
