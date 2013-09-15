using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCWeb.Controllers;
using System.Web.Mvc;
using Moq;
using BCWeb.Models.Project.ServiceLayer;
using BCWeb.Models;
using BCWeb.Models.Project.ViewModel;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace BCWeb.Tests.Controllers
{
    [TestClass]
    public class ProjectControllerTest
    {
        [TestMethod]
        public void IndexReturnsViewResult()
        {
            // arrange
            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            ProjectsController controller = new ProjectsController(service.Object, security.Object);

            // act
            var result = controller.Index();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("Index", ((ViewResult)result).ViewName);
        }

        [TestMethod]
        public void GetCreateReturnsViewResult()
        {
            // arrange
            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            ProjectsController controller = new ProjectsController(service.Object, security.Object);

            // act
            var result = controller.Create();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("Create", ((ViewResult)result).ViewName);
        }

        [TestMethod]
        public void PostCreateValidProjectRedirectsToDetails()
        {
            // arrange
            EditProjectViewModel viewModel = new EditProjectViewModel();

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<BCModel.Projects.Project>())).Returns(true);

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId(It.IsAny<string>())).Returns(1);

            ProjectsController controller = new ProjectsController(service.Object, security.Object);


            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("qwer@qwer.com");
            context.SetupGet(c => c.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = context.Object;

            // act
            var result = controller.Create(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Details", ((RedirectToRouteResult)result).RouteValues["action"]);
        }

        [TestMethod]
        public void PostCreateProjectValidationFailReturnsModelStateErrors()
        {
            // arrange
            EditProjectViewModel viewModel = new EditProjectViewModel();

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<BCModel.Projects.Project>())).Returns(false);
            service.SetupGet(s => s.ValidationDic).Returns(new Dictionary<string, string> { { "Duplicate", "Title already exists" } });

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId(It.IsAny<string>())).Returns(1);

            ProjectsController controller = new ProjectsController(service.Object, security.Object);


            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("qwer@qwer.com");
            context.SetupGet(c => c.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = context.Object;

            // act
            var result = controller.Create(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("Create", ((ViewResult)result).ViewName);
            Assert.IsTrue(((ViewResult)result).ViewData.ModelState.ContainsKey("Duplicate"));
            Assert.AreEqual("Title already exists", ((ViewResult)result).ViewData.ModelState["Duplicate"].Errors[0].ErrorMessage.ToString());
        }

        [TestMethod]
        public void PostCreateProjectThrowsExceptionReturnsModelStateErrors()
        {
            // arrange
            EditProjectViewModel viewModel = new EditProjectViewModel();

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<BCModel.Projects.Project>())).Throws(new Exception("something broke"));

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId(It.IsAny<string>())).Returns(1);

            ProjectsController controller = new ProjectsController(service.Object, security.Object);


            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("qwer@qwer.com");
            context.SetupGet(c => c.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = context.Object;

            // act
            var result = controller.Create(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("Create", ((ViewResult)result).ViewName);
            Assert.IsTrue(((ViewResult)result).ViewData.ModelState.ContainsKey("Exception"));
            Assert.AreEqual("something broke", ((ViewResult)result).ViewData.ModelState["Exception"].Errors[0].ErrorMessage.ToString());
        }
    }
}
