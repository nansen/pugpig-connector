using EPiPugPigConnector.Fakes;
using StructureMap.Configuration.DSL;

namespace EPiPugPigConnector
{
    public class Bootstrapper : Registry
    {
        public Bootstrapper()
        {
            For<IEditionsGenerator>().Use<FakePageTreeEditionsGenerator>();
        }
    }
}
