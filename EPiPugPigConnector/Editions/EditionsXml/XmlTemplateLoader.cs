//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Xml;
//using System.Xml.Linq;
//using System.Xml.Schema;
//using System.Xml.XPath;
//using EPiPugPigConnector.Editions.Interfaces.Editions;

//namespace EPiPugPigConnector.Editions.EditionsXml
//{
//    public class XmlTemplateLoader
//    {
//        public static XmlDocument GetEditionsRootTemplate()
//        {
//            //load xml editions template file
//            XmlDocument xmlDocument = new XmlDocument();
//            xmlDocument.LoadXml(Resources.Resources.editionsRootTemplateXml);
//            return xmlDocument;
//        }

//        public static XmlDocument GetEditionsEntryTemplate()
//        {
//            //load xml editions template file
//            XmlDocument xmlDocument = new XmlDocument();
//            xmlDocument.LoadXml(Resources.Resources.editionsEntryTemplateXml);
//            return xmlDocument;
//        }

//        public static XmlDocument PopulateXmlTemplateFrom(IEditionsXmlFeed editionsRoot, List<IEditionsXmlFeedEntry> editionEntries)
//        {
//            //get xml template
//            var rootTemplate = GetEditionsRootTemplate();
//            var entryTemplate = GetEditionsEntryTemplate();

//            //populate xml template
//            rootTemplate = PopulateRootFeedElement(editionsRoot, rootTemplate);

//            foreach (var editionEntry in editionEntries)
//            {
//                entryTemplate = PopulateEntryFeedElement(editionEntry, entryTemplate);
//                var entryNode = entryTemplate.GetElementsByTagName("entry").Item(0);
//                if (entryNode != null)
//                {
//                    var targetEntryElement = rootTemplate.CreateElement("entry", "http://www.w3.org/2005/Atom");
//                    targetEntryElement.InnerXml = entryNode.InnerXml;
//                    rootTemplate.DocumentElement.AppendChild(targetEntryElement);
//                }
//            }

//            //return populated xml template
//            return rootTemplate;
//        }

//        private static XmlDocument PopulateRootFeedElement(IEditionsXmlFeed rootData, XmlDocument rootTemplate)
//        {
//            SetAttributeValue(rootTemplate, "link", "href", rootData.FeedLinkHref);
//            SetElementValue(rootTemplate, "title", rootData.FeedTitle);
//            SetElementValue(rootTemplate, "updated", rootData.FeedUpdated);
//            SetElementValue(rootTemplate, "name", "AuthorName");

//            return rootTemplate;
//        }

//        private static XmlDocument PopulateEntryFeedElement(IEditionsXmlFeedEntry entryData, XmlDocument entryTemplate)
//        {
//            SetElementValue(entryTemplate, "title", entryData.EntryTitle);
//            SetElementValue(entryTemplate, "id", entryData.EntryId);
//            SetElementValue(entryTemplate, "updated", entryData.EntryUpdated);
//            SetElementValue(entryTemplate, "dcterms:issued", entryData.EntryDcTermsIssued);
//            SetElementValue(entryTemplate, "name", entryData.EntryAuthorName); 
//            SetElementValue(entryTemplate, "summary", entryData.EntrySummaryText);
//            SetAttributeValue(entryTemplate, "link", "href", entryData.EntryLinkCoverImage, itemIndex:0);
//            SetAttributeValue(entryTemplate, "link", "href", entryData.EntryLinkEditionXml, itemIndex:1);
//            SetAttributeValue(entryTemplate, "link", "href", entryData.EntryLinkEditionXml, itemIndex:2);

//            return entryTemplate;
//        }

//        /// <summary>
//        /// Sets innertext value for the first found descendant element matched on tagname.
//        /// </summary>
//        /// <param name="xmlDocument"></param>
//        /// <param name="tagName"></param>
//        /// <param name="value"></param>
//        private static void SetElementValue(XmlDocument xmlDocument, string tagName, string value)
//        {
//            var xmlNode = xmlDocument.GetElementsByTagName(tagName).Item(0);
//            if (xmlNode != null)
//                xmlNode.InnerText = value;
//        }

//        /// <summary>
//        /// Sets a attribute value for the first found/or given indexnumber, descendant element matched on tagname.
//        /// </summary>
//        /// <param name="rootTemplate"></param>
//        /// <param name="elementName"></param>
//        /// <param name="attributeName"></param>
//        /// <param name="value"></param>
//        /// <param name="itemIndex">Target a specific element index.</param>
//        private static void SetAttributeValue(XmlDocument rootTemplate, string elementName, string attributeName, string value, int itemIndex = 0)
//        {
//            var xmlNode = rootTemplate.GetElementsByTagName(elementName).Item(itemIndex);
//            if (xmlNode != null && xmlNode.Attributes != null && xmlNode.Attributes.GetNamedItem(attributeName) != null)
//            {
//                xmlNode.Attributes.GetNamedItem(attributeName).Value = value;
//            }
//        }
//    }
//}