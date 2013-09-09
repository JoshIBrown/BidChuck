using BCWeb.Areas.Account.Models.Company.ViewModel;
using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Helpers;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Account.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        
        private IWebSecurityWrapper _security;
        private ICompanyProfileServiceLayer _serviceLayer;

        public CompanyController(ICompanyProfileServiceLayer serviceLayer, IWebSecurityWrapper security)
        {
            _security = security;
            _serviceLayer = serviceLayer;
        }

        [Authorize(Roles = "Manager,Administrator")]
        [HttpGet]
        public ActionResult Edit()
        {

            // get users company
            var raw = _serviceLayer.GetUserProfiles(u=> u.UserId == _security.GetUserId(User.Identity.Name)).FirstOrDefault().Company;

            // transpose into viewmodel
            EditCompanyViewModel viewModel = new EditCompanyViewModel
            {
                Address1 = raw.Address1,
                Address2 = raw.Address2,
                BusinessTypeId = raw.BusinessTypeId,
                City = raw.City,
                CompanyName = raw.CompanyName,
                Id = raw.Id,
                OperatingDistance = raw.OperatingDistance,
                Phone = raw.Phone == "" ? "" : Util.ConvertPhoneForDisplay(raw.Phone),
                PostalCode = raw.PostalCode,
                StateId = raw.StateId
            };

            // fill states and business types
            viewModel.States = _serviceLayer.GetStates().Select(x => new SelectListItem { Text = x.Abbr, Value = x.Id.ToString(), Selected = x.Id == viewModel.StateId });
            viewModel.BusinessTypes = _serviceLayer.GetBusinessTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == viewModel.BusinessTypeId });

            return View(viewModel);
        }

        [Authorize(Roles = "Manager,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // get company
                var company = _serviceLayer.GetUserProfiles(u=> u.UserId == _security.GetUserId(User.Identity.Name)).FirstOrDefault().Company;

                if (viewModel.Address1 != null && company.Address1 != viewModel.Address1.Trim())
                    company.Address1 = viewModel.Address1.Trim();

                if (viewModel.Address2 != null && company.Address2 != viewModel.Address2.Trim())
                    company.Address2 = viewModel.Address2.Trim();


                if (viewModel.BusinessTypeId != null && company.BusinessTypeId != viewModel.BusinessTypeId)
                {

                    var businesstypes = _serviceLayer.GetBusinessTypes();

                    // add new role for all users in company
                    string newTypeName = businesstypes.FirstOrDefault(x => x.Id == viewModel.BusinessTypeId).Name;
                    switch (newTypeName)
                    {
                        case "General Contractor":
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "general_contractor");
                            break;
                        case "Sub-Contractor":
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "subcontractor");
                            break;
                        case "Architect":
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "architect");
                            break;
                        case "Engineer":
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "engineer");
                            break;
                        case "@Owner/Client":
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "owner_client");
                            break;
                        case "Materials Vendor":
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "materials_vendor");
                            break;
                        case "Materials Manufacturer":
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "materials_manufacturer");
                            break;
                        case "Consultant":
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "consultant");
                            break;
                    };


                    // remove old role for all users in company
                    string existingTypeName = businesstypes.FirstOrDefault(x => x.Id == company.BusinessTypeId).Name;
                    switch (existingTypeName)
                    {
                        case "General Contractor":
                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "general_contractor");
                            break;
                        case "Sub-Contractor":

                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "subcontractor");
                            break;
                        case "Architect":

                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "architect");
                            break;
                        case "Engineer":

                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "engineer");
                            break;
                        case "@Owner/Client":

                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "owner_client");
                            break;
                        case "Materials Vendor":

                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "materials_vendor");
                            break;
                        case "Materials Manufacturer":

                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "materials_manufacturer");
                            break;
                        case "Consultant":

                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "consultant");
                            break;
                    };

                    company.BusinessTypeId = viewModel.BusinessTypeId;
                }

                if (viewModel.City != null && company.City != viewModel.City)
                    company.City = viewModel.City;

                if (viewModel.CompanyName != null && company.CompanyName != viewModel.CompanyName.Trim())
                    company.CompanyName = viewModel.CompanyName.Trim();

                if (viewModel.OperatingDistance == 0 && company.OperatingDistance != viewModel.OperatingDistance)
                    company.OperatingDistance = viewModel.OperatingDistance;

                if (viewModel.Phone != null && company.Phone != viewModel.Phone.Trim())
                    company.Phone = Util.ConvertPhoneForStorage(viewModel.Phone.Trim());

                if (viewModel.PostalCode != null && company.PostalCode != viewModel.PostalCode.Trim())
                    company.PostalCode = viewModel.PostalCode.Trim();

                if (viewModel.StateId == 0 && company.StateId != viewModel.StateId)
                    company.StateId = viewModel.StateId;

                // update changes in the database
                if (_serviceLayer.Update(company))
                {
                    return RedirectToRoute("Default", new { controller="Account", action="Manage", message = ManageMessageId.ChangeCompanyInfoSuccess });
                }
                else
                {
                    Util.MapValidationErrors(_serviceLayer.ValidationDic, this.ModelState);
                    viewModel.States = _serviceLayer.GetStates().Select(x => new SelectListItem { Text = x.Abbr, Value = x.Id.ToString(), Selected = x.Id == viewModel.StateId });
                    viewModel.BusinessTypes = _serviceLayer.GetBusinessTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == viewModel.BusinessTypeId });
                    return View(viewModel);
                }
            }
            else
            {
                viewModel.States = _serviceLayer.GetStates().Select(x => new SelectListItem { Text = x.Abbr, Value = x.Id.ToString(), Selected = x.Id == viewModel.StateId });
                viewModel.BusinessTypes = _serviceLayer.GetBusinessTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == viewModel.BusinessTypeId });
                return View(viewModel);
            }
        }
    }
}
