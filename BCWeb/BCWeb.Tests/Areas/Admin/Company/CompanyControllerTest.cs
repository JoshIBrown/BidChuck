using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Areas.Admin.Controllers;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using BCModel;
using BCWeb.Areas.Admin.Models.Companies;

namespace BCWeb.Tests.Areas.Admin.Company
{
    [TestClass]
    public class CompanyControllerTest
    {
        [TestMethod]
        public void Get_Index_returns_ViewResult()
        {
            // arrange
            Mock<ICompanyProfileServiceLayer> service = new Mock<ICompanyProfileServiceLayer>();
            CompanyController controller = new CompanyController(service.Object);

            // act
            var result = controller.Index() as ViewResult;

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Get_Create_returns_ViewResult()
        {
            // arrange
            Mock<ICompanyProfileServiceLayer> service = new Mock<ICompanyProfileServiceLayer>();
            CompanyController controller = new CompanyController(service.Object);

            // act
            var result = controller.Create() as ViewResult;

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Get_Create_returns_ViewModel_with_States()
        {
            // arrange
            List<State> states = new List<State>();
            states.Add(new State { Id = 1, Abbr = "wa", Name = "washington" });
            states.Add(new State { Id = 2, Abbr = "or", Name = "oregon" });
            Mock<ICompanyProfileServiceLayer> service = new Mock<ICompanyProfileServiceLayer>();
            service.Setup(s => s.GetStates()).Returns(states);

            CompanyController controller = new CompanyController(service.Object);

            // act
            var result = controller.Create() as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(CompanyProfileEditModel));
            Assert.AreEqual(states.Count, ((CompanyProfileEditModel)result.Model).States.Count());
        }
    }
}
