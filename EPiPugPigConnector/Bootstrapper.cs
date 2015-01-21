using EPiPugPigConnector.Controllers;
using EPiPugPigConnector.Editions;
using EPiPugPigConnector.Fakes;
using EPiPugPigConnector.ManifestOldImplementation;
using EPiServer;
using StructureMap.Configuration.DSL;

namespace EPiPugPigConnector
{
    public class Bootstrapper : Registry
    {
        public Bootstrapper()
        {
            //For<IEditionsGenerator>().Use<FakePageTreeEditionsGenerator>();
            
            For<IEditionsGenerator>().Use<PageTreeEditionsGenerator>();
            
            For<IManifestFactory>().Use<FakeManifestFactory>();
        }
    }
}
