﻿using BCModel;
using BCWeb.Helpers;
using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Models.Company.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BCWeb.Models;
using BCWeb.Models.Company;

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
            int currentCompanyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            if (!id.HasValue)
            {
                return RedirectToAction("Profile", new { id = currentCompanyId });
            }

            CompanyProfile company = _service.Get(id);

            if (company == null)
                throw new HttpException(404, "company not found");

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



        public ActionResult Contacts(int? id)
        {
            int currentCompanyId = _service.GetUserProfile(_security.GetUserId(User.Identity.Name)).CompanyId;

            if (!id.HasValue)
            {
                return RedirectToAction("Contacts", new { id = currentCompanyId });
            }

            CompanyProfile company = _service.Get(id);

            if (company == null)
                throw new HttpException(404, "company not found");

            CompanyContactsViewModel viewModel = new CompanyContactsViewModel { CompanyId = id.Value };

            return View(viewModel);
        }

        [OutputCache(NoStore = true, VaryByParam = "*", Duration = 0)]
        public PartialViewResult Connected()
        {
            return PartialView("_Connected");
        }

        [OutputCache(NoStore = true, VaryByParam = "*", Duration = 0)]
        public PartialViewResult NotConnected()
        {
            return PartialView("_NotConnected");
        }

        [OutputCache(NoStore = true, VaryByParam = "*", Duration = 0)]
        public PartialViewResult RequestSent()
        {
            return PartialView("_RequestSent");
        }

        [OutputCache(NoStore = true, VaryByParam = "*", Duration = 0)]
        public PartialViewResult PendingRequest()
        {
            return PartialView("_PendingRequest");
        }

        [OutputCache(NoStore = true, VaryByParam = "*", Duration = 0)]
        public PartialViewResult Self()
        {
            return PartialView("_Self");
        }

        [OutputCache(NoStore = true, VaryByParam = "*", Duration = 0)]
        public PartialViewResult BlackListed()
        {
            return PartialView("_BlackListed");
        }
    }
}
