using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using EPiPugPigConnector.Editions.Factories;
using EPiPugPigConnector.Editions.Interfaces.Edition;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Fakes.Pages;
using EPiServer.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiPugPigConnector.Tests
{
    [TestClass]
    public class UnitTest_Caching
    {
        [TestMethod]
        public void Test_Add_Manifest_To_Cache()
        {
            //Arrange 
            DateTime fakeDataCreated = DateTime.Now;
            var fakeManifestData = new
            {
                data = "FAKE MANIFEST",
                created = fakeDataCreated
            };
            string fakeKey = "Fake_Manifest_1";

            //Act
            //Caching.PugPigCache.Set(PugPigCacheType.Manifest, fakeKey, fakeManifestData);
            //Caching.DefaultCacheProvider.Set();

            Thread.Sleep(1000);

            //Assert
            Assert.IsTrue(3 == 3);
        }
    }
}

namespace EPiPugPigConnector.Caching
{
    public class PugPigCache
    {
       // Caching.DefaultCacheProvider.Set(
    }
}