﻿using EPiPugPigConnector.Controllers;
using EPiPugPigConnector.Editions;
using EPiPugPigConnector.Fakes;
using EPiPugPigConnector.WebClients;
//using EPiPugPigConnector.ManifestOldImplementation;
using EPiServer;
using EPiServer.Web.Routing;
using StructureMap.Configuration.DSL;

namespace EPiPugPigConnector
{
    public class Bootstrapper : Registry
    {
        public Bootstrapper()
        {
            //For<IEditionsGenerator>().Use<FakePageTreeEditionsGenerator>();
            
            For<IEditionsGenerator>().Use<PageTreeEditionsGenerator>();
            For<IWebClient>().Use<SystemWebClient>();

            //For<IManifestFactory>().Use<FakeManifestFactory>();
        }
    }
}
