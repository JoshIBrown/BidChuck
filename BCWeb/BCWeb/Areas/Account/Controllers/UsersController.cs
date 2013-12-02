using BCModel;
using BCWeb.Areas.Account.Models.Users.ServiceLayer;
using BCWeb.Areas.Account.Models.Users.ViewModel;
using BCWeb.Helpers;
using BCWeb.Models;
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
    public class UserController : Controller
    {
        private IUserProfileServiceLayer _service;
        private IWebSecurityWrapper _security;
        private IEmailSender _emailer;

        public UserController(IUserProfileServiceLayer service,IWebSecurityWrapper security, IEmailSender emailer)
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Administrator")]
        public ActionResult Add(NewUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int currentUserId = _security.GetUserId(User.Identity.Name);
                UserProfile user = _service.Get(currentUserId);
                CompanyProfile company = _service.GetCompany(user.CompanyId);

                // use security context to create user
                string confirmToken = _security.CreateUserAndAccount(viewModel.Email,
                    "password",
                    new
                    {
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        CompanyId = user.CompanyId
                    }, true);

                // give them a minion role
                _security.AddUserToRole(viewModel.Email, "Employee");

                switch (company.BusinessType)
                {
                    case BusinessType.GeneralContractor:
                        _security.AddUserToRole(viewModel.Email, "general_contractor");
                        break;
                    case BusinessType.SubContractor:
                        _security.AddUserToRole(viewModel.Email, "subcontractor");
                        break;
                    case BusinessType.Architect:
                        _security.AddUserToRole(viewModel.Email, "architect");
                        break;
                    case BusinessType.Engineer:
                        _security.AddUserToRole(viewModel.Email, "engineer");
                        break;
                    case BusinessType.Owner:
                        _security.AddUserToRole(viewModel.Email, "owner_client");
                        break;
                    case BusinessType.MaterialsVendor:
                        _security.AddUserToRole(viewModel.Email, "materials_vendor");
                        break;
                    case BusinessType.MaterialsMfg:
                        _security.AddUserToRole(viewModel.Email, "materials_manufacturer");
                        break;
                    case BusinessType.Consultant:
                        _security.AddUserToRole(viewModel.Email, "consultant");
                        break;
                };

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
            var user = _service.Get(_security.GetUserId(email));
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
