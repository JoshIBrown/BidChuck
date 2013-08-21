using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using BCModel;
using BCWeb.Models;
using BCWeb.Models.Account.ServiceLayer;
using BCWeb.Models.Account.Repository;

namespace BCWeb
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    

            container.RegisterType<IGenericServiceLayer<UserProfile>, UserProfileServiceLayer>();
            container.RegisterType<IGenericRepository<UserProfile>, UserProfileRepository>();
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}