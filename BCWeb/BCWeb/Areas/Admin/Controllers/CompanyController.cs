using BCModel;
using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Areas.Admin.Models.Companies;
using BCWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CompanyController : Controller
    {
        private ICompanyProfileServiceLayer _service;

        public CompanyController(ICompanyProfileServiceLayer service)
        {
            _service = service;
        }

        //
        // GET: /Admin/Company/
        public ActionResult Index()
        {
            return View();
        }


        //
        // GET: /Admin/Company/Create
        public ActionResult Create()
        {
            CompanyProfileEditItem viewModel = new CompanyProfileEditItem();
            viewModel.States = _service.GetStates().Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString() });
            viewModel.BusinessTypes = Util.CreateSelectListFromEnum(typeof(BusinessType));

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, HandleError]
        public ActionResult Create(CompanyProfileEditItem viewModel)
        {
            if (ModelState.IsValid)
            {
                CompanyProfile toAdd = new CompanyProfile
                {
                    Address1 = viewModel.Address1,
                    Address2 = viewModel.Address2,
                    BusinessType = viewModel.BusinessType.Value,
                    City = viewModel.City,
                    CompanyName = viewModel.CompanyName,
                    OperatingDistance = viewModel.OperatingDistance,
                    Phone = viewModel.Phone,
                    PostalCode = viewModel.PostalCode,
                    Published = viewModel.Published,
                    StateId = viewModel.StateId,
                    Website = viewModel.Website
                };

                if (_service.Create(toAdd))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                    rePopViewModel(viewModel);
                    return View(viewModel);
                }
            }
            // modelstate is not valid
            rePopViewModel(viewModel);
            return View(viewModel);
        }

        private void rePopViewModel(CompanyProfileEditItem viewModel)
        {
            viewModel.States = _service.GetStates().Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString(), Selected = viewModel.StateId == s.Id });
            viewModel.BusinessTypes = Util.CreateSelectListFromEnum(typeof(BusinessType), viewModel.BusinessType.Value.ToString());
        }
    }
}
