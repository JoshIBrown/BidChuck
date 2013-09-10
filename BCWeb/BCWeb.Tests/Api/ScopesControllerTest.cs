using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BCWeb.Areas.Account.Models.Scopes.ServiceLayer;
using BCWeb.Controllers.Api;
using BCWeb.Models;
using BCModel;
using System.Collections.Generic;
using BCWeb.Areas.Account.Models.Scopes.ViewModel;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using System.Threading;

namespace BCWeb.Tests.Api
{
    [TestClass]
    public class ScopesControllerTest
    {
        [TestMethod]
        public void CompanyGetScopesToMangeReturnsAllScopes()
        {
            // arrange
            List<Scope> scopes = new List<Scope> { new Scope { Id = 1, CsiNumber = "00 00 00", Description = "Concrete" }, 
                new Scope { Id = 2, CsiNumber = "01 00 00", Description = "Plumbing" }, 
                new Scope { Id = 3, CsiNumber="02 00 00", Description = "Electrical" },
                new Scope { Id = 4, CsiNumber="03 00 00", Description="General Requirements" }};

            UserProfile user = new UserProfile { UserId = 1, Email = "asdf@asdf.com", CompanyId = 1, Scopes = new List<UserXScope>() };
            CompanyProfile company = new CompanyProfile { Id = 1, CompanyName = "abu dabu", Users = new List<UserProfile> { user }, Scopes = new List<CompanyXScope>() };

            Mock<IScopeServiceLayer> service = new Mock<IScopeServiceLayer>();
            service.Setup(s => s.GetEnumerable()).Returns(scopes);
            service.Setup(s => s.GetUser(It.IsAny<int>())).Returns(user);
            service.Setup(s => s.GetCompany(It.IsAny<int>())).Returns(company);

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId("asdf@asfd.com")).Returns(1);

            ScopesController controller = new ScopesController(service.Object, security.Object);

