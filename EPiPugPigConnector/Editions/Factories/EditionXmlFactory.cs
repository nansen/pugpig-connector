using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EPiPugPigConnector.Caching;
using EPiPugPigConnector.Editions.Interfaces.Edition;
using EPiPugPigConnector.Editions.Models.Pages;
using EPiPugPigConnector.EPiExtensions;
using EPiPugPigConnector.Helpers;
using EPiPugPigConnector.Utils;
using EPiServer.Core;
using Microsoft.SqlServer.Server;

namespace EPiPugPigConnector.Editions.Factories
{
    public static class EditionXmlFactory
    {
        /// <summary>
        /// Generates a Pugpig edition.xml file.
        /// Iterate the EPiServer childpages of the edition page and creates a xml atom feed.
        /// Step 2 of 4 to create a working connector:
        /// https://pugpig.zendesk.com/hc/en-us/articles/201079186-How-To-Write-A-Connector
        /// </summary>
        public static string GenerateXmlFrom(EditionPage editionPage)
        {
            var cacheType = PugPigCacheType.Feed;
            string cacheKey = editionPage.ContentLink.ID.ToString();

            if (PugPigCache.IsSet(cacheType, cacheKey))
            {
                return PugPigCache.Get(cacheType, cacheKey) as string;
            }
            else
            {
                string resultXml = GenerateRootAndFeedXml(editionPage);
                PugPigCache.Set(cacheType, cacheKey, resultXml);
                return resultXml;
            }
        }

        private static string GenerateRootAndFeedXml(IEditionFeedElement editionFeedData)
        {
            StopwatchTimer stopwatch = new StopwatchTimer();

            var rootDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var feedElement = CreateFeedXmlFrom(editionFeedData);
            var entryElements = CreateEntriesXmlFrom(editionFeedData);

            feedElement.Add(entryElements);
            rootDocument.Add(feedElement); //adds the main feed with entries.
            XmlHelper.AddElapsedTimeComment(stopwatch, rootDocument);

            var resultXml = XmlHelper.ForceXmlToUtf8Output(rootDocument);
            
            return resultXml;
        }

        /// <summary>
        /// Creates the main "feed xml element 
        /// </summary>
        private static XElement CreateFeedXmlFrom(IEditionFeedElement rootData)
        {
            var ns = XmlHelper.GetAtomXmlNameSpace();
            var feedElement = new XElement(ns + "feed");

            var id = new XElement(ns + "id", rootData.FeedId);
            var link = XmlHelper.GetLinkElement(relValue: "self", typeValue: "application/atom+xml", hrefValue: rootData.FeedLink);
            var title = new XElement(ns + "title", rootData.FeedTitle, new XAttribute("type", "text"));
            var subtitle = new XElement(ns + "subtitle", rootData.FeedSubtitle, new XAttribute("type", "text"));
            var author = new XElement(ns + "author",
                new XElement(ns + "name", rootData.FeedAuthorName));
            var updated = new XElement(ns + "updated", rootData.FeedUpdated);

            feedElement.Add(id, link, title, subtitle, author, updated);
            return feedElement;
        }

        /// <summary>
        /// Creates the "entry" xml elements.
        /// </summary>
        private static List<XElement> CreateEntriesXmlFrom(IEditionFeedElement editionFeedData)
        {
            //Get collection of all viewable published pages below the edition root.
            IEnumerable<PageData> availablePages = GetAvailableDescendantPages(editionFeedData.FeedId);
            List<XElement> resultXmlEntries = new List<XElement>();

            //Iterate all pages and create xml elements foreach page.
            foreach (var availablePage in availablePages)
            {
                IEditionEntryElement entryData = ParseAsEditionEntryData(availablePage);
                XElement entryElement = CreateEntryXmlFrom(entryData);
                resultXmlEntries.Add(entryElement);
            }

            return resultXmlEntries;
        }

        private static XElement CreateEntryXmlFrom(IEditionEntryElement entryData)
        {
            var ns = XmlHelper.GetAtomXmlNameSpace();
            var entryElement = new XElement(ns + "entry");

            var id = new XElement(ns + "id", entryData.EntryId);
            var alternateLink = XmlHelper.GetLinkElement(relValue: "alternate", typeValue: "text/html", hrefValue: entryData.EntryHtmlLink);
            var relatedLink = XmlHelper.GetLinkElement(relValue: "related", typeValue: "text/cache-manifest", hrefValue: entryData.EntryManifestLink);
            var title = new XElement(ns + "title", entryData.EntryTitle, new XAttribute("type", "text"));
            var summary = new XElement(ns + "summary", entryData.EntrySummary, new XAttribute("type", "text"));
            var updated = new XElement(ns + "updated", entryData.EntryUpdated);

            entryElement.Add(id, alternateLink, relatedLink, title, summary, updated);
            return entryElement;
        }

        private static EditionEntryXmlElement ParseAsEditionEntryData(PageData page)
        {
            //todo: all pages are treated as PageData so we use PageName for title.
            // perhaps someone wants to change this to another property e.g. "PageHeading"
            // future function: this property and probably some other props connected to 
            // the opds xml feeds should be changable.
            string entryTitleValue = page.PageName;
            string entrySummarTextValue = EPiServer.Core.Html.TextIndexer.StripHtml(page.GetPropertyValue("MainBody"), maxTextLengthToReturn: 300);

            var data = new EditionEntryXmlElement
            {
                EntryId = "PageEntryId-" + page.ContentLink.ID,
                EntryHtmlLink = PageHelper.GetFriendlyUrlWithExtension(page, "html", includeHost: true), //must be absolute url e.g. http://editions/edition1/start.html
                EntryManifestLink = PageHelper.GetFriendlyUrlWithExtension(page, "manifest", includeHost: true), //must be absolute url
                EntryTitle = entryTitleValue,
                EntrySummary = entrySummarTextValue,
                EntryUpdated = XmlHelper.GetDateTimeXmlFormatted(page.Changed)
            };

            return data;
        }

        private static IEnumerable<PageData> GetAvailableDescendantPages(string feedId)
        {
            var page = GetPageFrom(feedId);

            if (page != null)
            {
                var pages = page.GetDescendants()
                                .FilterForDisplay(requirePageTemplate: true, requireVisibleInMenu: false)
                                .Where(p => !p.IsDeleted);
                return pages;
            }
            return null;
        }

        private static PageData GetPageFrom(string pageId)
        {
            pageId = pageId.Replace(Constants.XML_EDITION_ID_PREFIX, string.Empty);
            var page = PageReference.Parse(pageId).Get<PageData>();
            return page;
        }
    }

    public class EditionEntryXmlElement : IEditionEntryElement
    {
        public string EntryId { get; set; }
        public string EntryHtmlLink { get; set; }
        public string EntryManifestLink { get; set; }
        public string EntryTitle { get; set; }
        public string EntrySummary { get; set; }
        public string EntryUpdated { get; set; }
    }
}
