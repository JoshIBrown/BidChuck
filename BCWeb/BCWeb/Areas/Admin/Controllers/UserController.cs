using BCModel;
using BCWeb.Areas.Account.Models.Users.ServiceLayer;
using BCWeb.Areas.Admin.Models.Users;
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

        public ActionResult Create()
        {
            UserProfileEditModel viewModel = new UserProfileEditModel();
            viewModel.Companies = _service.GetEnumerableCompanies().Select(s => new SelectListItem { Text = s.CompanyName, Value = s.Id.ToString() });
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HandleError()]
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


    }
}
