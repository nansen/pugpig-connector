using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using EPiPugPigConnector.EPiExtensions;

namespace EPiPugPigConnector.Models.Pages
{
    public static class EditionsPageHelper
    {
        /// <summary>
        /// Should be ISO 8601 datetime http://tools.ietf.org/html/rfc3339
        /// Such as 2011-08-08T15:02:28+00:00
        /// </summary>
        public static string GetDateTimeXmlFormatted(DateTime dateTimeUpdated)
        {
            // from: https://pugpig.zendesk.com/hc/en-us/articles/200948376-Document-Discovery
            // from: http://stackoverflow.com/questions/6314154/generate-datetime-format-for-xml

            var dateTimeWithUtcOffset = new DateTime(
                dateTimeUpdated.Year,
                dateTimeUpdated.Month,
                dateTimeUpdated.Day,
                dateTimeUpdated.Hour,
                dateTimeUpdated.Minute,
                dateTimeUpdated.Second,
                DateTimeKind.Local); //adds the Z offset

            return dateTimeWithUtcOffset.ToString("O");
        }

        /// <summary>
        /// Returns e.g. "http://epipugpigdemo.azurewebsites.net/editions.xml"
        /// </summary>
        /// <param name="editionsXmlPage"></param>
        /// <returns></returns>
        public static string GetLinkHref(EditionsContainerPage editionsXmlPage)
        {
            return string.Format("{0}/{1}", GetCurrentDomainRootUrl(), Constants.OPDS_FEED_EDITIONS_XML_FILENAME);
        }

        public static string GetCurrentDomainRootUrl()
        {
            var request = HttpContext.Current.Request;

            string rootUrl = string.Format("{0}{1}{2}{3}",
                request.Url.Scheme,
                Uri.SchemeDelimiter,
                request.Url.Host,
                (request.Url.IsDefaultPort ? "" : ":" + request.Url.Port));

            return rootUrl;
        }

        /// <summary>
        /// com.mycompany.edition0123456789 - just a unique string of some kind.
        /// </summary>
        /// <param name="editionPage"></param>
        public static string GetEntryId(EditionPage editionPage)
        {
            if (editionPage != null)
            {
                return string.Format("EditionId-{0}", editionPage.PageLink.ID);
            }
            else
            {
                return string.Format("EditionId-{0}", Guid.NewGuid());
            }
        }

        public static string EntryDcTermsIssued(DateTime changeDateTime)
        {
            string formatString = "yyyy-MM-dd";
            string result = changeDateTime.ToString(formatString);
            return result;
        }

        /// <summary>
        /// The link to the Edition.xml file. e.g "edition1.xml"
        /// </summary>
        public static string GetEntryLinkEditionXml(EditionPage editionPage)
        {
            if (editionPage == null)
            {
                //used by test project
                return string.Format("edition-{0}.xml", Guid.NewGuid());
            }
            else
            {
                //todo: implement this based on how the edition xml file is created
                return string.Format("edition-{0}.xml", editionPage.PageLink.ID);
            }
        }

        public static EditionsContainerPage GetEditionsContainerPage()
        {
            var page = PageReference.RootPage.GetPage()
                            .GetDescendants<EditionsContainerPage>()
                            .FirstOrDefault(p => p.CheckPublishedStatus(PagePublishedStatus.Published));
            return page;
        }

        public static List<EditionPage> GetEditionPages(EditionsContainerPage containerPage)
        {
            return containerPage
                .GetDescendants<EditionPage>()
                .Where(p => p.CheckPublishedStatus(PagePublishedStatus.Published))
                .ToList();
        }
    }
}
