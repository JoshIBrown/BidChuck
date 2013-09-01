using BCWeb.Models;
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

namespace BCWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private IUserProfileServiceLayer _serviceLayer;

        public AccountController(IUserProfileServiceLayer serviceLayer)
        {
            _serviceLayer = serviceLayer;
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
                // Look up the user's confirmation token
                //using (UsersContext uc = new UsersContext())
                //{
                //UserProfile user = uc.UserProfiles.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
                UserProfile user = _serviceLayer.GetProfiles(u => u.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();

                if (user != null)
                {
                    try
                    {
                        passwordResetToken = WebSecurity.GeneratePasswordResetToken(model.Email);

                        EmailSender.SendPasswordResetMail(user.FirstName, user.Email, passwordResetToken);

                        return RedirectToAction("ForgotPasswordStepTwo", "Account");
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
                : "";

            var raw = _serviceLayer.GetProfile(WebSecurity.GetUserId(User.Identity.Name));

            ManageDashboardViewModel viewModel = new ManageDashboardViewModel
            {
                Address1 = raw.Address1,
                Address2 = raw.Address2,
                BusinessType = raw.BusinessType.Name,
                City = raw.City,
                CompanyName = raw.CompanyName,
                Email = raw.Email,
                OperatingRadius = raw.OperatingDistance.ToString(),
                Phone = raw.Phone,
                PostalCode = raw.PostalCode,
                State = raw.State.Abbr,
                Name = raw.FirstName + " " + raw.LastName
            };

            viewModel.Scopes = raw.Scopes
                .Where(x => !x.ParentId.HasValue)
                .OrderBy(x => x.CsiNumber)
                .Select(x => x.CsiNumber.Substring(0, 2) + " " + x.Description);

            viewModel.Minions = raw.Delegates
                .OrderBy(x => x.LastName)
                .Select(x => new MinionOverviewViewModel
                {
                    Name = x.LastName + ", " + x.FirstName,
                    Email = x.Email,
                    Confirmed = WebSecurity.IsConfirmed(x.Email) ? "Active" : "Invited"
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

                UserProfile user = _serviceLayer.GetProfiles(u => u.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();

                if (user != null)
                {
                    // ResetPassword may throw an exception rather than return false in certain failure scenarios.
                    bool resetPasswordSucceeded;
                    try
                    {
                        resetPasswordSucceeded = WebSecurity.ResetPassword(model.PasswordResetToken, model.NewPassword);
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
            if (WebSecurity.IsAuthenticated)
            {
                return RedirectToRoute("Default", new { controller = "Accout", action = "Manage" });
            }

            RegisterModel viewModel = new RegisterModel();
            viewModel.States = _serviceLayer.GetStates().Select(x => new SelectListItem { Text = x.Abbr, Value = x.Id.ToString() });
            viewModel.BusinessTypes = _serviceLayer.GetBusinessTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    string confirmationToken = WebSecurity.CreateUserAndAccount(
                        model.Email,
                        model.Password,
                        new
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            StateId = model.StateId,
                            //CountyId = model.CountyId, // pulled for now
                            CompanyName = model.CompanyName,
                            Phone = Util.ConvertPhoneForStorage(model.Phone),
                            Address1 = model.Address1,
                            Address2 = model.Address2,
                            City = model.City,
                            PostalCode = model.PostalCode,
                            OperatingDistance = model.OperatingDistance,
                            BusinessTypeId = model.BusinessTypeId,
                            Published = false
                        }, true);

                    Roles.AddUserToRole(model.Email, "Manager");

                    // todo: add user to role for business type

                    EmailSender.SendConfirmationMail(model.FirstName, model.Email, confirmationToken);

                    return RedirectToAction("RegisterStepTwo", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
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
                UserProfile user = _serviceLayer.GetProfiles(u => u.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    try
                    {
                        using (Database db = Database.Open("DefaultConnection"))
                        {
                            var sql = "SELECT ConfirmationToken FROM webpages_Membership WHERE UserId =" + user.UserId;
                            confirmationToken = db.Query(sql).First()["ConfirmationToken"];
                        }

                        EmailSender.SendConfirmationMail(user.FirstName, user.Email, confirmationToken);

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
            if (WebSecurity.ConfirmAccount(ID))
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
            if (WebSecurity.IsAuthenticated)
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
            if (ModelState.IsValid && WebSecurity.Login(model.Email, model.Password, persistCookie: model.RememberMe))
            {
                if (returnUrl == null || returnUrl == "")
                    return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
                else
                    return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ChangePassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveSignInSuccess ? "The external SignIn was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            return View("ChangePassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("ChangePassword", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("ChangePassword", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Manager,Administrator")]
        [HttpGet]
        public ActionResult EditCompany()
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            if (hasLocalAccount)
            {
                var raw = _serviceLayer.GetProfile(WebSecurity.GetUserId(User.Identity.Name));
                EditCompanyViewModel viewModel = new EditCompanyViewModel
                {
                    Address1 = raw.Address1,
                    Address2 = raw.Address2,
                    BusinessTypeId = raw.BusinessTypeId.HasValue ? raw.BusinessTypeId.Value : 0,
                    City = raw.City,
                    CompanyName = raw.CompanyName,
                    Id = raw.UserId,
                    OperatingDistance = raw.OperatingDistance,
                    Phone = raw.Phone == "" ? "" : Util.ConvertPhoneForDisplay(raw.Phone),
                    PostalCode = raw.PostalCode,
                    StateId = raw.StateId.HasValue ? raw.StateId.Value : 0
                };
                viewModel.States = _serviceLayer.GetStates().Select(x => new SelectListItem { Text = x.Abbr, Value = x.Id.ToString(), Selected = x.Id == viewModel.StateId });
                viewModel.BusinessTypes = _serviceLayer.GetBusinessTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == viewModel.BusinessTypeId });

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("SignIn", new { returnUrl = Url.Action("SignIn").ToString() });
            }

        }

        [Authorize(Roles = "Manager,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCompany(EditCompanyViewModel viewModel)
        {


            if (ModelState.IsValid)
            {
                var profile = _serviceLayer.GetProfile(WebSecurity.GetUserId(User.Identity.Name));

                if (viewModel.Address1 != null && profile.Address1 != viewModel.Address1.Trim())
                    profile.Address1 = viewModel.Address1.Trim();

                if (viewModel.Address2 != null && profile.Address2 != viewModel.Address2.Trim())
                    profile.Address2 = viewModel.Address2.Trim();

                if (viewModel.BusinessTypeId != null && profile.BusinessTypeId != viewModel.BusinessTypeId)
                {

                    var businesstypes = _serviceLayer.GetBusinessTypes();

                    // add new role
                    string newTypeName = businesstypes.FirstOrDefault(x => x.Id == viewModel.BusinessTypeId).Name;
                    switch (newTypeName)
                    {
                        case "General Contractor":
                            Roles.AddUserToRole(User.Identity.Name, "general_contractor");
                            break;
                        case "Sub-Contractor":
                            Roles.AddUserToRole(User.Identity.Name, "subcontractor");
                            break;
                        case "Architect":
                            Roles.AddUserToRole(User.Identity.Name, "architect");
                            break;
                        case "Engineer":
                            Roles.AddUserToRole(User.Identity.Name, "engineer");
                            break;
                        case "@Owner/Client":
                            Roles.AddUserToRole(User.Identity.Name, "owner_client");
                            break;
                        case "Materials Vendor":
                            Roles.AddUserToRole(User.Identity.Name, "materials_vendor");
                            break;
                        case "Materials Manufacturer":
                            Roles.AddUserToRole(User.Identity.Name, "materials_manufacturer");
                            break;
                        case "Consultant":
                            Roles.AddUserToRole(User.Identity.Name, "consultant");
                            break;
                    };

                    // remove old role
                    if (profile.BusinessTypeId.HasValue)
                    {
                        string existingTypeName = businesstypes.FirstOrDefault(x => x.Id == profile.BusinessTypeId).Name;
                        switch (existingTypeName)
                        {
                            case "General Contractor":
                                if (Roles.IsUserInRole("general_contractor"))
                                    Roles.RemoveUserFromRole(User.Identity.Name, "general_contractor");
                                break;
                            case "Sub-Contractor":
                                if (Roles.IsUserInRole("subcontractor"))
                                    Roles.RemoveUserFromRole(User.Identity.Name, "subcontractor");
                                break;
                            case "Architect":
                                if (Roles.IsUserInRole("architect"))
                                    Roles.RemoveUserFromRole(User.Identity.Name, "architect");
                                break;
                            case "Engineer":
                                if (Roles.IsUserInRole("engineer"))
                                    Roles.RemoveUserFromRole(User.Identity.Name, "engineer");
                                break;
                            case "@Owner/Client":
                                if (Roles.IsUserInRole("owner_client"))
                                    Roles.RemoveUserFromRole(User.Identity.Name, "owner_client");
                                break;
                            case "Materials Vendor":
                                if (Roles.IsUserInRole("materials_vendor"))
                                    Roles.RemoveUserFromRole(User.Identity.Name, "materials_vendor");
                                break;
                            case "Materials Manufacturer":
                                if (Roles.IsUserInRole("materials_manufacturer"))
                                    Roles.RemoveUserFromRole(User.Identity.Name, "materials_manufacturer");
                                break;
                            case "Consultant":
                                if (Roles.IsUserInRole("consultant"))
                                    Roles.RemoveUserFromRole(User.Identity.Name, "consultant");
                                break;
                        };
                    }
                    profile.BusinessTypeId = viewModel.BusinessTypeId;
                }

                if (viewModel.City != null && profile.City != viewModel.City)
                    profile.City = viewModel.City;

                if (viewModel.CompanyName != null && profile.CompanyName != viewModel.CompanyName.Trim())
                    profile.CompanyName = viewModel.CompanyName.Trim();

                if (viewModel.OperatingDistance != null && profile.OperatingDistance != viewModel.OperatingDistance)
                    profile.OperatingDistance = viewModel.OperatingDistance;

                if (viewModel.Phone != null && profile.Phone != viewModel.Phone.Trim())
                    profile.Phone = Util.ConvertPhoneForStorage(viewModel.Phone.Trim());

                if (viewModel.PostalCode != null && profile.PostalCode != viewModel.PostalCode.Trim())
                    profile.PostalCode = viewModel.PostalCode.Trim();

                if (viewModel.StateId != null && profile.StateId != viewModel.StateId)
                    profile.StateId = viewModel.StateId;

                // update changes in the database
                if (_serviceLayer.UpdateProfile(profile))
                {
                    return RedirectToAction("Manage", new { message = ManageMessageId.ChangeCompanyInfoSuccess });
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

        [HttpGet]
        public ActionResult ChangeEmail()
        {
            // get user profile
            var raw = _serviceLayer.GetProfile(WebSecurity.GetUserId(User.Identity.Name));
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

                    var toUpdate = _serviceLayer.GetProfile(viewModel.UserId);
                    toUpdate.Email = viewModel.NewEmail;

                    // if update successfull
                    if (_serviceLayer.UpdateProfile(toUpdate))
                    {
                        // logout and back in unobtrusively to refresh context
                        WebSecurity.Logout();
                        WebSecurity.Login(viewModel.NewEmail, viewModel.Password);
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
            if (!User.Identity.IsAuthenticated)
            {
                // confirm account to unlock it
                if (WebSecurity.ConfirmAccount(user, token))
                {
                    // force user to reset their passowrd
                    string passwordToken = WebSecurity.GeneratePasswordResetToken(user);
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
                if (WebSecurity.UserExists(viewModel.Email))
                {
                    // ResetPassword may throw an exception rather than return false in certain failure scenarios.
                    bool resetPasswordSucceeded;
                    try
                    {
                        resetPasswordSucceeded = WebSecurity.ResetPassword(viewModel.PasswordResetToken, viewModel.NewPassword);
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

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveSignInSuccess,
            ResetPasswordSuccess,
            ChangeCompanyInfoSuccess,
            ChangeEmailSuccess,
            ChangeProfileSuccess,
            NewDelegateSuccess
        }

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