using BCModel;
using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Areas.Admin.Models.Companies;
using BCWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
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
            CompanyProfileEditModel viewModel = new CompanyProfileEditModel();
            viewModel.States = _service.GetStates().Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString() });
            viewModel.BusinessTypes = Util.CreateSelectListFromEnum(typeof(BusinessType));

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, HandleError]
        public ActionResult Create(CompanyProfileEditModel viewModel)
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
                    Phone = Util.ConvertPhoneForStorage(viewModel.Phone),
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
                }
            }

            rePopViewModel(viewModel);
            return View(viewModel);
        }


        // GET: /Admin/Company/Edit/123
        public ActionResult Edit(int id)
        {
            CompanyProfile company = _service.Get(id);

            CompanyProfileEditModel viewModel = new CompanyProfileEditModel
            {
                Address1 = company.Address1,
                Address2 = company.Address2,
                BusinessType = company.BusinessType,
                City = company.City,
                CompanyName = company.CompanyName,
                Id = company.Id,
                OperatingDistance = company.OperatingDistance,
                Phone = Util.ConvertPhoneForDisplay(company.Phone),
                PostalCode = company.PostalCode,
                Published = company.Published,
                StateId = company.StateId.HasValue ? company.StateId.Value : 0,
                Website = company.Website
            };

            rePopViewModel(viewModel);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, HandleError]
        public ActionResult Edit(CompanyProfileEditModel viewModel)
        {
            if (ModelState.IsValid)
            {
                CompanyProfile toUpdate = _service.Get(viewModel.Id);

                toUpdate.Address1 = viewModel.Address1;
                toUpdate.Address2 = viewModel.Address2;
                toUpdate.BusinessType = viewModel.BusinessType.Value;
                toUpdate.City = viewModel.City;
                toUpdate.CompanyName = viewModel.CompanyName;
                toUpdate.OperatingDistance = viewModel.OperatingDistance;
                toUpdate.Phone = Util.ConvertPhoneForStorage(viewModel.Phone);
                toUpdate.PostalCode = viewModel.PostalCode;
                toUpdate.Published = viewModel.Published;
                toUpdate.StateId = viewModel.StateId;
                toUpdate.Website = viewModel.Website;
                if (_service.Update(toUpdate))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                }
            }
            rePopViewModel(viewModel);
            return View(viewModel);
        }


        public ActionResult Details(int id)
        {
            CompanyProfile company = _service.Get(id);
            CompanyProfileDetailsModel viewModel = new CompanyProfileDetailsModel
            {
                Address1 = company.Address1,
                Address2 = company.Address2,
                BusinessType = company.BusinessType.ToDescription(),
                City = company.City,
                CompanyName = company.CompanyName,
                Id = company.Id,
                OperatingDistance = company.OperatingDistance.ToString() + " miles",
                Phone = Util.ConvertPhoneForDisplay(company.Phone),
                PostalCode = company.PostalCode,
                Published = company.Published ? "Yes" : "No",
                State = company.StateId.HasValue ? company.State.Abbr : "N/A",
                Website = company.Website,
                Users = company.Users.ToDictionary(x => x.UserId, x => x.Email)
            };

            return View(viewModel);
        }

        public ActionResult UpdateEmptyLatLong()
        {
            // get list of companies with missing lat long
            var companies = _service.GetEmptyLatLongList().Select(s => new CompanyProfileListItem { Id = s.Id, CompanyName = s.CompanyName }).ToList();
            // show to user
            return View(companies);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateEmptyLatLong(int[] companyId)
        {
            GeoLocator locator = new GeoLocator();
            CompanyProfile company;

            for (int i = 0; i < companyId.Length; i++)
            {
                company = _service.Get(companyId[i]);
                if (company.Id != 1)
                {
                    if (company.Address1 == null && company.City == null && company.StateId != null && company.PostalCode != null)
                    {
                       company.GeoLocation = locator.GetFromStateZip(company.State.Abbr, company.PostalCode);
                    }
                    else if ((company.Address1 == null || company.Address1 == string.Empty) && company.StateId != null && company.PostalCode != null)
                    {
                        company.GeoLocation = locator.GetFromCityStateZip(company.City, company.State.Abbr, company.PostalCode);
                    }
                    else if ((company.Address1 != null && company.Address1 != string.Empty) && (company.City != null && company.City != string.Empty) && company.StateId != null && company.PostalCode != null)
                    {
                        company.GeoLocation = locator.GetFromAddress(company.Address1, company.City, company.State.Abbr, company.PostalCode);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        private void rePopViewModel(CompanyProfileEditModel viewModel)
        {
            viewModel.States = _service.GetStates().Select(s => new SelectListItem { Text = s.Abbr, Value = s.Id.ToString(), Selected = viewModel.StateId == s.Id });
            viewModel.BusinessTypes = Util.CreateSelectListFromEnum(typeof(BusinessType), viewModel.BusinessType.Value.ToString());
        }
    }
}