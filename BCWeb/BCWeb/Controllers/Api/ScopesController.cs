using BCModel;
using BCWeb.Areas.Account.Models.Scopes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Controllers.Api
{
    public class ScopesController : ApiController
    {
        public IEnumerable<ScopeHierarchyViewModel> GetHierarchyList()
        {
            IEnumerable<ScopeHierarchyViewModel> viewModel;
            using (BidChuckContext context = new BidChuckContext())
            {
                viewModel = context.Scopes
                    .Where(s => s.ParentId == null)
                    .OrderBy(s => s.Id)
                    .Select(s => new ScopeHierarchyViewModel
                    {
                        Description = s.Description,
                        Id = s.Id,
                        Parent = s.ParentId
                    }).ToArray();

                foreach (var i in viewModel)
                {
                    i.Children = context.Scopes
                        .Where(s => s.ParentId.HasValue && s.ParentId.Value == i.Id)
                        .OrderBy(s => s.Id)
                        .Select(s => new ScopeHierarchyViewModel
                        {
                            Description = s.Description,
                            Id = s.Id,
                            Parent = s.ParentId
                        }).ToArray();

                    foreach (var j in i.Children)
                    {
                        j.Children = context.Scopes
                        .Where(s => s.ParentId.HasValue && s.ParentId.Value == j.Id)
                        .OrderBy(s => s.Id)
                        .Select(s => new ScopeHierarchyViewModel
                        {
                            Description = s.Description,
                            Id = s.Id,
                            Parent = s.ParentId
                        }).ToArray();
                    }
                }
            }

            return viewModel;
        }

        public IEnumerable<ScopeViewModel> GetList()
        {
            IEnumerable<ScopeViewModel> viewModel;
            using (BidChuckContext context = new BidChuckContext())
            {
                viewModel = context.Scopes.OrderBy(s => s.Id)
                    .Select(s => new ScopeViewModel { Description = s.Description, Id = s.Id, ParentId = s.ParentId }).ToArray();
            }

            return viewModel;
        }
    }
}
