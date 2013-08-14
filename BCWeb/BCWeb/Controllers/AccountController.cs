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

namespace BCWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private IGenericServiceLayer<UserProfile> _serviceLayer;

        public AccountController(IGenericServiceLayer<UserProfile> serviceLayer)
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
                UserProfile user = _serviceLayer.GetEnumerable(u => u.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();

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
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //[HttpGet]
        //public ActionResult ManageScopes()
        //{
        //    using (UsersContext uc = new UsersContext())
        //    {
        //        return View(new ScopesModel(uc.Scopes.ToList()));
        //    }
        //}

        //public ActionResult ManageScopes(ScopesModel model)
        //{
        //    return View(model);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
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
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
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
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
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
                //using (UsersContext uc = new UsersContext())
                //{
                UserProfile user = _serviceLayer.GetEnumerable(u => u.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();

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
                        return RedirectToAction("ResetPasswordSuccess");
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
                //}
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
            return View();
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
                            CompanyName = model.CompanyName,
                            Phone = model.Phone,
                            Published = false
                        }, true);

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
                UserProfile user = _serviceLayer.GetEnumerable(u => u.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();
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
        #region Helpers

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveSignInSuccess,
            ResetPasswordSuccess
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