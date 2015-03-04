using System;
using System.Collections.Generic;
using System.IO;
using Castle.Core.Internal;
using CsQuery;
using EPiPugPigConnector.ExtensionMethods;
using EPiPugPigConnector.Helpers;
using EPiPugPigConnector.HtmlCrawlers.Manifest.Models;
using EPiPugPigConnector.WebClients;

namespace EPiPugPigConnector.HtmlCrawlers.Manifest.Crawlers
{
    /// <summary>
    /// Decorates the RelativeUrlCrawler.
    /// Gets Assets from within css files listed in the provided HTML string
    /// </summary>
    public class CssCrawler : RelativeUrlCrawler
    {
        private readonly Uri _manifestUri;
        private readonly IWebClient _webClient;
        
        public CssCrawler(IWebClient webClient, Uri manifestUri)
            : base(manifestUri, "link", "href")
        {
            _manifestUri = manifestUri;
            _webClient = webClient;
        }

        public override List<Asset> Crawl(CQ htmlDocument)
        {
            var cssFiles = base.GetUrlsFrom(htmlDocument);
            var imageAssets = new List<Asset>();

            foreach (var cssFile in cssFiles)
            {
                var abslouteCssUrl = UrlHelper.GetAbslouteUrl(cssFile);
                var linesToParse = FindImageAssetsLinesInCssFile(abslouteCssUrl);
                imageAssets = ConvertImageAssetLinesToRelativeUrls(linesToParse, abslouteCssUrl, "img");
            }

            return imageAssets;
        }

        private List<string> FindImageAssetsLinesInCssFile(string cssFileUrl)
        {
            List<string> resultLines = new List<string>();
            List<string> allLines = new List<string>();
            
            var cssFileByteArray = _webClient.DownloadData(new Uri(cssFileUrl));
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

        private List<Asset> ConvertImageAssetLinesToRelativeUrls(List<string> linesToParse, string cssFileUrl, string element)
        {
            var resultList = new List<Asset>();
            foreach (var line in linesToParse)
            {
                var resultString = ParseUrlSegmentFrom(line);
                if (resultString.IsNullOrEmpty())
                    continue;

                resultString = RemoveUnwantedCharacters(resultString);
                resultString = ConvertCssRelativeToManifestRelativeString(resultString, cssFileUrl);

                if (resultString.IsNotNullOrEmpty())
                {
                    resultList.Add(
                        new Asset
                        {
                            Url = resultString,
                            Element = element
                        }
                    );
                }
            }

            return resultList;
        }

        private string ConvertCssRelativeToManifestRelativeString(string resultString, string cssFile)
        {
            var cssFileUrl = cssFile.Remove(cssFile.LastIndexOf('/'));
            cssFileUrl = string.Format("{0}/", cssFileUrl);

            var assetUri = new Uri(string.Format("{0}{1}", cssFileUrl, resultString));
            var relativeUrl = UrlHelper.GetRelativeUrl(_manifestUri.ToString(), assetUri.ToString());

            return relativeUrl;
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
