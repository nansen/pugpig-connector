using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using EPiPugPigConnector.Editions.EditionsXml;
using EPiPugPigConnector.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web.Routing;

namespace EPiPugPigConnector.Editions
{
    public class PageTreeEditionsGenerator : IEditionsGenerator
    {
        private readonly IContentLoader _contentLoader;
        private readonly ILanguageBranchRepository _languageBranchRepository;
        private readonly UrlResolver _urlResolver;

        public PageTreeEditionsGenerator(IContentLoader contentLoader, UrlResolver urlResolver,
            ILanguageBranchRepository languageBranchRepository)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _languageBranchRepository = languageBranchRepository;
        }

        public string ContentType
        {
            get { return "text/xml"; }
        }

        public string GetEditions(string language = null)
        {
            var containerPage = EditionsPageHelper.GetEditionsContainerPage();
            var editionEntries = EditionsPageHelper.GetEditionPages(containerPage);

            if (containerPage == null && !editionEntries.Any())
            {
                throw new NullReferenceException("The editions container page or editions childpages was not found/not published.");
            }
            
            var xmlString = XmlFactory.GenerateEditionsXmlFrom(containerPage, editionEntries);
            return xmlString;
        }

        public string GetEdition(string id)
        {
            var filePath = HttpContext.Current.Server.MapPath("~\\App_Data\\FakeEdition.xml");
            var sb = LoadXmlFile(filePath);

            return sb.ToString(); ;
        }

        private static StringBuilder LoadXmlFile(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(filePath))
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            return sb;
        }
    }
}
