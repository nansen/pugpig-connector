using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using EPiPugPigConnector.Editions.Factories;
using EPiPugPigConnector.Editions.Interfaces.Edition;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.ExtensionMethods;
using EPiPugPigConnector.Fakes;
using EPiPugPigConnector.Fakes.Pages;
using EPiPugPigConnector.HttpModules.Manifest;
using EPiPugPigConnector.Tests.TestUtilities;
using EPiPugPigConnector.Utils;
using EPiPugPigConnector.WebClients;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EPiPugPigConnector.Tests
{
    [TestClass]
    public class UnitTest_CssFileManifestAssetParser
    {
        [TestMethod]
        public void Test_Find_Image_Asset_Lines_In_Test_Css_File()
        {
            //Arrange
            var cssFile = GetTestCssFile();
            var serverMock = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);
            serverMock.Setup(i => i.MapPath(It.IsAny<String>()))
               .Returns((String a) => a.Replace("~/", @"http://pugpig.local/").Replace(@"\", "/"));  //TODO: place folder in appSettings for shared dev env.

            var projectDir = TestHelper.GetTestProjectDir().FullName;
            var abslouteCssFilePath = cssFile.FullName;
            abslouteCssFilePath = abslouteCssFilePath.Replace(projectDir, "~").Replace(@"\", "/");
            abslouteCssFilePath = serverMock.Object.MapPath(abslouteCssFilePath);

            IWebClient webClient = new FakeWebClientFactory().Create(TestHelper.GetTestProjectDir().FullName);
            List<string> linesToParse = new List<string>();

            if (cssFile.Exists)
            {
                //Act
                linesToParse = CssAssetProcessor.FindImageAssetsLinesInCssFile(webClient, abslouteCssFilePath);
            }

            //Assert
            Assert.IsTrue(linesToParse.Count > 0);
        }

        [TestMethod]
        public void Test_Parse_Image_Asset_Lines_As_Relative_Manifest_Urls()
        {
            //Arrange
            var cssFile = GetTestCssFile();
            List<string> parsedLines = new List<string>();

            var serverMock = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);
            serverMock.Setup(i => i.MapPath(It.IsAny<String>()))
               .Returns((String a) => a.Replace("~/", @"http://pugpig.local/").Replace(@"\", "/"));  //TODO: place folder in appSettings for shared dev env.

            var manifestFilePath = serverMock.Object.MapPath("~/");
            var projectDir = TestHelper.GetTestProjectDir().FullName;
            var abslouteCssFilePath = cssFile.FullName;
            abslouteCssFilePath = abslouteCssFilePath.Replace(projectDir, "~").Replace(@"\", "/");
            abslouteCssFilePath = serverMock.Object.MapPath(abslouteCssFilePath);

            IWebClient webClient = new FakeWebClientFactory().Create(TestHelper.GetTestProjectDir().FullName);

            if (cssFile.Exists)
            {
                //Act 
                var cssAssetProcessor = new CssAssetProcessor(webClient, manifestFilePath, abslouteCssFilePath);
                parsedLines = cssAssetProcessor.ProcessCssFile();
            }

            //Assert
            Assert.IsTrue(parsedLines.Count > 0);
        }

        

        private static FileInfo GetTestCssFile()
        {
            string cssFilePath = TestHelper.GetTestProjectDir().FullName + "\\TestFiles\\ManifestAssets\\Static\\css\\TestImageAssetsParsing.css";
            FileInfo cssFile = new FileInfo(cssFilePath);
            return cssFile;
        }

        

        //[TestMethod]
        //public void Test_Find_Assets_In_Css()
        //{
        //    //Arrange 
        //    var solutionDir = TestHelper.GetSolutionDir();
        //    var testProjectDir = TestHelper.GetTestProjectDir();
            
        //    //var cssDir =  new DirectoryInfo(Path.Combine(solutionDir.FullName + "\\DemoApp\\Static\\css"));
        //    var cssDir = new DirectoryInfo(Path.Combine(testProjectDir.FullName + "\\TestFiles\\ManifestAssets\\Static\\css"));

        //    if (cssDir.Exists)
        //    {
        //        var cssfiles = cssDir.GetFiles("*.css");
        //        List<string> imageRefList = new List<string>();

        //        foreach (FileInfo cssfile in cssfiles)
        //        {
        //            var fileString = TestHelper.GetStringFromFileInfo(cssfile);

        //            //todo:

        //            //psuedo code
        //            /* 
        //             * 1. find lines of external file refs.
        //             *  e.g. the string "url("
        //             *  2. Add the line to a list
        //             *  3. Process the list and parseout relative url path for each image.
        //             *  4. Add this to the manifest caching system in some intelligent way. (use the css file name as cachekey)
        //             *      - check if cached external urls already exists on cachekey.
        //             *  5. upon creating manifest add the cached filelist above to the manifest.
        //             */
                    
        //            //find lines of image refs and add to listoflines.
        //            //imageRefList.Add();
        //        }
        //    }

        //    //File.Open()

        //    //Act
        //    int result = 1 + 3;

        //    //Assert
        //    Assert.IsTrue(result == 4);
        //}
    }
}