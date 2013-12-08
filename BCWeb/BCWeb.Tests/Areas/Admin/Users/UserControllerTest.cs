using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BCWeb.Areas.Admin.Controllers;
using BCWeb.Areas.Account.Models.Users.ServiceLayer;
using BCWeb.Models;
using System.Web.Mvc;
using BCWeb.Areas.Admin.Models.Users;
using System.Collections.Generic;
using System.Linq;
using BCModel;

namespace BCWeb.Tests.Areas.Admin.Users
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IndexReturnsViewResult()
        {
            // arrange
            Mock<IUserProfileServiceLayer> service = new Mock<IUserProfileServiceLayer>();
            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> email = new Mock<IEmailSender>();

            UserController controller = new UserController(service.Object, security.Object, email.Object);

            // act
            var result = controller.Index();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Create_GET_ReturnsViewResult()
        {
            // arrange

            List<CompanyProfile> companies = new List<CompanyProfile>();
            companies.Add(new CompanyProfile { Id = 1, CompanyName = "some company" });
            companies.Add(new CompanyProfile { Id = 2, CompanyName = "some other company" });

            Mock<IUserProfileServiceLayer> service = new Mock<IUserProfileServiceLayer>();
            service.Setup(s => s.GetEnumerableCompanies()).Returns(companies);

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> email = new Mock<IEmailSender>();

            UserController controller = new UserController(service.Object, security.Object, email.Object);

            // act
            var result = controller.Create() as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(UserProfileEditModel));
            Assert.AreEqual(companies.Count, ((UserProfileEditModel)result.Model).Companies.Count());
        }


        [TestMethod]
        public void Edit_GET_ReturnsViewResult()
        {
            // arrange

            List<CompanyProfile> companies = new List<CompanyProfile>();
            companies.Add(new CompanyProfile { Id = 1, CompanyName = "some company" });
            companies.Add(new CompanyProfile { Id = 2, CompanyName = "some other company" });

            UserProfile theUser = new UserProfile
            {
                CompanyId = 1,
                UserId = 1,
                Company = companies.Where(x => x.Id == 1).Single(),
                Email = "somefool@email.com",
                FirstName = "some",
                LastName = "fool",
                JobTitle = "nerf herder"
            };

            Mock<IUserProfileServiceLayer> service = new Mock<IUserProfileServiceLayer>();
            service.Setup(s => s.GetEnumerableCompanies()).Returns(companies);
            service.Setup(s => s.Get(theUser.UserId)).Returns(theUser);

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> email = new Mock<IEmailSender>();

            UserController controller = new UserController(service.Object, security.Object, email.Object);

            // act
            var result = controller.Edit(theUser.UserId) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(UserProfileEditModel));
            Assert.AreEqual(companies.Count, ((UserProfileEditModel)result.Model).Companies.Count());
            Assert.AreEqual(theUser.FirstName, ((UserProfileEditModel)result.Model).FirstName);
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void Edit_GET_InvalidUserId()
        {
            // arrange

            List<CompanyProfile> companies = new List<CompanyProfile>();
            companies.Add(new CompanyProfile { Id = 1, CompanyName = "some company" });
            companies.Add(new CompanyProfile { Id = 2, CompanyName = "some other company" });

            UserProfile theUser = new UserProfile
            {
                CompanyId = 1,
                UserId = 1,
                Company = companies.Where(x => x.Id == 1).Single(),
                Email = "somefool@email.com",
                FirstName = "some",
                LastName = "fool",
                JobTitle = "nerf herder"
            };

            Mock<IUserProfileServiceLayer> service = new Mock<IUserProfileServiceLayer>();
            service.Setup(s => s.GetEnumerableCompanies()).Returns(companies);
            service.Setup(s => s.Get(theUser.UserId)).Returns(theUser);

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> email = new Mock<IEmailSender>();

            UserController controller = new UserController(service.Object, security.Object, email.Object);

            // act
            var result = controller.Edit(2) as ViewResult;

            // assert
            
        }
    }
}
