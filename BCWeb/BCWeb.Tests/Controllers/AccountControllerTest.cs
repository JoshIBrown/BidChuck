using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BCWeb.Models;
using BCWeb.Controllers;
using System.Web.Mvc;
using System.Collections;
using BCModel;
using System.Collections.Generic;
using BCWeb.Models.Account.ServiceLayer;

namespace BCWeb.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {


        [TestMethod]
        public void CanGetToRegister()
        {
            // arrange
            Mock<IAccountServiceLayer> mockService = new Mock<IAccountServiceLayer>();
            Mock<IWebSecurityWrapper> mockSecurity = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> mockEmail = new Mock<IEmailSender>();

            AccountController controller = new AccountController(mockService.Object, mockSecurity.Object, mockEmail.Object);

            // act
            var result = controller.Register();


            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(((ViewResult)result).ViewName, "Register");
            Assert.IsInstanceOfType(((ViewResult)result).Model, typeof(RegisterModel));
        }

        [TestMethod]
        public void CanRegisterAsNewUser()
        {
            // arrange
            Mock<IAccountServiceLayer> mockService = new Mock<IAccountServiceLayer>();
            Mock<IWebSecurityWrapper> mockSecurity = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> mockEmail = new Mock<IEmailSender>();

            mockService.Setup(m => m.GetBusinessTypes()).Returns(new List<BusinessType> { new BusinessType { Id = 1, Name = "GC" }, new BusinessType { Id = 2, Name = "Vendor" } });

            mockSecurity.Setup(m => m.CreateUserAndAccount("asdf@asdf.com", "password", new
            {
                BusinessTypeId = 1,
                FirstName = "john",
                LastName = "smith",
                CompanyName = "asdf",
                PostalCode = "98074",
                Phone = "1231231234",
                OperatingDistance = 50
            }, true)).Returns("abc123");

            AccountController controller = new AccountController(mockService.Object, mockSecurity.Object, mockEmail.Object);

            // act
            var result = controller.Register(new RegisterModel
            {
                Email = "asdf@asdf.com",
                Password = "password",
                ConfirmPassword = "password",
                BusinessTypeId = 1,
                FirstName = "john",
                LastName = "smith",
                CompanyName = "asdf",
                PostalCode = "98074",
                Phone = "1231231234",
                OperatingDistance = 50
            }); ;

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("RegisterStepTwo", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual("Account", ((RedirectToRouteResult)result).RouteValues["controller"]);
        }


        [TestMethod]
        public void CanSignIn()
        {
            // arrange
            Mock<IAccountServiceLayer> mockService = new Mock<IAccountServiceLayer>();
            Mock<IWebSecurityWrapper> mockSecurity = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> mockEmail = new Mock<IEmailSender>();

            // security returns true when logging in a user
            mockSecurity.Setup(m => m.Login("asdf@asdf.com", "password", false)).Returns(true);

            AccountController controller = new AccountController(mockService.Object, mockSecurity.Object, mockEmail.Object);

            // act
            var result = controller.SignIn(new SignInModel { Email = "asdf@asdf.com", Password = "password", RememberMe = false }, "");


            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual("Home", ((RedirectToRouteResult)result).RouteValues["controller"]);
        }


        [TestMethod]
        public void ConfirmRegistrationCodeRedirectsToSuuccess()
        {

            // arrange
            Mock<IAccountServiceLayer> mockService = new Mock<IAccountServiceLayer>();
            Mock<IWebSecurityWrapper> mockSecurity = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> mockEmail = new Mock<IEmailSender>();

            mockSecurity.Setup(m => m.ConfirmAccount("abcdef")).Returns(true);

            AccountController controller = new AccountController(mockService.Object, mockSecurity.Object, mockEmail.Object);

            // act
            var result = controller.RegisterConfirmation("abcdef");

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("ConfirmationSuccess", ((RedirectToRouteResult)result).RouteValues["action"]);

        }

        [TestMethod]
        public void ConfirmRegistrationCodeRedirectsToFailure()
        {

            // arrange
            Mock<IAccountServiceLayer> mockService = new Mock<IAccountServiceLayer>();
            Mock<IWebSecurityWrapper> mockSecurity = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> mockEmail = new Mock<IEmailSender>();

            mockSecurity.Setup(m => m.ConfirmAccount("abcdef")).Returns(false);

            AccountController controller = new AccountController(mockService.Object, mockSecurity.Object, mockEmail.Object);

            // act
            var result = controller.RegisterConfirmation("abcdef");

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("ConfirmationFailure", ((RedirectToRouteResult)result).RouteValues["action"]);

        }


        [TestMethod]
        public void ResetPasswordWithUnknownEmailFails()
        {

            // arrange
            Mock<IAccountServiceLayer> mockService = new Mock<IAccountServiceLayer>();
            Mock<IWebSecurityWrapper> mockSecurity = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> mockEmail = new Mock<IEmailSender>();

            mockSecurity.Setup(m => m.GetUserId("qwer@qwer.com")).Returns(-1);

            AccountController controller = new AccountController(mockService.Object, mockSecurity.Object, mockEmail.Object);

            // act
            var result = controller.ResetPassword(new ResetPasswordModel { ConfirmPassword = "newpassword", NewPassword = "newpassword", Email = "asdf@asfd.com", PasswordResetToken = "abc123" });

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(((ViewResult)result).Model, typeof(ResetPasswordModel));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
            Assert.AreEqual("Unknown email address.", ((ViewResult)result).ViewData.ModelState["Email"].Errors[0].ErrorMessage);
        }


    }
}
