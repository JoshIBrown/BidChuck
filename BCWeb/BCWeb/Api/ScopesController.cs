using BCModel;
using BCWeb.Areas.Account.Models.Scopes.ServiceLayer;
using BCWeb.Areas.Account.Models.Scopes.ViewModel;
using BCWeb.Models;
using BCWeb.Models.GenericViewModel;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Attributes;
using WebMatrix.WebData;

namespace BCWeb.Controllers.Api
{
    [Authorize(Roles = "Manager,Administrator")]
    public class ScopesController : ApiController
    {

        private IScopeServiceLayer _service;
        private IWebSecurityWrapper _security;

        public ScopesController(IScopeServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
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


        // /api/Scopes/GetScopesToManage?user=soandso@ladeeda.com
        public IEnumerable<ScopeMgmtViewModel> GetScopesToManage([FromUri]string type, [FromUri]string ident)
        {
            return getScopesToManage(type, ident);
        }

        // /api/Scopes/GetScopesToManage?user=soandso@ladeeda.com
        public IEnumerable<ScopeMgmtViewModel> GetScopesToManage([FromUri]string type)
        {
            return getScopesToManage(type, "");
        }

        private IEnumerable<ScopeMgmtViewModel> getScopesToManage(string type, string ident)
        {
            IEnumerable<ScopeMgmtViewModel> viewModel;
            IEnumerable<Scope> selectedScopes;
            IEnumerable<Scope> availableScopes;
            UserProfile theUser;
            CompanyProfile theCompany;


            // the company dictates the scopes available for management
            // if editing the company, display all scopes
            // figure out request type
            switch (type)
            {
                case "self":
                    theUser = _service.GetUser(_security.GetUserId(User.Identity.Name));
                    theCompany = _service.GetCompany(theUser.CompanyId);
                    selectedScopes = theUser.Scopes.Select(s => s.Scope).ToList();
                    availableScopes = theCompany.Scopes.Select(s => s.Scope).ToList();
                    break;
                case "company":
                    theUser = _service.GetUser(_security.GetUserId(User.Identity.Name));
                    theCompany = _service.GetCompany(theUser.CompanyId);
                    selectedScopes = theCompany.Scopes.Select(s => s.Scope).ToList();
                    availableScopes = _service.GetEnumerable();
                    break;
                case "user":
                    var userid = _security.GetUserId(ident);
                    theUser = _service.GetUser(userid);
                    theCompany = _service.GetCompany(theUser.CompanyId);
                    // make sure user is part of logged in users company
                    if (_service.GetUser(_security.GetUserId(User.Identity.Name)).Company != theCompany)
                        throw new Exception("you are not allowed to edit users in other companies");
                    selectedScopes = theUser.Scopes.Select(s => s.Scope).ToList();
                    availableScopes = theCompany.Scopes.Select(s => s.Scope).ToList();
                    break;
                default:
                    throw new ArgumentException("invalid arguments");
            };



            viewModel = availableScopes.Select(s => new ScopeMgmtViewModel
            {
                Checked = selectedScopes.Contains(s),
                Description = s.Description,
                Id = s.Id,
                ParentId = s.ParentId,
                CsiNumber = s.CsiNumber
            }).ToArray();

            return viewModel;
        }


        // FIXME
        [ValidateHttpAntiForgeryTokenAttribute]
        public JQueryPostResult PutSelectedScopes([FromBody] SelectedScopesViewModel viewModel)
        {
            JQueryPostResult result = new JQueryPostResult();

            try
            {
                // get scope objects matching the id's of the selected
                var selectedScopes = _service.GetEnumerable(x => viewModel.Selected.Contains(x.Id)).ToList();

                // get scope objects user had chosen previously
                UserProfile user;
                CompanyProfile company;
                switch (viewModel.Type)
                {
                    case "user":
                        user = _service.GetUser(_security.GetUserId(viewModel.Ident));
                        company = _service.GetCompany(user.CompanyId);
                        break;
                    case "company":
                        user = _service.GetUser(_security.GetUserId(User.Identity.Name));
                        company = _service.GetCompany(user.CompanyId);
                        break;
                    case "self":
                        user = _service.GetUser(_security.GetUserId(User.Identity.Name));
                        company = _service.GetCompany(user.CompanyId);
                        break;
                    default:
                        result.message = "invalid operation";
                        result.success = false;
                        return result;
                }

                if (viewModel.Type == "user" || viewModel.Type == "self")
                {
                    var existing = user.Scopes.ToList();

                    // add selections not in existing collection
                    var toAdd = selectedScopes.Where(x => !existing.Select(s => s.Scope).Contains(x));
                    foreach (var a in toAdd)
                    {
                        a.Users.Add(new UserXScope { User = user, Scope = a });
                        if (!_service.Update(a))
                        {
                            throw new Exception(_service.ValidationDic.First().Value);
                        }
                    }

                    // remove scopes not in selected that are in existing
                    var toRemove = existing.Where(y => !selectedScopes.Contains(y.Scope)).Select(s => s.Scope);
                    foreach (var r in toRemove)
                    {
                        r.Users.Remove(existing.Find(x => x.Scope == r && x.User == user));
                        if (!_service.Update(r))
                        {
                            throw new Exception(_service.ValidationDic.First().Value);
                        }
                    }
                }
                else if (viewModel.Type == "company")
                {
                    var existing = company.Scopes.ToList();

                    // add selections not in existing collection
                    var toAdd = selectedScopes.Where(x => !existing.Select(s => s.Scope).Contains(x));
                    foreach (var a in toAdd)
                    {
                        a.Companies.Add(new CompanyXScope { Company = company, Scope = a });
                        if (!_service.Update(a))
                        {
                            throw new Exception(_service.ValidationDic.First().Value);
                        }
                    }

                    // remove scopes not in selected that are in existing
                    var toRemove = existing.Where(y => !selectedScopes.Contains(y.Scope)).Select(s => s.Scope);
                    foreach (var r in toRemove)
                    {
                        r.Companies.Remove(existing.Find(x => x.Scope == r && x.Company == company));
                        if (!_service.Update(r))
                        {
                            throw new Exception(_service.ValidationDic.First().Value);
                        }
                    }
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

            return result;

        }
    }
}
