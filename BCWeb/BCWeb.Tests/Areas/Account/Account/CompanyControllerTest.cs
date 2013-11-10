using BCWeb.Areas.Account.Controllers;
using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Areas.Account.Models.Company.ViewModel;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BCWeb.Models;
using BCModel;
using System.Web.Mvc;
using System.Collections.Generic;

namespace BCWeb.Tests.Areas.Account.Account
{
    [TestClass]
    public class CompanyControllerTest
    {
        [TestMethod]
        public void Get_CreateArchitect_Returns_ViewResult()
        {
            // arrange
            Mock<ICompanyProfileServiceLayer> service = new Mock<ICompanyProfileServiceLayer>();
            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> email = new Mock<IEmailSender>();
            CompanyController controller = new CompanyController(service.Object, security.Object, email.Object);

            // act
            var result = controller.CreateArchitect("Architect Company Name", "The project Title", "The Project Number");

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Post_CreateArchitect_ValidModel_Returns_Redirect()
        {
            // arrange
            string currentUserEmail = "asdf@asdf.com";
            int currentUserId = 321;
            int newArchitectId = 123;

            NewArchitectViewModel viewModel = new NewArchitectViewModel
            {
                CompanyName = "Some company",
                ContactEmail = "qwer@qwer.com",
                ContactFirstName = "bob",
                ContactLastName = "builder",
                ProjectNumber = "abc123",
                ProjectTitle = "the project"
            };

            CompanyProfile company = new CompanyProfile
                {
                    BusinessType = BusinessType.Architect,
                    ContactEmail = viewModel.ContactEmail,
                    CompanyName = viewModel.CompanyName
                };

            CompanyProfile userCompany = new CompanyProfile
            {
                CompanyName = "another company"
            };

            UserProfile theUser = new UserProfile { UserId = currentUserId, Email = currentUserEmail, FirstName = "jim", LastName = "Corn", Company = userCompany };

            Mock<ICompanyProfileServiceLayer> service = new Mock<ICompanyProfileServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<CompanyProfile>())).Returns(true).Callback((CompanyProfile toCreate) => toCreate.Id = newArchitectId);
            service.Setup(s => s.GetUserProfile(321)).Returns(theUser);

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId(currentUserEmail)).Returns(currentUserId);
            security.Setup(s => s.UserExists(viewModel.ContactEmail)).Returns(false);
            security.Setup(s => s.CreateUserAndAccount(viewModel.ContactEmail, It.IsAny<string>(), new
                    {
                        FirstName = viewModel.ContactFirstName,
                        LastName = viewModel.ContactLastName,
                        CompanyId = company.Id
                    }
                    , true)).Returns("abcderf");

            Mock<IEmailSender> email = new Mock<IEmailSender>();


            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns(currentUserEmail);


            CompanyController controller = new CompanyController(service.Object, security.Object, email.Object);

            controller.ControllerContext = context.Object;

            // act
            var result = controller.CreateArchitect(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Default", ((RedirectToRouteResult)result).RouteName);
            Assert.AreEqual("Project", ((RedirectToRouteResult)result).RouteValues["controller"]);
            Assert.AreEqual("CreateStepTwo", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual(newArchitectId, ((RedirectToRouteResult)result).RouteValues["architect"]);
            Assert.AreEqual(viewModel.ProjectTitle, ((RedirectToRouteResult)result).RouteValues["title"]);
            Assert.AreEqual(viewModel.ProjectNumber, ((RedirectToRouteResult)result).RouteValues["number"]);
        }

        [TestMethod]
        public void Post_CreateArchitect_CannotCreateCompany_Returns_ViewModelErrors()
        {
            // arrange
            string currentUserEmail = "asdf@asdf.com";
            int currentUserId = 321;
            

            NewArchitectViewModel viewModel = new NewArchitectViewModel
            {
                CompanyName = "Some company",
                ContactEmail = "qwer@qwer.com",
                ContactFirstName = "bob",
                ContactLastName = "builder",
                ProjectNumber = "abc123",
                ProjectTitle = "the project"
            };

            CompanyProfile company = new CompanyProfile
            {
                BusinessType = BusinessType.Architect,
                ContactEmail = viewModel.ContactEmail,
                CompanyName = viewModel.CompanyName
            };

            CompanyProfile userCompany = new CompanyProfile
            {
                CompanyName = "another company"
            };

            UserProfile theUser = new UserProfile { UserId = currentUserId, Email = currentUserEmail, FirstName = "jim", LastName = "Corn", Company = userCompany };

            Mock<ICompanyProfileServiceLayer> service = new Mock<ICompanyProfileServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<CompanyProfile>())).Returns(false);

            service.SetupGet(s => s.ValidationDic).Returns(new Dictionary<string, string> { { "Error", "Cannot create company" } });

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.UserExists(viewModel.ContactEmail)).Returns(false);


            Mock<IEmailSender> email = new Mock<IEmailSender>();


            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns(currentUserEmail);


            CompanyController controller = new CompanyController(service.Object, security.Object, email.Object);

            controller.ControllerContext = context.Object;

            // act
            var result = controller.CreateArchitect(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
