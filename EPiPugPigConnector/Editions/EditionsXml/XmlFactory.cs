using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Models.Pages;

namespace EPiPugPigConnector.Editions.EditionsXml
{
    public class XmlFactory
    {
        private static XNamespace _atomNS = "http://www.w3.org/2005/Atom";

        public static XDocument GenerateEditionsXmlFrom(IEditionsXmlFeedRoot rootData, IEnumerable<IEditionsXmlFeedEntry> entries)
        {
            //create the xml
            XDocument rootDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            AddFeedXml(rootData, rootDocument);
            AddEntriesXml(entries, rootDocument);
            return rootDocument;
        }

        private static void AddFeedXml(IEditionsXmlFeedRoot rootData, XDocument rootDocument)
        {
            var feedElementXml = CreateFeedXmlFrom(rootData);
            rootDocument.Add(feedElementXml);
        }

        private static void AddEntriesXml(IEnumerable<IEditionsXmlFeedEntry> entries, XDocument rootDocument)
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

        private static XElement CreateEntryXmlFrom(IEditionsXmlFeedEntry entryData)
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
                GetEntryLinkElement("http://opds-spec.org/image", "image/jpg", entryData.EntryLinkCoverImage),
                GetEntryLinkElement("http://opds-spec.org/acquisition", "application/atom+xml", entryData.EntryLinkEditionXml),
                GetEntryLinkElement("alternate", "application/atom+xml", entryData.EntryLinkEditionXml)
            );
            return entryElement;
        }

        private static XElement GetEntryLinkElement(string relValue, string typeValue, string hrefValue)
        {
            return new XElement(_atomNS + "link",
                new XAttribute("rel", relValue),
                new XAttribute("type", typeValue),
                new XAttribute("href", hrefValue)
                );
        }

        private static XElement CreateFeedXmlFrom(IEditionsXmlFeedRoot rootData)
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

        private static object GetEditionsLinkElement(string feedLinkHref)
        {
            var link = new XElement(_atomNS + "link");
                link.SetAttributeValue("rel", "self");
                link.SetAttributeValue("type", "application/atom+xml;profile=opds-catalog;kind=acquisition");
                link.SetAttributeValue("href", feedLinkHref);
            return link;
        }
    }
}