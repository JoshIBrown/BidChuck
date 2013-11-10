using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BCWeb.Areas.Project.Models.Invitations.ServiceLayer;
using BCWeb.Models;
using BCWeb.Areas.Project.Controllers;
using System.Web.Mvc;
using BCWeb.Areas.Project.Models.Invitations.ViewModel;
using System.Collections;
using BCModel.Projects;
using System.Collections.Generic;


namespace BCWeb.Tests.Areas.Project.Controllers
{
    [TestClass]
    public class InvitationControllerTest
    {
        [TestMethod]
        public void GetSendReturnsViewModelWithBidPackageId()
        {
            // arrange
            Mock<IInvitationServiceLayer> service = new Mock<IInvitationServiceLayer>();
            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> email = new Mock<IEmailSender>();

            InvitationController controller = new InvitationController(service.Object, security.Object, email.Object);


            // act
            var result = controller.Send(1);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(((ViewResult)result).ViewData.Model, typeof(BidPackageInvitationViewModel));
            Assert.AreEqual(1, ((BidPackageInvitationViewModel)((ViewResult)result).ViewData.Model).BidPackageId);
        }


        [TestMethod]
        public void PostSendValidModelRedirectsToProjectDetails()
        {
            // arrange
            Mock<IInvitationServiceLayer> service = new Mock<IInvitationServiceLayer>();
            service.Setup(s => s.CreateRange(It.IsAny<IEnumerable<Invitation>>())).Returns(true);
            service.Setup(s => s.GetBidPackage(1)).Returns(new BidPackage { Id = 1, ProjectId = 1 });

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> email = new Mock<IEmailSender>();

            InvitationController controller = new InvitationController(service.Object, security.Object, email.Object);

            BidPackageInvitationViewModel viewModel = new BidPackageInvitationViewModel();
            viewModel.BidPackageId = 1;
            viewModel.CompanyId = new List<int> { 1, 2, 3, 4, 5, 6 };

            // act
            var result = controller.Send(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Project", ((RedirectToRouteResult)result).RouteValues["Controller"]);
            Assert.AreEqual("Details", ((RedirectToRouteResult)result).RouteValues["Action"]);
            Assert.AreEqual(1, ((RedirectToRouteResult)result).RouteValues["id"]);
        }

        [TestMethod]
        public void PostSendInvalidModelReturnsViewWithModelStatErros()
        {
            // arrange
            Mock<IInvitationServiceLayer> service = new Mock<IInvitationServiceLayer>();
            service.Setup(s => s.CreateRange(It.IsAny<IEnumerable<Invitation>>())).Returns(false);
            service.SetupGet(s => s.ValidationDic).Returns(new Dictionary<string, string> { { "Duplicate", "There is already an invitation sent to this company" } });
            service.Setup(s => s.GetBidPackage(1)).Returns(new BidPackage { Id = 1, ProjectId = 1 });

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            Mock<IEmailSender> email = new Mock<IEmailSender>();

            InvitationController controller = new InvitationController(service.Object, security.Object, email.Object);

            BidPackageInvitationViewModel viewModel = new BidPackageInvitationViewModel();
            viewModel.BidPackageId = 1;
            viewModel.CompanyId = new List<int> { 1, 2, 3, 4, 5, 6 };

            // act
            var result = controller.Send(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(((ViewResult)result).ViewData.Model, typeof(BidPackageInvitationViewModel));
            Assert.AreEqual(1, ((BidPackageInvitationViewModel)((ViewResult)result).ViewData.Model).BidPackageId);
            Assert.AreEqual("There is already an invitation sent to this company", ((ViewResult)result).ViewData.ModelState["Duplicate"].Errors[0].ErrorMessage);
        }
    }
}
