using System;
using System.Linq;
using EPiPugPigConnector.Controllers;
using EPiPugPigConnector.Editions.Factories;
using EPiPugPigConnector.Editions.Models.Pages;
using EPiPugPigConnector.Editions.Models.Pages.Helpers;
using EPiPugPigConnector.Logging;
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
            get { return "application/atom+xml"; }
        }

        public string GetEditions(string language = null)
        {
            var containerPage = EditionsHelper.GetEditionsContainerPage();
            var editionEntries = EditionsHelper.GetEditionPages(containerPage);

            if (containerPage == null && !editionEntries.Any())
            {
                string errorMessage = "The editions container page or editions childpages was not found/not published.";
                LogHelper.Log(errorMessage);
                throw new NullReferenceException(errorMessage);
                return errorMessage;
            }

            return EditionsXmlFactory.GenerateXmlFrom(containerPage.ContentLink, containerPage, editionEntries);
        }

        public string GetEdition(string id)
        {
            var editionPage = EditionHelper.GetEditionPage(id);
            return EditionXmlFactory.GenerateXmlFrom(editionPage);
        }
    }
}
