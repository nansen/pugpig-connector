//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using System.Xml.Linq;
//using System.Xml.Schema;
//using EPiPugPigConnector.Editions.Factories;
//using EPiPugPigConnector.Editions.Interfaces.Edition;
//using EPiPugPigConnector.Editions.Interfaces.Editions;
//using EPiPugPigConnector.Fakes.Pages;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace EPiPugPigConnector.Tests.Editions.EditionsXml
//{
//    [TestClass]
//    public class TestEditionXmlFactory
//    {
//        [TestMethod]
//        public void Test_Generate_Edition_Xml()
//        {
//            //Arrange 
//            IEditionFeedElement editionsRootPage = new FakeEditionPage();

//            IEditionsEntryElement editionPage1 = new FakeEditionPage(
//                issuedAndUpdated: DateTime.Now.AddMonths(-2),
//                authorName: "Arnold Schwarzenegger",
//                entryTitle: "The body building magazine issue #4564");

//            IEditionsEntryElement editionPage2 = new FakeEditionPage(
//                issuedAndUpdated: DateTime.Now.AddMonths(-1),
//                authorName: "Abraham Lincoln",
//                entryTitle: "Christmas Magazine #1");

//            IEnumerable<IEditionsEntryElement> editionEntries = new List<IEditionsEntryElement>
//            {
//                editionPage1,
//                editionPage2
//            };

//            //Act
//            string resultXml = EditionsXmlFactory.GenerateXmlFrom(editionsRootPage, editionEntries);
//            //TODO: validate the feedxml at http://opds-validator.appspot.com/ (do it only manually perhaps?)


//            //Assert
//            string expectedXmlBeginning =
//                "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>" + System.Environment.NewLine +
//                "<feed xmlns:dcterms=\"http://purl.org/dc/terms/\" xmlns:opds=\"http://opds-spec.org/2010/catalog\" xmlns:app=\"http://www.w3.org/2007/app\" xmlns=\"http://www.w3.org/2005/Atom\">";

//            Assert.IsTrue(resultXml.StartsWith(expectedXmlBeginning));
//        }

//        [TestMethod]
//        public void Test_Create_Xml_Template_Model_From_Pages()
//        {
//            //Arrange 
//            IEditionsFeedElement editionsPage = new FakeEditionsPage();
//            IEditionsEntryElement editionPage = new FakeEditionPage();
//            //Act
//            //Assert
//            Assert.IsFalse(
//                    string.IsNullOrEmpty(editionsPage.FeedId) &&
//                    string.IsNullOrEmpty(editionsPage.FeedTitle) &&
//                    string.IsNullOrEmpty(editionPage.EntryId) &&
//                    string.IsNullOrEmpty(editionPage.EntryTitle)
//                );
//        }
//    }
//}