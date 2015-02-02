using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using EPiPugPigConnector.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiPugPigConnector.Tests
{
    [TestClass]
    public class UnitTest_Caching
    {
        private string _urlCacheKey;
        private FakeManifestData _fakeManifestData;

        [TestInitialize]
        public void Test_Init()
        {
            //Init code that runs before testmethods

            //Arrange 
            DateTime fakeDataCreated = DateTime.Now;
            _fakeManifestData = new FakeManifestData
            {
                Id = 1,
                Data = "FAKE MANIFEST",
                Created = fakeDataCreated
            };
            _urlCacheKey = "Fake_Manifest_1";

            //Act
            PugPigCache.Set(PugPigCacheType.Manifest, _urlCacheKey, _fakeManifestData);
        }

        [TestMethod]
        public void Test_Manifest_Cache_Set_And_Get()
        {
            //Assert
            var result = PugPigCache.Get(PugPigCacheType.Manifest, _urlCacheKey) as FakeManifestData;
            Assert.IsTrue(result != null && result.Id == 1);
        }        
        

        /// <summary>
        /// Uses the Feed enum value.
        /// </summary>
        [TestMethod]
        public void Test_Feed_Cache_Set_And_Get()
        {
            //Arrange
            string xmlData = "<xml />";
            string urlKey = "http://pugpig.local/editions.xml";

            //Act
            PugPigCache.Set(PugPigCacheType.Feed, urlKey, xmlData);

            //Assert
            var result = PugPigCache.Get(PugPigCacheType.Feed, urlKey) as string;
            Assert.IsTrue(result != null && result.Equals(xmlData));
        }       
        
        [TestMethod]
        public void Test_Manifest_Cache_IsSet()
        {
            //Assert
            Assert.IsTrue(PugPigCache.IsSet(PugPigCacheType.Manifest, _urlCacheKey));
        }

        [TestMethod]
        public void Test_Manifest_Cache_Invalidate()
        {
            //Arrange 
            var testData = new FakeManifestData
            {
                Data = "Fake_Manifest_Invalidate_Test",
            };
            var cacheKey = "Fake_Manifest_Invalidate_Test";

            //Act
            PugPigCache.Set(PugPigCacheType.Manifest, cacheKey, testData);
            PugPigCache.Invalidate(PugPigCacheType.Manifest, cacheKey);

            //Assert
            Assert.IsFalse(PugPigCache.IsSet(PugPigCacheType.Manifest, cacheKey));
        }
    }

    public class FakeManifestData
    {
        public string Data { get; set; }
        public DateTime Created { get; set; }
        public int Id { get; set; }
    }
}