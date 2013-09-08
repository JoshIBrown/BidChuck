﻿using BCModel;
using BCWeb.Areas.Account.Models.Users.ViewModel;
using BCWeb.Helpers;
using BCWeb.Models;
using BCWeb.Models.Account.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.Data;
using WebMatrix.WebData;

namespace BCWeb.Areas.Account.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private IUserProfileServiceLayer _service;
        private IWebSecurityWrapper _security;
        private IEmailSender _emailer;

        public UsersController(IUserProfileServiceLayer service,IWebSecurityWrapper security, IEmailSender emailer)
        {
            _service = service;
            _security = security;
            _emailer = emailer;
        }

        //
        // GET: /Account/Users/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Manager,Administrator")]
        public ActionResult Add()
        {
            return View();
        }



        // FIXME
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Administrator")]
        public ActionResult Add(NewUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int currentUserId = _security.GetUserId(User.Identity.Name);
                UserProfile user = _service.GetProfile(currentUserId);

                // use security context to create user
                string confirmToken = _security.CreateUserAndAccount(viewModel.Email,
                    "password",
                    new
                    {
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName
                        //StateId = user.StateId,
                        ////CountyId = user.CountyId, // pulled for now
                        //CompanyName = user.CompanyName,
                        //Phone = Util.ConvertPhoneForStorage(user.Phone),
                        //Address1 = user.Address1,
                        //Address2 = user.Address2,
                        //City = user.City,
                        //PostalCode = user.PostalCode,
                        //OperatingDistance = user.OperatingDistance,
                        //BusinessTypeId = user.BusinessTypeId,
                        //Published = false
                    }, true);

                // give them a minion role
                _security.AddUserToRole(viewModel.Email, "Employee");

                // add user to minions
                int newUserId = _security.GetUserId(viewModel.Email);



                //user.Delegates.Add(_service.GetProfile(newUserId));
                //_service.UpdateProfile(user);

                // email minion a "password reset" email
                //string passwordToken = _security.GeneratePasswordResetToken(viewModel.Email); // change password to something impossibru
                //_security.ChangePassword(viewModel.Email, "password", passwordToken);
                _emailer.SendNewDelegateEmail(user.FirstName + " " + user.LastName, viewModel.FirstName, viewModel.Email, confirmToken);
                return RedirectToRoute("Default", new { controller = "Account", action = "Manage" });
            }
            else
            {
                return View(viewModel);
            }

        }

        [HttpPost]
        [Authorize(Roles = "Manager,Administrator")]
        [ValidateAntiForgeryToken]
        public void ResendInvitation(string email)
        {
            var user = _service.GetProfile(_security.GetUserId(email));
            string confirmationToken = "";
            using (Database db = Database.Open("DefaultConnection"))
            {
                var sql = "SELECT ConfirmationToken FROM webpages_Membership WHERE UserId =" + user.UserId;
                confirmationToken = db.Query(sql).First()["ConfirmationToken"];
            }
            _emailer.SendNewDelegateEmail(user.FirstName + " " + user.LastName, user.FirstName, user.Email, confirmationToken);
        }

    }
}
