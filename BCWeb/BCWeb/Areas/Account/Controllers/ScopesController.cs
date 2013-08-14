using BCModel;
using BCWeb.Areas.Account.Models.Scopes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Account.Controllers
{
    [Authorize]
    public class ScopesController : Controller
    {
        //
        // GET: /Account/Scopes/

        // used by user to manage their ability scope

        public ActionResult Index()
        {
            // display chosen scopes
            return View();
        }


        public ActionResult Manage()
        {
            return View();
        }


        [HttpGet]
        [OutputCache(Duration=0,NoStore=true,VaryByParam="*")]
        public string GetScopes()
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

            return JsonConvert.SerializeObject(viewModel);
        }
    }
}
