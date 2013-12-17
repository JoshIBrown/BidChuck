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
using BCModel.Projects;
using BCModel;

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
            ProjectController controller = new ProjectController(service.Object, security.Object);

            // act
            var result = controller.Index();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("Index", ((ViewResult)result).ViewName);
        }

        [TestMethod]
        public void Get_Create_AsArchitect_Returns_CreateStepTwo_ViewResult()
        {
            // arrange
            UserProfile theUser = new UserProfile { UserId = 1, CompanyId = 1 };

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            service.Setup(s => s.GetUserProfile(1)).Returns(theUser);

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId("qwert@qwer.com")).Returns(1);

            ProjectController controller = new ProjectController(service.Object, security.Object);

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.Setup(c => c.HttpContext.User.IsInRole("architect")).Returns(true);
            context.Setup(p => p.HttpContext.User.Identity.Name).Returns("qwert@qwer.com");

            controller.ControllerContext = context.Object;

            // act
            var result = controller.Create();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("CreateStepTwo", ((ViewResult)result).ViewName);
        }

        [TestMethod]
        public void Get_Create_AsGC_Returns_Create_ViewResult()
        {
            // arrange
            UserProfile theUser = new UserProfile { UserId = 1, CompanyId = 1 };

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();

            ProjectController controller = new ProjectController(service.Object, security.Object);

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.Setup(c => c.HttpContext.User.IsInRole("architect")).Returns(false);

            controller.ControllerContext = context.Object;

            // act
            var result = controller.Create();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("Create", ((ViewResult)result).ViewName);
        }

        [TestMethod]
        public void Post_Create_UniqueProject_Redirects_CreateStepTwo()
        {
            // arrange
            DupeCheckViewModel viewModel = new DupeCheckViewModel
                {
                    ArchitectId = 3,
                    Number = "abc123 hello hello",
                    Title = "build a house for cake"
                };

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            service.Setup(s => s.FindDuplicate(viewModel.Title, viewModel.Number, viewModel.ArchitectId.Value)).Returns(new List<Project>()); // return empty list.  no matches found


            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            ProjectController controller = new ProjectController(service.Object, security.Object);

            // act
            var result = controller.Create(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("CreateStepTwo", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual("Project", ((RedirectToRouteResult)result).RouteValues["controller"]);
        }


        [TestMethod]
        public void PostCreateDuplicateRedirectsToDuplicates()
        {
            // arrange
            DupeCheckViewModel viewModel = new DupeCheckViewModel
            {
                ArchitectId = 3,
                Number = "abc123 hello hello",
                Title = "build a house for cake"
            };
            Project found = new Project { Id = 2, ArchitectId = viewModel.ArchitectId.Value, Title = viewModel.Title, Number = viewModel.Number };
            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            service.Setup(s => s.FindDuplicate(viewModel.Title, viewModel.Number, viewModel.ArchitectId.Value)).Returns(new List<Project> { found }); // return empty list.  no matches found


            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            ProjectController controller = new ProjectController(service.Object, security.Object);

            // act
            var result = controller.Create(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Duplicates", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual("Project", ((RedirectToRouteResult)result).RouteValues["controller"]);
        }

        [TestMethod]
        public void Post_Create_NewArchitect_RedirectsTo_CreateArchitect()
        {
            // arrange
            DupeCheckViewModel viewModel = new DupeCheckViewModel
            {
                Architect = "u build it, we draw it",
                Number = "abc123 hello hello",
                Title = "build a house for cake"
            };

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();


            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            ProjectController controller = new ProjectController(service.Object, security.Object);

            // act
            var result = controller.Create(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("CreateArchitect", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual("Company", ((RedirectToRouteResult)result).RouteValues["controller"]);
        }


        [TestMethod]
        public void Get_CreateStepTwo_Returns_ViewResult()
        {
            // arrange
            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            ProjectController controller = new ProjectController(service.Object, security.Object);

            // act
            var result = controller.CreateStepTwo(1, "a project", "abc123");

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("CreateStepTwo", ((ViewResult)result).ViewName);
        }

        [TestMethod]
        public void Post_CreateStepTwo_ValidProject_RedirectsTo_Details()
        {
            // arrange
            ProjectEditModel viewModel = new ProjectEditModel();
            viewModel.SelectedScope = new int[0];

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<BCModel.Projects.Project>())).Returns(true);
            service.Setup(s => s.GetUserProfile(It.IsAny<int>())).Returns(new BCModel.UserProfile { CompanyId = 1, UserId = 1 });

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId(It.IsAny<string>())).Returns(1);

            ProjectController controller = new ProjectController(service.Object, security.Object);


            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("qwer@qwer.com");
            context.SetupGet(c => c.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = context.Object;

            // act
            var result = controller.CreateStepTwo(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Details", ((RedirectToRouteResult)result).RouteValues["action"]);
        }

        [TestMethod]
        public void PostCreateStepTwoProjectValidationFailReturnsModelStateErrors()
        {
            // arrange
            ProjectEditModel viewModel = new ProjectEditModel();
            viewModel.SelectedScope = new int[0];

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<BCModel.Projects.Project>())).Returns(false);
            service.Setup(s => s.GetUserProfile(It.IsAny<int>())).Returns(new BCModel.UserProfile { CompanyId = 1, UserId = 1 });
            service.SetupGet(s => s.ValidationDic).Returns(new Dictionary<string, string> { { "Duplicate", "Title already exists" } });

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId(It.IsAny<string>())).Returns(1);

            ProjectController controller = new ProjectController(service.Object, security.Object);


            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("qwer@qwer.com");
            context.SetupGet(c => c.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = context.Object;

            // act
            var result = controller.CreateStepTwo(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("CreateStepTwo", ((ViewResult)result).ViewName);
            Assert.IsTrue(((ViewResult)result).ViewData.ModelState.ContainsKey("Duplicate"));
            Assert.AreEqual("Title already exists", ((ViewResult)result).ViewData.ModelState["Duplicate"].Errors[0].ErrorMessage.ToString());
        }

        [TestMethod]
        public void PostCreateStepTwoProjectThrowsExceptionReturnsModelStateErrors()
        {
            // arrange
            ProjectEditModel viewModel = new ProjectEditModel();
            viewModel.SelectedScope = new int[0];

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            service.Setup(s => s.Create(It.IsAny<BCModel.Projects.Project>())).Throws(new Exception("something broke"));
            service.Setup(s => s.GetUserProfile(It.IsAny<int>())).Returns(new BCModel.UserProfile { CompanyId = 1, UserId = 1 });

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId(It.IsAny<string>())).Returns(1);

            ProjectController controller = new ProjectController(service.Object, security.Object);


            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("qwer@qwer.com");
            context.SetupGet(c => c.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = context.Object;

            // act
            var result = controller.CreateStepTwo(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("CreateStepTwo", ((ViewResult)result).ViewName);
            Assert.IsTrue(((ViewResult)result).ViewData.ModelState.ContainsKey("Exception"));
            Assert.AreEqual("something broke", ((ViewResult)result).ViewData.ModelState["Exception"].Errors[0].ErrorMessage.ToString());
        }

        [TestMethod]
        public void PostEditValidProjectRedirectsToDetails()
        {
            // arrange
            ProjectEditModel viewModel = new ProjectEditModel { Id = 1, PostalCode = "98008" };
            viewModel.SelectedScope = new int[0];
            Project aProject = new Project
            {
                Id = 1,
                ArchitectId = 1,
                Scopes = new List<ProjectXScope>(),
                BidPackages = new List<BidPackage>(),
                Title = "foo"
            };
            aProject.BidPackages.Add(new BidPackage { Id = 1, IsMaster = true, Project = aProject, ProjectId = aProject.Id, Scopes = new List<BidPackageXScope>(), CreatedById = 1 });

            Mock<IProjectServiceLayer> service = new Mock<IProjectServiceLayer>();
            service.Setup(s => s.Update(It.IsAny<BCModel.Projects.Project>())).Returns(true);
            service.Setup(s => s.GetUserProfile(It.IsAny<int>())).Returns(new BCModel.UserProfile { CompanyId = 1, UserId = 1 });
            service.Setup(s => s.Get(It.IsAny<int>())).Returns(aProject);

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId(It.IsAny<string>())).Returns(1);

            ProjectController controller = new ProjectController(service.Object, security.Object);


            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");

            Mock<ControllerContext> context = new Mock<ControllerContext>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("qwer@qwer.com");
            context.SetupGet(c => c.HttpContext.Request.IsAuthenticated).Returns(true);

            controller.ControllerContext = context.Object;

            // act
            var result = controller.Edit(viewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Details", ((RedirectToRouteResult)result).RouteValues["action"]);
        }
    }
}
