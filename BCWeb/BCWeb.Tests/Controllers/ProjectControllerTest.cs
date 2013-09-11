using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCWeb.Controllers;
using System.Web.Mvc;
using Moq;
using BCWeb.Models.Project.ServiceLayer;
using BCWeb.Models;

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

    }
}
