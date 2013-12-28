using BCWeb.Areas.Account.Models.Company.ViewModel;
using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Helpers;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BCModel;
using System.Data.Spatial;

namespace BCWeb.Areas.Account.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {

        private IWebSecurityWrapper _security;
        private ICompanyProfileServiceLayer _serviceLayer;
        private IEmailSender _email;

        public CompanyController(ICompanyProfileServiceLayer serviceLayer, IWebSecurityWrapper security, IEmailSender email)
        {
            _security = security;
            _serviceLayer = serviceLayer;
            _email = email;
        }

        [Authorize(Roles = "Manager,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Publish(int id)
        {
            CompanyProfile company = _serviceLayer.Get(id);
            UserProfile user = _serviceLayer.GetUserProfile(_security.GetUserId(User.Identity.Name));
            if (company.Users.Contains(user))
            {
                // publish the company
                company.Published = true;
                if (_serviceLayer.Update(company))
                {
                    return RedirectToRoute("Default", new { controller = "Account", action = "Manage", message = ManageMessageId.PublishSuccess });
                }
                else
                {
                    return RedirectToRoute("Default", new { controller = "Account", action = "Manage", message = ManageMessageId.PublishFail });
                }
            }
            else
            {
                // you are not allowed to do this
                throw new Exception("unauthorized operation");
            }
        }

        [Authorize(Roles = "Manager,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unpublish(int id)
        {
            CompanyProfile company = _serviceLayer.Get(id);
            UserProfile user = _serviceLayer.GetUserProfile(_security.GetUserId(User.Identity.Name));
            if (company.Users.Contains(user))
            {
                // publish the company
                company.Published = false;
                if (_serviceLayer.Update(company))
                {
                    return RedirectToRoute("Default", new { controller = "Account", action = "Manage", message = ManageMessageId.UnpublishSuccess });
                }
                else
                {
                    return RedirectToRoute("Default", new { controller = "Account", action = "Manage", message = ManageMessageId.UnpublishFail });
                }
            }
            else
            {
                // you are not allowed to do this
                throw new Exception("unauthorized operation");
            }
        }

        [Authorize(Roles = "Manager,Administrator")]
        [HttpGet]
        public ActionResult Edit()
        {

            // get users company
            int userId = _security.GetUserId(User.Identity.Name);
            var raw = _serviceLayer.GetUserProfiles(u => u.UserId == userId).FirstOrDefault().Company;

            // transpose into viewmodel
            EditCompanyViewModel viewModel = new EditCompanyViewModel
            {
                Address1 = raw.Address1,
                Address2 = raw.Address2,
                BusinessType = raw.BusinessType,
                City = raw.City,
                CompanyName = raw.CompanyName,
                Id = raw.Id,
                OperatingDistance = raw.OperatingDistance,
                Phone = raw.Phone == null || raw.Phone == "" ? "" : Util.ConvertPhoneForDisplay(raw.Phone),
                PostalCode = raw.PostalCode,
                StateId = raw.StateId
            };

            // fill states and business types
            viewModel.States = _serviceLayer.GetStates().Select(x => new SelectListItem { Text = x.Abbr, Value = x.Id.ToString(), Selected = x.Id == viewModel.StateId });
            viewModel.BusinessTypes = Util.CreateSelectListFromEnum(typeof(BusinessType), raw.BusinessType.ToString());

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
                int userId = _security.GetUserId(User.Identity.Name);
                var company = _serviceLayer.GetUserProfiles(u => u.UserId == userId).FirstOrDefault().Company;

                if (viewModel.Address1 != null)
                    company.Address1 = viewModel.Address1.Trim();
                if (viewModel.Address2 != null)
                    company.Address2 = viewModel.Address2.Trim();

                company.City = viewModel.City;
                company.CompanyName = viewModel.CompanyName.Trim();
                company.OperatingDistance = viewModel.OperatingDistance;
                company.Phone = Util.ConvertPhoneForStorage(viewModel.Phone.Trim());
                company.PostalCode = viewModel.PostalCode.Trim();
                company.StateId = viewModel.StateId;

                GeoLocator locator = new GeoLocator();

                if (company.Address1 == null && company.City == null && company.StateId != null && company.PostalCode != null)
                {

                    locator.GetFromStateZip(company.State.Abbr, company.PostalCode, (abc) =>
                    {
                        if (abc.statusCode == 200
                            && abc.resourceSets != null
                            && abc.resourceSets.Count == 1
                            && abc.resourceSets[0].estimatedTotal == 1)
                        {
                            var lat = abc.resourceSets[0].resources[0].point.coordinates[0];
                            var lng = abc.resourceSets[0].resources[0].point.coordinates[1];
                            company.GeoLocation = DbGeography.FromText(string.Format("POINT({1} {0})", lat, lng));

                        }
                    });
                }
                else if ((company.Address1 == null || company.Address1 == string.Empty) && company.StateId != null && company.PostalCode != null)
                {
                    locator.GetFromCityStateZip(company.City, company.State.Abbr, company.PostalCode, (abc) =>
                    {
                        if (abc.statusCode == 200
                            && abc.resourceSets != null
                            && abc.resourceSets.Count == 1
                            && abc.resourceSets[0].estimatedTotal == 1)
                        {
                            var lat = abc.resourceSets[0].resources[0].point.coordinates[0];
                            var lng = abc.resourceSets[0].resources[0].point.coordinates[1];
                            company.GeoLocation = DbGeography.FromText(string.Format("POINT({1} {0})", lat, lng));

                        }
                    });
                }
                else if ((company.Address1 != null && company.Address1 != string.Empty) && (company.City != null && company.City != string.Empty) && company.StateId != null && company.PostalCode != null)
                {
                    locator.GetFromAddress(company.Address1, company.City, company.State.Abbr, company.PostalCode, (abc) =>
                    {
                        if (abc.statusCode == 200
                            && abc.resourceSets != null
                            && abc.resourceSets.Count == 1
                            && abc.resourceSets[0].estimatedTotal == 1)
                        {
                            var lat = abc.resourceSets[0].resources[0].point.coordinates[0];
                            var lng = abc.resourceSets[0].resources[0].point.coordinates[1];
                            company.GeoLocation = DbGeography.FromText(string.Format("POINT({1} {0})", lat, lng));

                        }
                    });
                }

                // did business type change?
                if (company.BusinessType != viewModel.BusinessType)
                {


                    // add new role for all users in company
                    switch (viewModel.BusinessType.Value)
                    {
                        case BusinessType.GeneralContractor:
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "general_contractor");
                            break;
                        case BusinessType.SubContractor:
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "subcontractor");
                            break;
                        case BusinessType.Architect:
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "architect");
                            break;
                        case BusinessType.Engineer:
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "engineer");
                            break;
                        case BusinessType.Owner:
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "owner_client");
                            break;
                        case BusinessType.MaterialsVendor:
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "materials_vendor");
                            break;
                        case BusinessType.MaterialsMfg:
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "materials_manufacturer");
                            break;
                        case BusinessType.Consultant:
                            _security.AddUsersToRole(company.Users.Select(x => x.Email).ToArray(), "consultant");
                            break;
                    };


                    // remove old role for all users in company
                    switch (company.BusinessType)
                    {
                        case BusinessType.GeneralContractor:
                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "general_contractor");
                            break;
                        case BusinessType.SubContractor:
                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "subcontractor");
                            break;
                        case BusinessType.Architect:
                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "architect");
                            break;
                        case BusinessType.Engineer:
                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "engineer");
                            break;
                        case BusinessType.Owner:
                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "owner_client");
                            break;
                        case BusinessType.MaterialsVendor:
                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "materials_vendor");
                            break;
                        case BusinessType.MaterialsMfg:
                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "materials_manufacturer");
                            break;
                        case BusinessType.Consultant:
                            _security.RemoveUsersFromRole(company.Users.Select(x => x.Email).ToArray(), "consultant");
                            break;
                    };

                    // update company business type
                    company.BusinessType = viewModel.BusinessType.Value;
                }



                // update changes in the database
                if (_serviceLayer.Update(company))
                {
                    return RedirectToRoute("Default", new { controller = "Account", action = "Manage", message = ManageMessageId.ChangeCompanyInfoSuccess });
                }
                else
                {
                    Util.MapValidationErrors(_serviceLayer.ValidationDic, this.ModelState);
                    viewModel.States = _serviceLayer.GetStates().Select(x => new SelectListItem { Text = x.Abbr, Value = x.Id.ToString(), Selected = x.Id == viewModel.StateId });
                    //viewModel.BusinessTypes = _serviceLayer.GetBusinessTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == viewModel.BusinessTypeId });
                    return View(viewModel);
                }
            }
            else
            {
                viewModel.States = _serviceLayer.GetStates().Select(x => new SelectListItem { Text = x.Abbr, Value = x.Id.ToString(), Selected = x.Id == viewModel.StateId });
                //viewModel.BusinessTypes = _serviceLayer.GetBusinessTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == viewModel.BusinessTypeId });
                return View(viewModel);
            }
        }

        [Authorize(Roles = "general_contractor,Adminstrator")]
        [HttpGet]
        public ActionResult CreateArchitect(string name, string title, string number)
        {
            NewArchitectViewModel viewModel = new NewArchitectViewModel
            {
                CompanyName = name,
                ProjectNumber = number,
                ProjectTitle = title
            };
            return View(viewModel);
        }

        [Authorize(Roles = "general_contractor,Adminstrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateArchitect(NewArchitectViewModel viewModel)
        {
            CompanyProfile toCreate = new CompanyProfile
                {
                    CompanyName = viewModel.CompanyName,
                    ContactEmail = viewModel.ContactEmail,
                    BusinessType = BusinessType.Architect
                };

            // create record if user doesn't exist
            if (!_security.UserExists(viewModel.ContactEmail))
            {
                if (_serviceLayer.Create(toCreate))
                {
                    Guid password = new Guid();

                    var token = _security.CreateUserAndAccount(viewModel.ContactEmail, password.ToString(),
                        new
                        {
                            FirstName = viewModel.ContactFirstName,
                            LastName = viewModel.ContactLastName,
                            CompanyId = toCreate.Id
                        }
                        , true);

                    _security.AddUserToRoles(viewModel.ContactEmail, new[] { "architect", "Manager" });

                    var user = _serviceLayer.GetUserProfile(_security.GetUserId(User.Identity.Name));

                    string invitingTagLine = string.Format("{0} {1} of {2}", user.FirstName, user.LastName, user.Company.CompanyName);

                    string name = string.Format("{0} {1}", viewModel.ContactFirstName, viewModel.ContactLastName);

                    //send email
                    _email.InviteArchitect(viewModel.ContactEmail, name, viewModel.CompanyName, invitingTagLine, token);
                    return RedirectToRoute("Default", new { controller = "Project", action = "CreateStepTwo", architectId = toCreate.Id, title = viewModel.ProjectTitle, number = viewModel.ProjectNumber });
                }
                else
                {
                    Util.MapValidationErrors(_serviceLayer.ValidationDic, this.ModelState);
                    return View(viewModel);
                }
            }
            else
            {
                this.ModelState.AddModelError("ContactEmail", "There is already someone registered using that email");
                return View(viewModel);
            }

        }
    }
}
