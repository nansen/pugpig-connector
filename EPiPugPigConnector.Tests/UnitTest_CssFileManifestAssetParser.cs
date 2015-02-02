using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using EPiPugPigConnector.Editions.Factories;
using EPiPugPigConnector.Editions.Interfaces.Edition;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.ExtensionMethods;
using EPiPugPigConnector.Fakes.Pages;
using EPiPugPigConnector.Tests.TestUtilities;
using EPiPugPigConnector.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            List<string> linesToParse = new List<string>();

            if (cssFile.Exists)
            {
                //Act    
                linesToParse = FindImageAssetsLinesInCssFile(cssFile);
            }

            //Assert
            Assert.IsTrue(linesToParse.Count > 0);
        }

        [TestMethod]
        public void Test_Parse_Image_Asset_Lines_As_Relative_Manifest_Urls()
        {
            //Arrange
            var cssFile = GetTestCssFile();
            List<string> linesToParse = new List<string>();
            List<string> parsedLines = new List<string>();

            if (cssFile.Exists)
            {
                //Act    
                linesToParse = FindImageAssetsLinesInCssFile(cssFile);
                parsedLines = ConvertImageAssetLinesToRelativeUrls(linesToParse, cssFile);
            }

            //Assert
            Assert.IsTrue(linesToParse.Count > 0);
        }

        private List<string> ConvertImageAssetLinesToRelativeUrls(List<string> linesToParse, FileInfo cssFile)
        {
            var resultList = new List<string>();
            foreach (var line in linesToParse)
            {
                string resultString = ParseUrlSegmentFrom(line);
                resultString = RemoveUnwantedCharacters(resultString);
                resultString = ConvertCssRelativeToManifestRelativeString(resultString, cssFile);

                if (resultString.IsNotNullOrEmpty())
                {
                    resultList.Add(resultString);
                }
            }
            return resultList;
        }

        private string ConvertCssRelativeToManifestRelativeString(string resultString, FileInfo cssFile)
        {
            //todo: the css file is always relative to Server.MapPath("~").
            // use this fact to convert ../../ to manifest relative. e.g. /fkdskjfas/jfkdsja/image.png
            //ps. in this testingcontext we have no http context so we must fake the server mappath(~) in here.
            // use moq etc?
            
            throw new NotImplementedException();
        }

        private string RemoveUnwantedCharacters(string resultString)
        {
            //todo: remove ' " and such chars from string
            
        }

        private string ParseUrlSegmentFrom(string line)
        {
            string result = null;

            //remove all text but text inside the first ( )
            if (IsValidUrlSegment(line))
            {
                int leftPos = line.IndexOf('(') + 1;
                int rightPos = line.IndexOf(')');
                int stringLength = rightPos - leftPos;

                if (stringLength > 0 && (line.Length >= leftPos+stringLength))
                {
                    result = line.Substring(leftPos, stringLength);
                }
            }
            return result;
        }

        private bool IsValidUrlSegment(string line)
        {
            //must contain both a ( and a ) char.
            if ((line.IndexOf("(") > 0) && (line.IndexOf(")") > 0))
                return true;
            return false;
        }

        private static FileInfo GetTestCssFile()
        {
            string cssFilePath = TestHelper.GetTestProjectDir().FullName + "\\TestFiles\\ManifestAssets\\Static\\css\\TestImageAssetsParsing.css";
            FileInfo cssFile = new FileInfo(cssFilePath);
            return cssFile;
        }

        private List<string> FindImageAssetsLinesInCssFile(FileInfo cssFile)
        {
            List<string> resultLines = new List<string>();

            // Open the stream and read it back. 
            using (StreamReader sr = File.OpenText(cssFile.FullName))
            {
                string line = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.ToLower().Contains("url"))
                    {
                        resultLines.Add(line);
                    }
                }
            }
            return resultLines;
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