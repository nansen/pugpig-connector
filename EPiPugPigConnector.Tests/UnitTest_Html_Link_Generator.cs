using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using EPiPugPigConnector.Editions.Factories;
using EPiPugPigConnector.Editions.Interfaces.Edition;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Fakes.Pages;
using EPiPugPigConnector.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiPugPigConnector.Tests
{
    [TestClass]
    public class UnitTest_Html_Link_Generator
    {
        [TestMethod]
        public void Test_Generate_Valid_Html_And_Manifest_Links_With_Extensions()
        {
            //Arrange
            //Test values: input url, expected output url, extension parameter
            List<Tuple<string, string, string>> testValues = new List<Tuple<string, string, string>>
            {
                //relative 
                new Tuple<string, string, string>("/page1/", "/page1.html", "html"),
                //absolute
                new Tuple<string, string, string>("http://pugpig.local/page1/", "http://pugpig.local/page1.html", "html"),
                //https
                new Tuple<string, string, string>("https://pugpig.local/page1/", "https://pugpig.local/page1.html", "html"),
                //relative with query string
                new Tuple<string, string, string>("/page1/?id=123&test=456", "/page1.html?id=123&test=456", "html"),
                //absolute with query string
                new Tuple<string, string, string>("https://pugpig.local/page1/?id=123&test=456", "https://pugpig.local/page1.html?id=123&test=456", "html"),
                //episerver start page (episerver dependent!)
                new Tuple<string, string, string>("/", "/start.html", "html"),
                
                //with other extension
                //new Tuple<string, string, string>("http:/pugpig.local/page1.aspx", "http:/pugpig.local/page1.html", "html"),
            };

            //Act
            foreach (Tuple<string, string, string> testValue in testValues)
            {
                //string friendlyUrl = page.GetFriendlyUrl(includeHost: false);

                var inputUrl = testValue.Item1;
                var expectedResult = testValue.Item2;
                var extension = testValue.Item3;

                string resultUrl = HtmlHelper.FriendlyUrlToUrlWithExtension(inputUrl, extension);
                
                //Assert
                Assert.AreEqual(expectedResult, resultUrl);
            }
        }
    }
}