using System;
using System.Collections.Generic;
using System.IO;
using Castle.Core.Internal;
using EPiPugPigConnector.ExtensionMethods;
using EPiPugPigConnector.WebClients;

namespace EPiPugPigConnector.HttpModules.Manifest
{
    /// <summary>
    /// Scans css files for assests and adds them to a manifest file
    /// </summary>
    public class CssAssetProcessor
    {
        private readonly string _manifestFilePath;
        private readonly IWebClient _webClient;
        private readonly string _cssVirtualPath;

        public CssAssetProcessor(IWebClient webClient, string manifestVirtualPath, string cssVirtualPath)
        {
            _manifestFilePath = manifestVirtualPath;
            _cssVirtualPath = cssVirtualPath;
            _webClient = webClient;
        }

        public List<string> ProcessCssFile()
        {
            List<string> linesToParse = new List<string>();
            List<string> parsedLines = new List<string>();

            linesToParse = FindImageAssetsLinesInCssFile(_webClient, _cssVirtualPath);
            parsedLines = ConvertImageAssetLinesToRelativeUrls(linesToParse, _cssVirtualPath, _manifestFilePath);

            return parsedLines;
        }

        public static List<string> FindImageAssetsLinesInCssFile(IWebClient webClient, string cssVirtualPath)
        {
            List<string> resultLines = new List<string>();
            List<string> allLines = new List<string>();

            var cssFileByteArray = webClient.DownloadData(new Uri(cssVirtualPath));
            var stream = new MemoryStream(cssFileByteArray);

            // Open the stream and read it back. 
            using (StreamReader sr = new StreamReader(stream))
            {
                string line = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] stringSeparators = new string[] { "url" };
                    allLines.AddRange(line.Split(stringSeparators, StringSplitOptions.None));
                }
            }

            foreach (var line in allLines)
            {
                if (line.ToLower().Contains(")"))
                {
                    try
                    {
                        var url = line.Remove(line.IndexOf(')') + 1, line.Length - line.IndexOf(')') - 1);
                        //url = url.Remove(url.IndexOf(")"), url.IndexOf(")") +1 -);
                        resultLines.Add(url);
                    }
                    catch (Exception)
                    {
                        // Not an asset url so continue to the next line.
                        continue;
                    }
                }
            }

            return resultLines;
        }

        private List<string> ConvertImageAssetLinesToRelativeUrls(List<string> linesToParse, string cssFile, string manifestFilePath)
        {
            var resultList = new List<string>();
            foreach (var line in linesToParse)
            {
                var resultString = ParseUrlSegmentFrom(line);
                if (resultString.IsNullOrEmpty())
                    continue;

                resultString = RemoveUnwantedCharacters(resultString);
                //resultString = ConvertCssRelativeToManifestRelativeString(resultString, cssFile, manifestFilePath);

                if (resultString.IsNotNullOrEmpty())
                {
                    resultList.Add(resultString);
                }
            }
            return resultList;
        }

        private string ConvertCssRelativeToManifestRelativeString(string resultString, string cssFile, string manifestFilePath)
        {
            var baseUri = new Uri(manifestFilePath);
            var cssFilePath = cssFile.Remove(cssFile.LastIndexOf('/'));
            cssFilePath = string.Format("{0}/", cssFilePath);

            var assetUri = new Uri(string.Format("{0}{1}", cssFilePath, resultString));
            var relativeUrl = baseUri.MakeRelativeUri(assetUri);

            return relativeUrl.ToString();
        }

        private string RemoveUnwantedCharacters(string originalString)
        {

            //todo: remove ' " and such chars from string
            var filteredString = originalString;
            char[] invalidChars = { '\'', '\"' };

            foreach (var invalidChar in invalidChars)
            {
                filteredString = filteredString.Replace(invalidChar.ToString(), "");
            }

            return filteredString;
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

                if (stringLength > 0 && (line.Length >= leftPos + stringLength))
                {
                    result = line.Substring(leftPos, stringLength);
                }
            }
            return result;
        }

        private bool IsValidUrlSegment(string line)
        {
            //must contain both a ( and a ) char.
            if ((line.IndexOf("(") == 0) && (line.IndexOf(")") > 0))
                return true;
            return false;
        }
    }
}
