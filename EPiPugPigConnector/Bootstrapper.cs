using EPiPugPigConnector.Fakes;
using EPiPugPigConnector.Manifest;
using StructureMap.Configuration.DSL;

namespace EPiPugPigConnector
{
    public class Bootstrapper : Registry
    {
        public Bootstrapper()
        {
            For<IEditionsGenerator>().Use<FakePageTreeEditionsGenerator>();
            For<IManifestFactory>().Use<FakeManifestFactory>();
        }
    }
}
