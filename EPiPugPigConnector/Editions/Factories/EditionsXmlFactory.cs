using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Helpers;
using EPiPugPigConnector.Utils;
using EPiServer;
using EPiPugPigConnector.EPiExtensions;
using EPiServer.Web.Routing;

namespace EPiPugPigConnector.Editions.Factories
{
    public class EditionsXmlFactory
    {
        private static XNamespace _atomNS = Constants.ATOM_XML_NAMESPACE;

        /// <summary>
        /// Generates the Pugpig main editions.xml file.
        /// Step 1 of 4 to create a working connector:
        /// https://pugpig.zendesk.com/hc/en-us/articles/201079186-How-To-Write-A-Connector
        /// </summary>
        public static string GenerateXmlFrom(IEditionsFeedElement editionsFeedData, IEnumerable<IEditionsEntryElement> editionEntriesData, bool includeGeneratedTimeComment = true)
        {
            //TODO: Object cache the xml creation here. 

            //create the xml
            var stopwatch = new StopwatchTimer();

            XDocument rootDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            AddFeedXml(editionsFeedData, rootDocument);
            AddEntriesXml(editionEntriesData, rootDocument);

            if (includeGeneratedTimeComment)
            {
                XmlHelper.AddElapsedTimeComment(stopwatch, rootDocument);
            }

            string resultXml = XmlHelper.ForceXmlToUtf8Output(rootDocument);
            return resultXml;
        }

        private static void AddFeedXml(IEditionsFeedElement rootData, XDocument rootDocument)
        {
            var feedElementXml = CreateFeedXmlFrom(rootData);
            rootDocument.Add(feedElementXml);
        }

        private static void AddEntriesXml(IEnumerable<IEditionsEntryElement> entries, XDocument rootDocument)
        {
            if (rootDocument.Root != null)
            {
                foreach (var feedEntry in entries)
                {
                    XElement entryElement = CreateEntryXmlFrom(feedEntry);
                    rootDocument.Root.Add(entryElement); // add entry xml.
                }
            }
        }

        private static XElement CreateEntryXmlFrom(IEditionsEntryElement entryData)
        {
            XNamespace dcTermsNamespace = "http://purl.org/dc/terms/";
            XElement entryElement = new XElement(_atomNS + "entry",
                    new XAttribute(XNamespace.Xmlns + "dcterms", dcTermsNamespace)
                );

            entryElement.Add(
                new XElement(_atomNS + "title", entryData.EntryTitle),
                new XElement(_atomNS + "id", entryData.EntryId),
                new XElement(_atomNS + "updated", entryData.EntryUpdated),
                new XElement(_atomNS + "author",
                    new XElement(_atomNS + "name", entryData.EntryAuthorName)
                    ),
                new XElement(dcTermsNamespace + "issued", entryData.EntryDcTermsIssued),
                new XElement(_atomNS + "summary", 
                    new XAttribute("type", "text"),
                    entryData.EntrySummaryText),
                XmlHelper.GetLinkElement("http://opds-spec.org/image", "image/jpg", GetContentExternalUrl(entryData.EntryLinkCoverImage)),
                XmlHelper.GetLinkElement("http://opds-spec.org/acquisition", "application/atom+xml", entryData.EntryLinkEditionXml),
                XmlHelper.GetLinkElement("alternate", "application/atom+xml", entryData.EntryLinkEditionXml)
            );
            return entryElement;
        }

        private static string GetContentExternalUrl(Url url)
        {
            string externalUrl = string.Empty;

            if (url != null && !url.IsEmpty())
            {
                var urlResolver = new UrlResolver();
                externalUrl = urlResolver.GetUrl(url.ToString());
            }
            return externalUrl;
        }

        private static XElement CreateFeedXmlFrom(IEditionsFeedElement rootData)
        {
            XElement feedElement = new XElement(_atomNS + "feed");
            feedElement.Add(new XAttribute(XNamespace.Xmlns + "dcterms", "http://purl.org/dc/terms/"));
            feedElement.Add(new XAttribute(XNamespace.Xmlns + "opds", "http://opds-spec.org/2010/catalog"));
            feedElement.Add(new XAttribute(XNamespace.Xmlns + "app", "http://www.w3.org/2007/app"));
            feedElement.Add(
                new XElement(_atomNS + "id", rootData.FeedId),
                GetEditionsLinkElement(rootData.FeedLinkHref),
                new XElement(_atomNS + "title", rootData.FeedTitle),
                new XElement(_atomNS + "updated", rootData.FeedUpdated)
                );
            return feedElement;
        }

        private static XElement GetEditionsLinkElement(string feedLinkHref)
        {
            //http://stackoverflow.com/questions/14840723/how-to-specify-an-xmlns-for-xdocument
            var ns = XNamespace.Get(Constants.ATOM_XML_NAMESPACE);

            var link = new XElement(ns + "link");
            link.SetAttributeValue("rel", "self");
            link.SetAttributeValue("type", "application/atom+xml;profile=opds-catalog;kind=acquisition");
            link.SetAttributeValue("href", feedLinkHref);
            return link;
        }
    }
}