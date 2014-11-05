using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Microsoft.Practices.ServiceLocation;
using StructureMap;


namespace EPiPugPigConnector.Mvc
{
    /// <summary>
    ///     MVC Dependency resolver using <see cref="StructureMap" />. Is injected during initialization, see <see cref="Initializer" />.
    /// </summary>
    /// <remarks>
    ///     Using a dependency resolver in MVC controllers enable you to simply add a argument of a type (that can be retreived through the dependency resolver) to the constructor of your controller. Like so:
    ///     <code>
    ///  class MyController : Controller 
    ///  {
    /// 		private ISomeFactory _factory;
    /// 		public MyController(ISomeFactory someFactory)
    /// 		{
    /// 			// someFactory is resolved by the dependency resolver for you.
    /// 			_factory = someFactory;
    /// 		}
    ///  }
    ///  </code>
    /// </remarks>
    public class StructureMapDependencyResolver : ServiceLocatorImplBase, IDependencyResolver
    {
        protected readonly IContainer Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">container</exception>
        public StructureMapDependencyResolver(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Container = container;
        }

        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        /// <returns>
        /// The retrieved collection of services.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetAllInstances(serviceType).Cast<object>();
        }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>
        /// The dependency scope.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            var child = Container.GetNestedContainer();
            return new StructureMapDependencyResolver(child);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Container.Dispose();
        }

        /// <summary>
        /// Implementation of <see cref="M:System.IServiceProvider.GetService(System.Type)" />.
        /// </summary>
        /// <param name="serviceType">The requested service.</param>
        /// <returns>
        /// The requested object.
        /// </returns>
        public override object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }

            try
            {
                return serviceType.IsAbstract || serviceType.IsInterface
                           ? Container.TryGetInstance(serviceType)
                           : Container.GetInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// This method will do the actual work of resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>
        /// Sequence of service instance objects.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return Container.GetAllInstances(serviceType).Cast<object>();
        }

        /// <summary>
        /// This method will do the actual work of resolving the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return serviceType.IsAbstract || serviceType.IsInterface
                           ? Container.TryGetInstance(serviceType)
                           : Container.GetInstance(serviceType);
            }

            return Container.GetInstance(serviceType, key);
        }
    }
}
