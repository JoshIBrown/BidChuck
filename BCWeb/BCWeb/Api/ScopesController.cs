using BCModel;
using BCWeb.Areas.Account.Models.Scopes.ViewModel;
using BCWeb.Models.GenericViewModel;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMatrix.WebData;

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



        public IEnumerable<ScopeMgmtViewModel> GetScopesToManage()
        {
            int uId = WebSecurity.GetUserId(User.Identity.Name);


            IEnumerable<ScopeMgmtViewModel> viewModel;
            using (BidChuckContext context = new BidChuckContext())
            {
                var user = context.UserProfiles.Find(uId);
                var chosenScopes = user.Scopes.ToList();

                viewModel = context.Scopes.OrderBy(s => s.Id)
                    .AsEnumerable()
                    .Select(s => new ScopeMgmtViewModel
                    {
                        Checked = chosenScopes.Contains(s),
                        Description = s.Description,
                        Id = s.Id,
                        ParentId = s.ParentId
                    }).ToArray();
            }

            return viewModel;
        }

        public JQueryPostResult PutSelectedScopes([FromBody]IEnumerable<int> selected)
        {
            JQueryPostResult result = new JQueryPostResult();
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));

            // if user logged in?
            if (hasLocalAccount)
            {
                try
                {
                    var userId = WebSecurity.GetUserId(User.Identity.Name);
                    using (BidChuckContext context = new BidChuckContext())
                    {
                        // get scope objects matching the id's of the selected
                        var selectedScopes = context.Scopes.Where(x => selected.Contains(x.Id)).ToList();

                        // get scope objects user had chosen previously
                        var user = context.UserProfiles.Find(userId);
                        var existingSelection = user.Scopes.ToList();

                        // add selections not in existing collection
                        var toAdd = selectedScopes.Where(x => !existingSelection.Contains(x));
                        foreach (var a in toAdd)
                        {
                            user.Scopes.Add(a);
                        }

                        // remove scopes not in selected that are in existing
                        var toRemove = existingSelection.Where(y => !selectedScopes.Contains(y));
                        foreach (var r in toRemove)
                        {
                            user.Scopes.Remove(r);
                        }

                        // save changes and report our glorious save
                        context.SaveChanges();
                        result.message = "changes saved";
                        result.success = true;
                    }
                }
                catch (Exception ex)
                {
                    // report the exception to the user for now.  quick'n'dirty
                    result.message = ex.Message;
                    result.success = false;
                }
            }
            else
            {
                result.message = "user not logged in";
                result.success = false;
            }
            return result;
        }
    }
}