            // act
            var result = controller.GetScopesToManage("company", "");

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ScopeMgmtViewModel[]));
            Assert.AreEqual(4, ((ScopeMgmtViewModel[])result).Length);
        }

        [TestMethod]
        public void SelfGetScopesToMangeReturnsCompanies()
        {
            // arrange
            List<Scope> scopes = new List<Scope> { new Scope { Id = 1, CsiNumber = "00 00 00", Description = "Concrete" }, 
                new Scope { Id = 2, CsiNumber = "01 00 00", Description = "Plumbing" }, 
                new Scope { Id = 3, CsiNumber="02 00 00", Description = "Electrical" },
                new Scope { Id = 4, CsiNumber="03 00 00", Description="General Requirements" }};

            UserProfile user = new UserProfile { UserId = 1, Email = "asdf@asdf.com", CompanyId = 1, Scopes = new List<UserXScope>() };
            CompanyProfile company = new CompanyProfile
            {
                Id = 1,
                CompanyName = "abu dabu",
                Users = new List<UserProfile> { user }
            };
            company.Scopes = new List<CompanyXScope> 
                { 
                    new CompanyXScope { Scope= scopes[1], ScopeId = scopes[1].Id, Company = company, CompanyId = company.Id }, 
                    new CompanyXScope{Scope= scopes[2], ScopeId = scopes[2].Id, Company = company, CompanyId = company.Id} 
                };

            Mock<IScopeServiceLayer> service = new Mock<IScopeServiceLayer>();
            service.Setup(s => s.GetEnumerable()).Returns(scopes);
            service.Setup(s => s.GetUser(It.IsAny<int>())).Returns(user);
            service.Setup(s => s.GetCompany(It.IsAny<int>())).Returns(company);

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId("asdf@asfd.com")).Returns(1);

            ScopesController controller = new ScopesController(service.Object, security.Object);

            // act
            var result = controller.GetScopesToManage("self", "");

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ScopeMgmtViewModel[]));
            Assert.AreEqual(2, ((ScopeMgmtViewModel[])result).Length);
        }


        [TestMethod]
        public void UserGetScopesToMangeReturnsCompanies()
        {
            // arrange
            List<Scope> scopes = new List<Scope> { new Scope { Id = 1, CsiNumber = "00 00 00", Description = "Concrete" }, 
                new Scope { Id = 2, CsiNumber = "01 00 00", Description = "Plumbing" }, 
                new Scope { Id = 3, CsiNumber="02 00 00", Description = "Electrical" },
                new Scope { Id = 4, CsiNumber="03 00 00", Description="General Requirements" }};

            UserProfile user = new UserProfile { UserId = 1, Email = "asdf@asdf.com", CompanyId = 1, Scopes = new List<UserXScope>() };
            CompanyProfile company = new CompanyProfile
            {
                Id = 1,
                CompanyName = "abu dabu",
                Users = new List<UserProfile> { user }
            };
            company.Scopes = new List<CompanyXScope> 
                { 
                    new CompanyXScope { Scope= scopes[1], ScopeId = scopes[1].Id, Company = company, CompanyId = company.Id }, 
                    new CompanyXScope{Scope= scopes[2], ScopeId = scopes[2].Id, Company = company, CompanyId = company.Id} 
                };

            Mock<IScopeServiceLayer> service = new Mock<IScopeServiceLayer>();
            service.Setup(s => s.GetEnumerable()).Returns(scopes);
            service.Setup(s => s.GetUser(It.IsAny<int>())).Returns(user);
            service.Setup(s => s.GetCompany(It.IsAny<int>())).Returns(company);

            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId("asdf@asfd.com")).Returns(1);

            ScopesController controller = new ScopesController(service.Object, security.Object);

            // act
            var result = controller.GetScopesToManage("user", "asdf@asdf.com");

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ScopeMgmtViewModel[]));
            Assert.AreEqual(2, ((ScopeMgmtViewModel[])result).Length);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void UserGetScopesToMangeFailWithManagerFromDifferentCompany()
        {
            // arrange
            List<Scope> scopes = new List<Scope> { new Scope { Id = 1, CsiNumber = "00 00 00", Description = "Concrete" }, 
                new Scope { Id = 2, CsiNumber = "01 00 00", Description = "Plumbing" }, 
                new Scope { Id = 3, CsiNumber="02 00 00", Description = "Electrical" },
                new Scope { Id = 4, CsiNumber="03 00 00", Description="General Requirements" }};

            UserProfile user = new UserProfile { UserId = 1, Email = "asdf@asdf.com", CompanyId = 1, Scopes = new List<UserXScope>() };
            UserProfile user2 = new UserProfile
            {
                UserId = 2,
                Email = "qwert@qwer.com",
                CompanyId = 2
                ,
                Scopes = new List<UserXScope>()
            };

            CompanyProfile company = new CompanyProfile
            {
                Id = 1,
                CompanyName = "abu dabu",
                Users = new List<UserProfile> { user }
            };
            user.Company = company;

            CompanyProfile company2 = new CompanyProfile
            {
                Id = 2,
                CompanyName = "xzcvzxv",
                Users = new List<UserProfile> { user2 }
            };
            user2.Company = company2;

            company.Scopes = new List<CompanyXScope> 
                { 
                    new CompanyXScope { Scope= scopes[1], ScopeId = scopes[1].Id, Company = company, CompanyId = company.Id }, 
                    new CompanyXScope{Scope= scopes[2], ScopeId = scopes[2].Id, Company = company, CompanyId = company.Id} 
                };

            Mock<IScopeServiceLayer> service = new Mock<IScopeServiceLayer>();
            service.Setup(s => s.GetEnumerable()).Returns(scopes);
            service.Setup(s => s.GetUser(1)).Returns(user);
            service.Setup(s => s.GetCompany(1)).Returns(company);
            service.Setup(s => s.GetUser(2)).Returns(user2);
            service.Setup(s => s.GetCompany(2)).Returns(company2);


            Mock<IWebSecurityWrapper> security = new Mock<IWebSecurityWrapper>();
            security.Setup(s => s.GetUserId("asdf@asdf.com")).Returns(1);
            security.Setup(s => s.GetUserId("qwert@qwer.com")).Returns(2);

            var identity = new GenericIdentity("qwert@qwer.com");

            Mock<IPrincipal> principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.Name).Returns("qwert@qwer.com");
            Thread.CurrentPrincipal = principal.Object;
            

            ScopesController controller = new ScopesController(service.Object, security.Object);
       
            
            // act
            var result = controller.GetScopesToManage("user", "asdf@asdf.com");

            // assert
            Assert.IsNotNull(result);
        }

    }
}
