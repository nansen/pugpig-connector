using System;
using System.Collections.Generic;
using EPiPugPigConnector.Editions.Factories;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Fakes.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EPiPugPigConnector.Tests
{
    [TestClass]
    public class UnitTest_EditionsXmlFactory
    {
        ///// <summary>
        ///// Mock episerver dependencies here
        ///// </summary>
        //[TestInitialize]
        //public void MockEPiServerDependenciese()
        //{
        //    //From http://tedgustaf.com/blog/2013/10/unit-testing-in-episerver-7/

        //    // Create a mock UrlResolver
        //    var mockUrlResolver = new Mock<UrlResolver>();

        //    // Setup the UrlResolver to return an url with a preset value
            
        //    mockUrlResolver
        //        .Setup(urlResolver => urlResolver.GetUrl(""))
        //        .Returns("/test/");

        //    // Create a mock service locator
        //    var mockLocator = new Mock<IServiceLocator>();

        //    // Setup the service locator ("fool episerver") 
        //    // to return our mock repository when an UrlResolver is requested
        //    mockLocator.Setup(l => l.GetInstance<UrlResolver>()).Returns(mockUrlResolver.Object);

        //    // Make use of our mock objects throughout EPiServer
        //    ServiceLocator.SetLocator(mockLocator.Object);
        //}

        [TestMethod]
        public void Test_Generate_Editions_Xml()
        {
            //Arrange 
            IEditionsFeedElement editionsRootPage = new FakeEditionsPage();

            IEditionsEntryElement editionPage1 = new FakeEditionPage(
                issuedAndUpdated: DateTime.Now.AddMonths(-2),
                authorName: "Arnold Schwarzenegger",
                entryTitle: "The body building magazine issue #4564");

            IEditionsEntryElement editionPage2 = new FakeEditionPage(
                issuedAndUpdated: DateTime.Now.AddMonths(-1),
                authorName: "Abraham Lincoln",
                entryTitle: "Christmas Magazine #1");

            IEnumerable<IEditionsEntryElement> editionEntries = new List<IEditionsEntryElement>
            {
                editionPage1,
                editionPage2
            };

            //Act
            string resultXml = EditionsXmlFactory.GenerateXmlFrom(editionsRootPage, editionEntries, includeGeneratedTimeComment: false);
            //TODO: validate the feedxml at http://opds-validator.appspot.com/ (do it only manually perhaps?)


            //Assert
            string expextedXmlBeginning =
                "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>" + System.Environment.NewLine +
                "<feed xmlns:dcterms=\"http://purl.org/dc/terms/\" xmlns:opds=\"http://opds-spec.org/2010/catalog\" xmlns:app=\"http://www.w3.org/2007/app\" xmlns=\"http://www.w3.org/2005/Atom\">";

            Assert.IsTrue(resultXml.StartsWith(expextedXmlBeginning));
        }

        [TestMethod]
        public void Test_Create_Xml_Template_Model_From_Pages()
        {
            //Arrange 
            IEditionsFeedElement editionsPage = new FakeEditionsPage();
            IEditionsEntryElement editionPage = new FakeEditionPage();
            //Act
            //Assert
            Assert.IsFalse(
                    string.IsNullOrEmpty(editionsPage.FeedId) &&
                    string.IsNullOrEmpty(editionsPage.FeedTitle) &&
                    string.IsNullOrEmpty(editionPage.EntryId) &&
                    string.IsNullOrEmpty(editionPage.EntryTitle)
                );
        }

        //TODO: validate xml against against a xsd schema, 
        //to create a schema file: when active xml file in vs, menu xml -> create schema file.
        ////get xsd schema file
        //var xsdFile = Resources.editionsNoEntries;
        //XmlSchemaSet schemas = new XmlSchemaSet();
        //schemas.Add("", XmlReader.Create(new StringReader(xsdFile)));
        ////validate the xml against the xsd schema file.
        //rootDocument.Validate(schemas, ValidationCallback);
        //static void ValidationCallback(object sender, ValidationEventArgs args)
        //{
        //    if (args.Severity == XmlSeverityType.Warning)
        //        throw new Exception("XSD WARNING: " + args.Message);
        //    else if (args.Severity == XmlSeverityType.Error)
        //        throw new Exception("XSD ERROR: " + args.Message);
        //}
        //END TOTO VALIDATE XML


        //[TestMethod]
        //public void Test_Load_Xml_Template()
        //{
        //    //Arrange
        //    XmlDocument xmlTemplate = null;
        //    //Act
        //    xmlTemplate = XmlTemplateLoader.GetEditionsRootTemplate();
        //    //Assert
        //    Assert.IsTrue(xmlTemplate != null);
        //}

        //public string GetEdition(string id)
        //{
        //  //old fake:
        //  var filePath = HttpContext.Current.Server.MapPath("~\\App_Data\\FakeEdition.xml");
        //  var sb = LoadXmlFile(filePath);
        //  return sb.ToString();
        //}

        //private static StringBuilder LoadXmlFile(string filePath)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    using (StreamReader sr = new StreamReader(filePath))
        //    {
        //        String line;
        //        // Read and display lines from the file until the end of 
        //        // the file is reached.
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            sb.AppendLine(line);
        //        }
        //    }
        //    return sb;
        //}
    }
}