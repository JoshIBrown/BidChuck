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

            if (_service.GetConnectionStatus(currentCompanyId, company.Id) == ConnectionStatus.BlackListed)
                throw new HttpException(404, "No company found with that id");

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
            // exclude black listed companies from returned data
            return View();
        }

        public PartialViewResult Connected()
        {
            return PartialView("_Connected");
        }

        public PartialViewResult NotConnected()
        {
            return PartialView("_NotConnected");
        }

        public PartialViewResult RequestSent()
        {
            return PartialView("_RequestSent");
        }

        public PartialViewResult PendingRequest()
        {
            return PartialView("_PendingRequest");
        }

        public PartialViewResult Self()
        {
            return PartialView("_Self");
        }
    }
}
