using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using EPiPugPigConnector.Editions.EditionsXml;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Fakes.Pages;
using EPiPugPigConnector.Models.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiPugPigConnector.Tests.Editions.EditionsXml
{
    [TestClass]
    public class TestEditionsXmlFactory
    {
        [TestMethod]
        public void Test_Generate_Editions_Xml()
        {
            //Arrange 
            IEditionsXmlFeedRoot editionsRootPage = new FakeEditionsPage();

            IEditionsXmlFeedEntry editionPage1 = new FakeEditionPage(
                issuedAndUpdated: DateTime.Now.AddMonths(-2),
                authorName: "Arnold Schwarzenegger",
                entryTitle: "The body building magazine issue #4564");

            IEditionsXmlFeedEntry editionPage2 = new FakeEditionPage(
                issuedAndUpdated: DateTime.Now.AddMonths(-1),
                authorName: "Abraham Lincoln",
                entryTitle: "Christmas Magazine #1");

            IEnumerable<IEditionsXmlFeedEntry> editionEntries = new List<IEditionsXmlFeedEntry>
            {
                editionPage1,
                editionPage2
            };

            //Act
            string resultXml = XmlFactory.GenerateEditionsXmlFrom(editionsRootPage, editionEntries);
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
            IEditionsXmlFeedRoot editionsPage = new FakeEditionsPage();
            IEditionsXmlFeedEntry editionPage = new FakeEditionPage();
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
    }
}