using BCModel;
using BCWeb.Helpers;
using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Models.Company.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BCWeb.Models;

namespace BCWeb.Controllers
{
    [Authorize, HandleError]
    public class CompanyController : Controller
    {

        private ICompanyProfileServiceLayer _service;
        private IWebSecurityWrapper _security;

        public CompanyController(ICompanyProfileServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }
        //
        // GET: /Company/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile(int? id)
        {

            if (!id.HasValue)
            {
                int companyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;
                return RedirectToAction("Profile", new { id = companyId });
            }

            CompanyProfile company = _service.Get(id);
            CompanyProfileViewModel viewModel = new CompanyProfileViewModel
            {
                Address1 = company.Address1,
                Address2 = company.Address2,
                BusinessType = company.BusinessType.ToDescription(),
                City = company.City,
                CompanyName = company.CompanyName,
                Id = company.Id,
                OperatingDistance = company.OperatingDistance.ToString() + " Miles",
                PostalCode = company.PostalCode,
                State = company.State == null ? "" : company.State.Abbr,
                WorkScopes = company.Scopes.OrderBy(s => s.Scope.CsiNumber).Select(s => s.Scope.CsiNumber + " " + s.Scope.Description)
            };



            return View(viewModel);
        }

        public ActionResult Connections(int id)
        {
            return View();
        }
    }
}
