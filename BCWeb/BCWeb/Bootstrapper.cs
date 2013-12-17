using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using BCModel;
using BCWeb.Models;
using BCWeb.Models.Account.ServiceLayer;
using BCWeb.Models.Account.Repository;
using System.Web.Http;
using BCWeb.Areas.Account.Models.Scopes.Repository;
using BCWeb.Areas.Account.Models.Scopes.ServiceLayer;
using BCWeb.Areas.Account.Models.Users.ServiceLayer;
using BCWeb.Areas.Account.Models.Users.Repository;
using BCWeb.Areas.Account.Models.Company.Repository;
using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Models.Project.Repository;
using BCWeb.Models.Project.ServiceLayer;
using BCWeb.Areas.Project.Models.BidPackage.Repository;
using BCWeb.Areas.Project.Models.BidPackage.ServiceLayer;
using BCWeb.Areas.Project.Models.Invitations.Repository;
using BCWeb.Areas.Project.Models.Invitations.ServiceLayer;
using BCWeb.Areas.Project.Models.Bids.Repository;
using BCWeb.Areas.Project.Models.Bids.ServiceLayer;
using BCWeb.Areas.Project.Models.Documents.ServiceLayer;
using BCWeb.Areas.Project.Models.Documents.Repository;

namespace BCWeb
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            // for mvc
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            // for web api
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    

            container.RegisterType<IProjectDocRepository, ProjectDocRepository>();
            container.RegisterType<IProjectDocServiceLayer, ProjectDocServiceLayer>();
            container.RegisterType<IBidServiceLayer, BidServiceLayer>();
            container.RegisterType<IBidRepository, BidRepository>();
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IAccountServiceLayer, AccountServiceLayer>();
            container.RegisterType<IUserProfileServiceLayer, UserProfileServiceLayer>();
            container.RegisterType<IUserProfileRepository, UserProfileRepository>();
            container.RegisterType<IScopeRepository, ScopeRepository>();
            container.RegisterType<IScopeServiceLayer, ScopeServiceLayer>();
            container.RegisterType<IWebSecurityWrapper, WebSecurityWrapper>();
            container.RegisterType<IEmailSender, EmailSender>();
            container.RegisterType<ICompanyProfileRepository, CompanyProfileRepository>();
            container.RegisterType<ICompanyProfileServiceLayer, CompanyProfileServiceLayer>();
            container.RegisterType<IProjectRepository, ProjectRepository>();
            container.RegisterType<IProjectServiceLayer, ProjectServiceLayer>();
            container.RegisterType<IBidPackageRepository, BidPackageRepository>();
            container.RegisterType<IBidPackageServiceLayer, BidPackageServiceLayer>();
            container.RegisterType<IInvitationRepository, InvitationRepository>();
            container.RegisterType<IInvitationServiceLayer, InvitationServiceLayer>();
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}