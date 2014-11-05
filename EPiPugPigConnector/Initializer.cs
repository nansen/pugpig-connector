using System;
using System.Web.Mvc;
using EPiPugPigConnector.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace EPiPugPigConnector
{
    [InitializableModule]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class Initializer : IConfigurableModule 
    {
        public void Initialize(InitializationEngine context)
        {
            // Attach event handlers here
        }

        public void Uninitialize(InitializationEngine context)
        {
            // Detach any event handlers here
        }

        public void Preload(string[] parameters)
        {
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            // Add base encore registries
            context.Container.Configure(ctx => ctx.AddRegistry<Bootstrapper>());

            // Wire up MVC dependency resolvers
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(context.Container)); // mvc
        }
    }
}
