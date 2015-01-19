using System;
using System.Collections.Generic;
using System.Linq;
using EPiPugPigConnector.EPiExtensions;
using EPiPugPigConnector.Helpers;
using EPiServer.Core;

namespace EPiPugPigConnector.Editions.Models.Pages.Helpers
{
    public static class EditionsHelper
    {
        /// <summary>
        /// Returns e.g. "http://epipugpigdemo.azurewebsites.net/editions.xml"
        /// </summary>
        public static string GetLinkHref()
        {
            return string.Format("{0}/{1}", UrlHelper.GetCurrentDomainRootUrl(), Constants.OPDS_FEED_EDITIONS_XML_FILENAME);
        }

        /// <summary>
        /// com.mycompany.edition0123456789 - just a unique string of some kind.
        /// </summary>
        /// <param name="editionPage"></param>
        public static string GetEntryId(EditionPage editionPage)
        {
            if (editionPage != null)
            {
                return string.Format("{0}{1}", Constants.XML_EDITION_ID_PREFIX, editionPage.PageLink.ID);
            }
            else
            {
                return string.Format("{0}{1}", Constants.XML_EDITION_ID_PREFIX, Guid.NewGuid());
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
                return string.Format("edition{0}.xml", Guid.NewGuid());
            }
            else
            {
                //todo: implement this based on how the edition xml file is created
                return string.Format("edition{0}.xml", editionPage.PageLink.ID);
            }
        }

        public static EditionsContainerPage GetEditionsContainerPage()
        {
            var page = PageReference.RootPage.GetPage()
                            .GetDescendants<EditionsContainerPage>()
                            .FirstOrDefault(p => p.CheckPublishedStatus(PagePublishedStatus.Published) && !p.IsDeleted);
            return page;
        }

        public static List<EditionPage> GetEditionPages(EditionsContainerPage containerPage)
        {
            return containerPage
                .GetDescendants<EditionPage>()
                .Where(p => p.CheckPublishedStatus(PagePublishedStatus.Published) && !p.IsDeleted)
                .ToList();
        }
    }
}
