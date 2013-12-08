using BCModel;
using BCWeb.Areas.Account.Models.Users.ServiceLayer;
using BCWeb.Areas.Admin.Models.Users;
using BCWeb.Helpers;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private IWebSecurityWrapper _security;
        private IUserProfileServiceLayer _service;
        private IEmailSender _email;
        public UserController(IUserProfileServiceLayer service, IWebSecurityWrapper security, IEmailSender email)
        {
            _service = service;
            _security = security;
            _email = email;
        }

        //
        // GET: /Admin/User/
        public ActionResult Index()
        {
            return View();
        }


        // GET: /Admin/User/Create/
        [HttpGet]
        public ActionResult Create()
        {
            UserProfileEditModel viewModel = new UserProfileEditModel();
            viewModel.Companies = _service.GetEnumerableCompanies().Select(s => new SelectListItem { Text = s.CompanyName, Value = s.Id.ToString() });
            return View(viewModel);
        }

        // POST: /Admin/User/
        [HttpPost, ValidateAntiForgeryToken, HandleError()]
        public ActionResult Create(UserProfileEditModel user)
        {
            // check for existing user
            if (_security.UserExists(user.Email))
            {
                ModelState.AddModelError("Email", "User already exists");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    // random password
                    Guid random = new Guid();

                    // create user
                    _security.CreateUserAndAccount(user.Email, random.ToString(),
                        new
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            CompanyId = user.CompanyId
                        }, false);

                    // add roles
                    // if manager else employee

                    CompanyProfile company = _service.GetCompany(user.CompanyId);

                    // add appropriate business role
                    switch (company.BusinessType)
                    {
                        case BusinessType.GeneralContractor:
                            _security.AddUserToRole(user.Email, "general_contractor");
                            break;
                        case BusinessType.SubContractor:
                            _security.AddUserToRole(user.Email, "subcontractor");
                            break;
                        case BusinessType.Architect:
                            _security.AddUserToRole(user.Email, "architect");
                            break;
                        case BusinessType.Engineer:
                            _security.AddUserToRole(user.Email, "engineer");
                            break;
                        case BusinessType.Owner:
                            _security.AddUserToRole(user.Email, "owner_client");
                            break;
                        case BusinessType.MaterialsVendor:
                            _security.AddUserToRole(user.Email, "materials_vendor");
                            break;
                        case BusinessType.MaterialsMfg:
                            _security.AddUserToRole(user.Email, "materials_manufacturer");
                            break;
                        case BusinessType.Consultant:
                            _security.AddUserToRole(user.Email, "consultant");
                            break;
                    };

                    if (user.IsManager)
                        _security.AddUserToRole(user.Email, "Manager");
                    else
                        _security.AddUserToRole(user.Email, "Employee");

                    // generate password reset token
                    string token = _security.GeneratePasswordResetToken(user.Email);

                    // send email with token
                    _email.SendPasswordResetMail(user.FirstName, user.Email, token);

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            user.Companies = _service.GetEnumerableCompanies().Select(s => new SelectListItem { Selected = s.Id == user.CompanyId, Text = s.CompanyName, Value = s.Id.ToString() });
            return View(user);
        }

        // GET: /Admin/User/Edit/4
        [HttpGet]
        [HandleError(ExceptionType = typeof(KeyNotFoundException))]
        public ActionResult Edit(int id)
        {

                UserProfile theUser = _service.Get(id);
                if (theUser != null)
                {
                    UserProfileEditModel viewModel = new UserProfileEditModel
                        {
                            CompanyId = theUser.CompanyId,
                            Email = theUser.Email,
                            FirstName = theUser.FirstName,
                            LastName = theUser.LastName,
                            IsManager = _security.IsUserInRole(theUser.Email, "Manager"),
                            JobTitle = theUser.JobTitle,
                            UserId = theUser.UserId
                        };
                    viewModel.Companies = _service.GetEnumerableCompanies().Select(s => new SelectListItem { Text = s.CompanyName, Value = s.Id.ToString() , Selected = s.Id == theUser.CompanyId});
                    return View(viewModel);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
        }

        [HttpPost, ValidateAntiForgeryToken(), HandleError]
        public ActionResult Edit(UserProfileEditModel viewModel)
        {
            UserProfile theUser = _service.Get(viewModel.UserId);

            // if email address has changed, and the newly chosen address already exists
            if (theUser.Email != viewModel.Email && _security.UserExists(viewModel.Email))
            {
                ModelState.AddModelError("Email", "user with that email already exists");
            }

            if (ModelState.IsValid)
            {
                 // if user was not a manager previously, but is now
                if (viewModel.IsManager && !_security.IsUserInRole(theUser.Email, "Manager"))
                {
                    _security.AddUserToRole(theUser.Email, "Manager");
                    _security.RemoveUserFromRole(theUser.Email, "Employee");
                }

                // if user was a manager previously, but is no longer
                if (!viewModel.IsManager && _security.IsUserInRole(theUser.Email, "Manager"))
                {
                    _security.AddUserToRole(theUser.Email, "Employee");
                    _security.RemoveUserFromRole(theUser.Email, "Manager");
                }

                // make changes
                theUser.FirstName = viewModel.FirstName;
                theUser.LastName = viewModel.LastName;
                theUser.Email = viewModel.Email;
                theUser.CompanyId = viewModel.CompanyId;
                theUser.JobTitle = viewModel.JobTitle;

                // apply changes
                if (_service.Update(theUser))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Util.MapValidationErrors(_service.ValidationDic, this.ModelState);
                }
            }
            viewModel.Companies = _service.GetEnumerableCompanies().Select(s => new SelectListItem { Text = s.CompanyName, Value = s.Id.ToString(), Selected = s.Id == viewModel.CompanyId });
            return View(viewModel);
        }
    }
}
