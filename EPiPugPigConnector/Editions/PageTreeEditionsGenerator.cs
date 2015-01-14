using System;
using System.Linq;
using EPiPugPigConnector.Editions.Factories;
using EPiPugPigConnector.Editions.Models.Pages;
using EPiPugPigConnector.Editions.Models.Pages.Helpers;
using EPiServer;
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
            var containerPage = EditionsHelper.GetEditionsContainerPage();
            var editionEntries = EditionsHelper.GetEditionPages(containerPage);

            if (containerPage == null && !editionEntries.Any())
            {
                throw new NullReferenceException("The editions container page or editions childpages was not found/not published.");
            }
            
            return EditionsXmlFactory.GenerateXmlFrom(containerPage, editionEntries);
        }

        public string GetEdition(string id)
        {
            var editionPage = EditionHelper.GetEditionPage(id);
            return EditionXmlFactory.GenerateXmlFrom(editionPage);
        }

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
