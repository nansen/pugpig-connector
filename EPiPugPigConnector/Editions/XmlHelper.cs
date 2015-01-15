using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using EPiPugPigConnector.Utils;

namespace EPiPugPigConnector.Editions
{
    public static class XmlHelper
    {
        public static XElement GetLinkElement(string relValue, string typeValue, string hrefValue)
        {
            var ns = XNamespace.Get(Constants.ATOM_XML_NAMESPACE);

            return new XElement(ns+"link",
                new XAttribute("rel", relValue),
                new XAttribute("type", typeValue),
                new XAttribute("href", hrefValue)
                );
        }

        public static string ForceXmlToUtf8Output(XDocument xDocument)
        {
            //to avoid problems with xml output string is utf16 and formatting fixes:
            //http://www.undermyhat.org/blog/2009/08/tip-force-utf8-or-other-encoding-for-xmlwriter-with-stringbuilder/
            //http://stackoverflow.com/questions/3871738/force-xdocument-to-write-to-string-with-utf-8-encoding
            //https://social.msdn.microsoft.com/Forums/vstudio/en-US/96616c0d-620d-4023-b6be-09351081b3c6/linq-to-xml-xdeclaration-missing-from-output?forum=netfxbcl

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                Indent = true
            };

            XmlWriter xmlWriter = null;
            MemoryStream memoryStream = null;
            string xmlResultString = null;

            try
            {
                memoryStream = new MemoryStream();
                xmlWriter = XmlWriter.Create(memoryStream, settings);
                xDocument.Save(xmlWriter); //persist XDocument to xmlwriter
                xmlWriter.Flush();
                memoryStream.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);
                StreamReader streamReader = new StreamReader(memoryStream);
                xmlResultString = streamReader.ReadToEnd();
            }
            finally
            {
                xmlWriter.Close();
                memoryStream.Close();
            }

            return xmlResultString;
        }

        /// <summary>
        /// Should be ISO 8601 datetime http://tools.ietf.org/html/rfc3339
        /// Such as 2011-08-08T15:02:28+00:00
        /// </summary>
        public static string GetDateTimeXmlFormatted(DateTime dateTime)
        {
            // from: https://pugpig.zendesk.com/hc/en-us/articles/200948376-Document-Discovery
            // from: http://stackoverflow.com/questions/6314154/generate-datetime-format-for-xml

            var dateTimeWithUtcOffset = new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                DateTimeKind.Local); //adds the Z offset

            return dateTimeWithUtcOffset.ToString("O");
        }

        public static XNamespace GetAtomXmlNameSpace()
        {
            return XNamespace.Get(Constants.ATOM_XML_NAMESPACE);
        }

        public static void AddElapsedTimeComment(StopwatchTimer stopwatch, XDocument rootDocument)
        {
            string elapsedTime = stopwatch.Stop();
            var generatedAtComment = new XComment(String.Format("XML GENERATED AT: {0}", DateTime.Now.ToString("o")));
            var elapsedTimeComment = new XComment(String.Format("XML GENERATED IN: {0}", elapsedTime));
            rootDocument.AddFirst(elapsedTimeComment);
            rootDocument.AddFirst(generatedAtComment);
        }
    }
}
