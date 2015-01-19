using System;
using System.Web.Mvc;
using EPiPugPigConnector.Mvc;
using EPiServer;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace EPiPugPigConnector
{
    [InitializableModule]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class Initializer : IConfigurableModule 
    {
        private bool _eventsAttached = false;
        private readonly PugPigPageEventsHandler _pugPigEventsHandler = new PugPigPageEventsHandler();

        public void Initialize(InitializationEngine context)
        {
            // Attach event handlers here

            // from: http://tedgustaf.com/blog/2010/5/attach-episerver-event-handlers-on-startup-using-initializablemodule/
            // Attach event handlers unless they've already been attached
            // The initialize method may be executed repeatedly if an exception
            // is thrown, but we don't want additional event handlers to attach
            // if the exception is thrown later in the call stack
            if (!_eventsAttached)
            {
                // Attach event handler to when a page has been changed
                DataFactory.Instance.PublishingPage += _pugPigEventsHandler.Instance_PublishingPage;
                DataFactory.Instance.PublishedPage += _pugPigEventsHandler.Instance_PublishedPage;
                DataFactory.Instance.MovingPage += _pugPigEventsHandler.Instance_MovingPage;
                DataFactory.Instance.DeletedPage += _pugPigEventsHandler.Instance_DeletedPage;
                _eventsAttached = true;
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            // Detach event handlers
            DataFactory.Instance.PublishingPage -= _pugPigEventsHandler.Instance_PublishingPage;
            DataFactory.Instance.PublishedPage -= _pugPigEventsHandler.Instance_PublishedPage;
            DataFactory.Instance.MovedPage -= _pugPigEventsHandler.Instance_MovingPage;
            DataFactory.Instance.DeletedPage -= _pugPigEventsHandler.Instance_DeletedPage;
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
