﻿using BCWeb.Models;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using WebMatrix.Data;
using BCModel;
using BCWeb.Models.Account.ServiceLayer;
using BCWeb.Models.Account.ViewModel;
using BCWeb.Helpers;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using System.Data.Spatial;

namespace BCWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private IAccountServiceLayer _serviceLayer;
        private IWebSecurityWrapper _security;
        private IEmailSender _emailer;

        public AccountController(IAccountServiceLayer serviceLayer, IWebSecurityWrapper security, IEmailSender emailer)
        {
            _serviceLayer = serviceLayer;
            _security = security;
            _emailer = emailer;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Administer()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmationFailure()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmationSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            string passwordResetToken = String.Empty;

            if (ModelState.IsValid)
            {
                UserProfile user = _serviceLayer.GetUserProfiles(u => u.Email.ToLower() == model.Email.Trim().ToLower()).FirstOrDefault();

                // if user exists and has confirmed their membership
                if (user != null && _security.IsConfirmed(model.Email.Trim()))
                {
                    try
                    {
                        passwordResetToken = _security.GeneratePasswordResetToken(model.Email);

                        _emailer.SendPasswordResetMail(user.FirstName, user.Email, passwordResetToken);

                        return RedirectToAction("ForgotPasswordStepTwo", "Account");
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Either you have not confirmed your registration or this email address is unknown to us.");
                }


            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveSignInSuccess ? "The external SignIn was removed."
                : message == ManageMessageId.ChangeCompanyInfoSuccess ? "Company Info successfully updated."
                : message == ManageMessageId.ChangeEmailSuccess ? "Email successfully updated."
                : message == ManageMessageId.ChangeProfileSuccess ? "Profile Information successfully updated."
                : message == ManageMessageId.NewDelegateSuccess ? "New delegate successfully added."
                : message == ManageMessageId.PublishFail ? "Unable to publish company."
                : message == ManageMessageId.PublishSuccess ? "Company published."
                : message == ManageMessageId.UnpublishSuccess ? "Company unpublished."
                : message == ManageMessageId.UnpublishFail ? "Unable to unpublish company."
                : "";

            var raw = _serviceLayer.GetUserProfile(_security.GetUserId(User.Identity.Name));

            ManageDashboardViewModel viewModel = new ManageDashboardViewModel
            {
                Address1 = raw.Company.Address1,
                Address2 = raw.Company.Address2,
                BusinessType = Util.GetEnumDescription(raw.Company.BusinessType),
                City = raw.Company.City,
                CompanyName = raw.Company.CompanyName,
                CompanyId = raw.CompanyId,
                Email = raw.Email,
                OperatingRadius = raw.Company.OperatingDistance.ToString(),
                Phone = raw.Company.Phone,
                PostalCode = raw.Company.PostalCode,
                State = raw.Company.StateId.HasValue ? raw.Company.State.Abbr : "NOT SET",
                Name = raw.FirstName + " " + raw.LastName,
                Published = raw.Company.Published
            };

            viewModel.Scopes = raw.Scopes
                .Where(x => !x.Scope.ParentId.HasValue)
                .OrderBy(x => x.Scope.CsiNumber)
                .Select(x => x.Scope.CsiNumber.Substring(0, 2) + " " + x.Scope.Description);

            var delegates = _security.GetUsersInRole("Employee");

            viewModel.Minions = raw.Company.Users.Where(x => delegates.Contains(x.Email))
                .OrderBy(x => x.LastName)
                .Select(x => new MinionOverviewViewModel
                {
                    Name = x.LastName + ", " + x.FirstName,
                    Email = x.Email,
                    Confirmed = _security.IsConfirmed(x.Email) ? "Active" : "Invited"
                });
            return View(viewModel);
        }



        [AllowAnonymous]
        public ActionResult ResetPassword(string User, string Token)
        {
            ResetPasswordModel rpm = new ResetPasswordModel();
            rpm.Email = User;
            rpm.PasswordResetToken = Token;

            return View(rpm);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // Look up the user

                UserProfile user = _serviceLayer.GetUserProfile(_security.GetUserId(model.Email));

                if (user != null)
                {
                    // ResetPassword may throw an exception rather than return false in certain failure scenarios.
                    bool resetPasswordSucceeded;
                    try
                    {
                        resetPasswordSucceeded = _security.ResetPassword(model.PasswordResetToken, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        resetPasswordSucceeded = false;
                    }

                    if (resetPasswordSucceeded)
                    {
                        return View("ResetPasswordSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error has occured. Please try again or contact the administrator.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Unknown email address.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (_security.IsAuthenticated)
            {
                return RedirectToRoute("Default", new { controller = "Accout", action = "Manage" });
            }

            RegisterModel viewModel = new RegisterModel();
            viewModel.States = _serviceLayer.GetStates().Select(x => new SelectListItem { Text = x.Abbr, Value = x.Id.ToString() });
            //viewModel.BusinessTypes = _serviceLayer.GetBusinessTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            return View("Register", viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
            model.States = _serviceLayer.GetStates().Select(x => new SelectListItem { Text = x.Abbr, Value = x.Id.ToString() });
            //model.BusinessTypes = _serviceLayer.GetBusinessTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            if (String.IsNullOrEmpty(recaptchaHelper.Response))
            {
                ModelState.AddModelError("", "Captcha answer cannot be empty.");
                return View(model);
            }

            RecaptchaVerificationResult recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();

            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
                ModelState.AddModelError("", "Incorrect captcha answer.");
            }

            int cpId;
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {

                    



                    CompanyProfile cp = new CompanyProfile
                    {
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        BusinessType = model.BusinessType,
                        City = model.City,
                        CompanyName = model.CompanyName,
                        OperatingDistance = model.OperatingDistance,
                        Phone = Util.ConvertPhoneForStorage(model.Phone),
                        PostalCode = model.PostalCode,
                        Published = true,
                        StateId = model.StateId
                    };

                    GeoLocator locator = new GeoLocator();

                    string state = _serviceLayer.GetStates().Where(x => x.Id == model.StateId).FirstOrDefault().Abbr;

                    if (model.Address1 == null || model.Address1 == string.Empty)
                    {
                        cp.GeoLocation = locator.GetFromCityStateZip(model.City, state, model.PostalCode);
                    }
                    else
                    {
                        cp.GeoLocation = locator.GetFromAddress(model.Address1, model.City, state, model.PostalCode);
                    }

                    // if we can create the company, create the user
                    if (_serviceLayer.CreateCompany(cp))
                    {
                        cpId = cp.Id;
                        string confirmationToken = _security.CreateUserAndAccount(
                            model.Email,
                            model.Password,
                            new
                            {
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                CompanyId = cp.Id
                            }, true);

                        _security.AddUserToRole(model.Email, "Manager");


                        //var businesstypes = _serviceLayer.GetBusinessTypes();

                        switch (model.BusinessType)
                        {
                            case BusinessType.GeneralContractor:
                                _security.AddUserToRole(model.Email, "general_contractor");
                                break;
                            case BusinessType.SubContractor:
                                _security.AddUserToRole(model.Email, "subcontractor");
                                break;
                            case BusinessType.Architect:
                                _security.AddUserToRole(model.Email, "architect");
                                break;
                            case BusinessType.Engineer:
                                _security.AddUserToRole(model.Email, "engineer");
                                break;
                            case BusinessType.Owner:
                                _security.AddUserToRole(model.Email, "owner_client");
                                break;
                            case BusinessType.MaterialsVendor:
                                _security.AddUserToRole(model.Email, "materials_vendor");
                                break;
                            case BusinessType.MaterialsMfg:
                                _security.AddUserToRole(model.Email, "materials_manufacturer");
                                break;
                            case BusinessType.Consultant:
                                _security.AddUserToRole(model.Email, "consultant");
                                break;
                        };




                        _emailer.SendConfirmationMail(model.FirstName, model.Email, confirmationToken);

                        return RedirectToAction("RegisterStepTwo", "Account");
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResendConfirmationEmail()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResendConfirmationEmail(ResendConfirmationEmailModel model)
        {
            string confirmationToken = String.Empty;

            if (ModelState.IsValid)
            {
                // Look up the user's confirmation token
                //using (UsersContext uc = new UsersContext())
                //{
                UserProfile user = _serviceLayer.GetUserProfiles(u => u.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    try
                    {
                        using (Database db = Database.Open("DefaultConnection"))
                        {
                            var sql = "SELECT ConfirmationToken FROM webpages_Membership WHERE UserId =" + user.UserId;
                            confirmationToken = db.Query(sql).First()["ConfirmationToken"];
                        }

                        _emailer.SendConfirmationMail(user.FirstName, user.Email, confirmationToken);

                        return RedirectToAction("RegisterStepTwo", "Account");
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Unknown email address.");
                }
                //}
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult RegisterConfirmation(string ID)
        {
            
            if (_security.ConfirmAccount(ID))
            {
                return RedirectToAction("ConfirmationSuccess");
            }
            return RedirectToAction("ConfirmationFailure");
        }

        [AllowAnonymous]
        public ActionResult RegisterStepTwo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordStepTwo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            if (_security.IsAuthenticated)
            {
                return RedirectToRoute(new { route = "Default", controller = "Home", action = "Index" });
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInModel model, string returnUrl)
        {
            // if valid model state, user is confirmed, and sign in successful
            if (ModelState.IsValid && _security.IsConfirmed(model.Email) && _security.Login(model.Email, model.Password, persistCookie: model.RememberMe))
            {
                if (User.IsInRole("Administrator"))
                {
                    return RedirectToRoute("Default", new { controller = "Admin", action = "Index" });
                }
                if (returnUrl == null || returnUrl == "")
                    return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
                else
                    return RedirectToLocal(returnUrl);
            }

            // TODO: add model error ofr account not confirmed
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            _security.Logout();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {

            // not used
            //ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(_security.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            return View("ChangePassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            //bool hasLocalAccount = OAuth_security.HasLocalAccount(_security.GetUserId(User.Identity.Name));
            //ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            //if (hasLocalAccount)
            //{
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = _security.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }
            //}
            //else
            //{
            //    // User does not have a local password so remove any validation errors caused by a missing
            //    // OldPassword field
            //    ModelState state = ModelState["OldPassword"];
            //    if (state != null)
            //    {
            //        state.Errors.Clear();
            //    }

            //    if (ModelState.IsValid)
            //    {
            //        try
            //        {
            //            _security.CreateAccount(User.Identity.Name, model.NewPassword);
            //            return RedirectToAction("ChangePassword", new { Message = ManageMessageId.SetPasswordSuccess });
            //        }
            //        catch (Exception e)
            //        {
            //            ModelState.AddModelError("", e);
            //        }
            //    }
            //}

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        [HttpGet]
        public ActionResult ChangeEmail()
        {
            // get user profile
            var raw = _serviceLayer.GetUserProfile(_security.GetUserId(User.Identity.Name));
            // create view model
            EditEmailViewModel viewModel = new EditEmailViewModel();
            viewModel.UserId = raw.UserId;
            viewModel.CurrentEmail = raw.Email;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(EditEmailViewModel viewModel)
        {
            // model state checks out
            if (ModelState.IsValid)
            {
                // username and password check out
                if (Membership.ValidateUser(viewModel.CurrentEmail, viewModel.Password))
                {

                    var toUpdate = _serviceLayer.GetUserProfile(viewModel.UserId);
                    toUpdate.Email = viewModel.NewEmail;

                    // if update successfull
                    if (_serviceLayer.UpdateUserProfile(toUpdate))
                    {
                        // logout and back in unobtrusively to refresh context
                        _security.Logout();
                        _security.Login(viewModel.NewEmail, viewModel.Password);
                        return RedirectToAction("Manage", new { message = ManageMessageId.ChangeEmailSuccess });
                    }
                    else
                    {
                        Util.MapValidationErrors(_serviceLayer.ValidationDic, this.ModelState);
                        return View(viewModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("Password", "Incorrect Password");
                    return View(viewModel);
                }
            }
            else
            {
                return View(viewModel);
            }

        }


        [HttpGet]
        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfileViewModel viewModel)
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult AcceptInvitation(string user, string token)
        {
            // make sure no one is logged in
            if (!User.Identity.IsAuthenticated)
            {
                // confirm account to unlock it
                if (_security.ConfirmAccount(user, token))
                {
                    // force user to reset their passowrd
                    string passwordToken = _security.GeneratePasswordResetToken(user);
                    InvitationViewModel viewModel = new InvitationViewModel
                    {
                        PasswordResetToken = passwordToken,
                        Email = user
                    };
                    return View(viewModel);
                }
                else
                {
                    // add view to show unable to accept invite
                    return RedirectToAction("ConfirmationFailure");
                }
            }
            else
            {
                // change this to "you must sign out first"
                return RedirectToAction("ConfirmationFailure");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptInvitation(InvitationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // if user exists
                if (_security.UserExists(viewModel.Email))
                {
                    // ResetPassword may throw an exception rather than return false in certain failure scenarios.
                    bool resetPasswordSucceeded;
                    try
                    {
                        resetPasswordSucceeded = _security.ResetPassword(viewModel.PasswordResetToken, viewModel.NewPassword);
                    }
                    catch (Exception)
                    {
                        resetPasswordSucceeded = false;
                    }

                    if (resetPasswordSucceeded)
                    {
                        return View("AcceptInvitationSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error has occured. Please try again or contact the administrator.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Unknown email address.");
                }
            }
            return View(viewModel);

        }
        #region Helpers

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Email address already registered.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }

        #endregion Helpers
    }
}