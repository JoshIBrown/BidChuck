using BCModel;
using BCWeb.Areas.Account.Models.Scopes.ServiceLayer;
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
    [Authorize(Roles = "Manager,Administrator")]
    public class ScopesController : ApiController
    {

        private IScopeServiceLayer _service;

        public ScopesController(IScopeServiceLayer service)
        {
            _service = service;
        }


        public IEnumerable<ScopeViewModel> GetList()
        {
            IEnumerable<ScopeViewModel> viewModel;

            viewModel = _service.GetEnumerable()
                .OrderBy(s => s.Id)
                .Select(s => new ScopeViewModel
                {
                    CsiNumber = s.CsiNumber,
                    Description = s.Description,
                    Id = s.Id,
                    ParentId = s.ParentId
                }).ToArray();

            return viewModel;
        }



        public IEnumerable<ScopeMgmtViewModel> GetScopesToManage()
        {
            int uId = WebSecurity.GetUserId(User.Identity.Name);


            IEnumerable<ScopeMgmtViewModel> viewModel;

            var user = _service.GetUser(uId);
            var chosenScopes = user.Scopes.ToList();

            viewModel = _service.GetEnumerable()
                .OrderBy(s => s.Id)
                .Select(s => new ScopeMgmtViewModel
                {
                    Checked = chosenScopes.Contains(s),
                    Description = s.Description,
                    Id = s.Id,
                    ParentId = s.ParentId,
                    CsiNumber = s.CsiNumber
                }).ToArray();


            return viewModel;
        }

        public IEnumerable<ScopeMgmtViewModel> GetScopesToManage(string user)
        {


            int uId = WebSecurity.GetUserId(user);
            var profile = _service.GetUser(uId);
            if (_service.GetUser(WebSecurity.GetUserId(User.Identity.Name)).Delegates.Contains(profile))
            {

                IEnumerable<ScopeMgmtViewModel> viewModel;


                var chosenScopes = profile.Scopes.ToList();
                var manager = profile.Manager;


                viewModel = manager.Scopes
                    .OrderBy(s => s.Id)
                    .Select(s => new ScopeMgmtViewModel
                    {
                        Checked = chosenScopes.Contains(s),
                        Description = s.Description,
                        Id = s.Id,
                        ParentId = s.ParentId,
                        CsiNumber = s.CsiNumber
                    }).ToArray();


                return viewModel;
            }
            else
            {
                return null;
            }
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

                    // get scope objects matching the id's of the selected
                    var selectedScopes = _service.GetEnumerable(x => selected.Contains(x.Id)).ToList();

                    // get scope objects user had chosen previously
                    var user = _service.GetUser(userId);
                    var existingSelection = user.Scopes.ToList();

                    // add selections not in existing collection
                    var toAdd = selectedScopes.Where(x => !existingSelection.Contains(x));
                    foreach (var a in toAdd)
                    {
                        //user.Scopes.Add(a);
                        a.Users.Add(user);
                        if (!_service.Update(a))
                        {
                            throw new Exception(_service.ValidationDic.First().Value);
                        }
                    }

                    // remove scopes not in selected that are in existing
                    var toRemove = existingSelection.Where(y => !selectedScopes.Contains(y));
                    foreach (var r in toRemove)
                    {
                        r.Users.Remove(user);
                        _service.Update(r);
                        if (!_service.Update(r))
                        {
                            throw new Exception(_service.ValidationDic.First().Value);
                        }
                        //user.Scopes.Remove(r);
                    }

                    // save changes and report our glorious save

                    result.message = "changes saved";
                    result.success = true;
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
