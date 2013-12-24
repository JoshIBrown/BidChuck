using BCModel;
using BCWeb.Areas.Account.Models.Scopes.ServiceLayer;
using BCWeb.Areas.Account.Models.Scopes.ViewModel;
using BCWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Account.Controllers
{
    [Authorize, HandleError]
    public class ScopesController : Controller
    {
        private IScopeServiceLayer _service;
        private IWebSecurityWrapper _security;

        public ScopesController(IScopeServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }
        //
        // GET: /Account/Scopes/

        // used by user to manage their ability scope
        public ActionResult Index()
        {
            // display chosen scopes
            return View();
        }

        public ActionResult ManageCompany()
        {
            ManageCompanyScopesModel viewModel = new ManageCompanyScopesModel();
            viewModel.CompanyId = _service.GetUser(_security.GetUserId(User.Identity.Name)).CompanyId;
            viewModel.SelectedScope = _service.GetEnumerableForCompany(viewModel.CompanyId).Select(s => s.Id).ToArray();

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ManageCompany(ManageCompanyScopesModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // get list of existing selections
                List<Scope> existing = _service.GetEnumerableForCompany(viewModel.CompanyId).ToList();

                Scope theScope;
                // add selections not in existing collection
                var toAdd = viewModel.SelectedScope.Where(x => !existing.Select(s => s.Id).Contains(x));
                foreach (var a in toAdd)
                {
                    // get the scope from the back end
                    theScope = _service.Get(a); 
                    // add company associatoin to the scope
                    theScope.Companies.Add(new CompanyXScope { CompanyId = viewModel.CompanyId, Scope = theScope });
                    // save changes
                    if (!_service.Update(theScope))
                    {
                        throw new HttpException(500, _service.ValidationDic.First().Value);
                    }
                }

                // remove scopes not in selected that are in existing
                var toRemove = existing.Where(y => !viewModel.SelectedScope.Contains(y.Id)).Select(s => s.Id);
                foreach (var r in toRemove)
                {
                    // find the scope in the list of existing selections
                    theScope = existing.Where(x => x.Id == r).First();
                    // find the company in the scopes companies
                    var xCompany = theScope.Companies.Where(x => x.CompanyId == viewModel.CompanyId).FirstOrDefault();
                    // if company is found.  should always be found
                    if (xCompany != null)
                    {
                        // remove the company from the scope
                        theScope.Companies.Remove(xCompany);
                        // save the changes
                        if (!_service.Update(theScope))
                        {
                            throw new HttpException(500, _service.ValidationDic.First().Value);
                        }
                    }
                }
                return RedirectToRoute("Default", new { controller = "Account", action = "Manage" });
            }
            return View(viewModel);
        }
    }
}
