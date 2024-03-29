﻿using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Project.Models.Bids.Repository
{
    public interface IBidRepository
    {


        void AddOrUpdateBaseBid(BaseBid bid);
        void DeleteBaseBid(BaseBid bid);
        BaseBid GetBaseBid(params object[] key);
        IQueryable<BaseBid> QueryBaseBid();

        void AddOrUpdateComputedBid(ComputedBid bid);
        void DeleteComputedBid(ComputedBid bid);
        ComputedBid GetComputedBid(params object[] key);
        IQueryable<ComputedBid> QueryComputedBid();

        BCModel.Projects.Project GetProject(int id);
        BCModel.Projects.BidPackage GetBidPackage(int id);

        Invitation GetInvite(params object[] key);
        IQueryable<Invitation> QueryInvites();

        IQueryable<BCModel.Projects.BidPackage> QueryBidPackages();

        IQueryable<BidPackageXScope> QueryBidPackageScopes();

        UserProfile GetUserProfile(int id);
        CompanyProfile GetCompanyProfile(int id);

        void Save();

        void UpdateInvitation(Invitation invite);
    }
}
