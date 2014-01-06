using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BCWeb.Areas.Project.Models.BidPackage.ServiceLayer;
using BCWeb.Models;
using BCWeb.Areas.Project.Controllers;
using System.Web.Mvc;
using BCWeb.Areas.Project.Models.BidPackage.ViewModel;
using BCModel.Projects;
using BCModel;
using System.Security.Principal;
using System.Collections.Generic;
using BCWeb.Models.Notifications.ServiceLayer;

namespace BCWeb.Tests.Areas.Project.Controllers
{
    [TestClass]
    public class BidPackageControllerTest
    {


        [TestMethod]
        public void GetCreateReturnsViewModelWithProjectIdAndTemplateId()
        {
            // arrange
            Mock<IBidPackageServiceLayer> service = new Mock<IBidPackageServiceLayer>();
            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            Mock<INotificationSender> notice = new Mock<INotificationSender>();

            BidPackageController controller = new BidPackageController(service.Object, security.Object, notice.Object);

            // act
            var result = controller.Create(1, 1);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(((ViewResult)result).ViewData.Model, typeof(EditBidPackageViewModel));
            Assert.AreEqual(1, ((EditBidPackageViewModel)((ViewResult)result).ViewData.Model).ProjectId);
            Assert.AreEqual(1, ((EditBidPackageViewModel)((ViewResult)result).ViewData.Model).TemplateId);
        }


        [TestMethod]
        public void PostCreateValidBidPackageRedirectsToDetails()
        {
            // arrange
            Mock<IBidPackageServiceLayer> service = new Mock<IBidPackageServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<BidPackage>())).Returns(true);
            service.Setup(s => s.GetUser(It.IsAny<int>())).Returns(new UserProfile { CompanyId = 1, UserId = 1 });
            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId("qwert@qwer.com")).Returns(1);
            Mock<INotificationSender> notice = new Mock<INotificationSender>();

            BidPackageController controller = new BidPackageController(service.Object, security.Object, notice.Object);

            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("qwer@qwer.com");
            context.SetupGet(c => c.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = context.Object;

            EditBidPackageViewModel viewModel = new EditBidPackageViewModel
            {
                ProjectId = 1,
                TemplateId = 1,
                SelectedScope = new int[] { 1, 2, 3 },
                BidDateTime = new DateTime(2014, 2, 2, 17, 0, 0),
                Description = "booga booga"
            };

            // act
            var result = controller.Create(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("BidPackage", ((RedirectToRouteResult)result).RouteValues["controller"].ToString());
            Assert.AreEqual("Details", ((RedirectToRouteResult)result).RouteValues["action"].ToString());
        }


        [TestMethod]
        public void PostCreateInvalidBidPackageReturnsModelStateErrors()
        {
            // arrange
            Mock<IBidPackageServiceLayer> service = new Mock<IBidPackageServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<BidPackage>())).Returns(false);
            service.SetupGet(s => s.ValidationDic).Returns(new Dictionary<string, string> { { "Description", "You already have a bid package with this description" } });

            service.Setup(s => s.GetUser(It.IsAny<int>())).Returns(new UserProfile { CompanyId = 1, UserId = 1 });
            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId("qwert@qwer.com")).Returns(1);

            Mock<INotificationSender> notice = new Mock<INotificationSender>();

            BidPackageController controller = new BidPackageController(service.Object, security.Object, notice.Object);

            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("qwer@qwer.com");
            context.SetupGet(c => c.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = context.Object;

            EditBidPackageViewModel viewModel = new EditBidPackageViewModel
            {
                ProjectId = 1,
                TemplateId = 1,
                SelectedScope = new int[] { 1, 2, 3 },
                BidDateTime = new DateTime(2014, 2, 2, 17, 0, 0),
                Description = "booga booga"
            };

            // act
            var result = controller.Create(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(((ViewResult)result).ViewData.ModelState.ContainsKey("Description"));
            Assert.AreEqual("You already have a bid package with this description", ((ViewResult)result).ViewData.ModelState["Description"].Errors[0].ErrorMessage.ToString());
        }

        [TestMethod]
        public void PostCreateBidPackageExceptionReturnsModelStateErrors()
        {
            // arrange
            Mock<IBidPackageServiceLayer> service = new Mock<IBidPackageServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<BidPackage>())).Throws(new Exception("my spoon is too big"));
            service.SetupGet(s => s.ValidationDic).Returns(new Dictionary<string, string> { { "Description", "You already have a bid package with this description" } });

            service.Setup(s => s.GetUser(It.IsAny<int>())).Returns(new UserProfile { CompanyId = 1, UserId = 1 });
            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId("qwert@qwer.com")).Returns(1);

            Mock<INotificationSender> notice = new Mock<INotificationSender>();

            BidPackageController controller = new BidPackageController(service.Object, security.Object, notice.Object);

            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("qwer@qwer.com");
            context.SetupGet(c => c.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = context.Object;

            EditBidPackageViewModel viewModel = new EditBidPackageViewModel
            {
                ProjectId = 1,
                TemplateId = 1,
                SelectedScope = new int[] { 1, 2, 3 },
                BidDateTime = new DateTime(2014, 2, 2, 17, 0, 0),
                Description = "booga booga"
            };

            // act
            var result = controller.Create(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(((ViewResult)result).ViewData.ModelState.ContainsKey("Exception"));
            Assert.AreEqual("my spoon is too big", ((ViewResult)result).ViewData.ModelState["Exception"].Errors[0].ErrorMessage.ToString());
        }
    }
}
