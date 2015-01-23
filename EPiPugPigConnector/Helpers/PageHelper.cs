using System;
using System.Collections.Generic;
using System.Linq;
using EPiPugPigConnector.Editions.Models.Pages;
using EPiPugPigConnector.EPiExtensions;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;

namespace EPiPugPigConnector.Helpers
{
    public static class PageHelper
    {
        /// <summary>
        /// Checks if the page is a valid page for visitors. The page should be published, not in trash, have a template, and accesslevel read for current user.
        /// </summary>
        public static bool IsPublishedAndAvailableForDisplay(PageData page, bool requirePageTemplate = true, bool requireVisibleInMenu = false)
        {
            bool isValid = false;
            
            var pageFakeList = new List<PageData> { page };
            //FilterForDisplay also makes sure the current user has read access as well.
            pageFakeList.FilterForDisplay(requirePageTemplate, requireVisibleInMenu);
            var validPage = pageFakeList.ToList().FirstOrDefault();

            if (validPage != null)
            {
                isValid = !validPage.IsDeleted;
            }
            return isValid;
        }

        /// <summary>
        /// If page is: published, descendant of an edition page and edition page is published as well.
        /// </summary>
        public static bool IsValidPugPigPage(PageData page)
        {

            if (!IsPublishedAndAvailableForDisplay(page))
            {
                return false;
            }

            //if page is a editionpage and descandant of an editions container page.
            if (page is EditionPage)
            {
                if (IsPublishedAndAvailableForDisplay(page.GetParent()))
                {
                    return true;
                }
            }

            //if page is descendant of an edition page.
            var ancestorEditionPage = GetAncestorEditionPage(page);
            if (ancestorEditionPage != null)
            {
                // if the edition page is published and valid
                return IsPublishedAndAvailableForDisplay(ancestorEditionPage);
            }
            return false;
        }

        public static EditionPage GetAncestorEditionPage(PageData page)
        {
            if (page is EditionPage)
            {
                return page as EditionPage;
            }

            //if page is descendant of an edition page.
            if (page != null && page.ContentLink != null)
            {
                IEnumerable<EditionPage> editionPageAncestors = page.ContentLink.GetAncestors<EditionPage>().ToList();
                if (editionPageAncestors.Any())
                {
                    return editionPageAncestors.FirstOrDefault();
                }
            }
            return null;
        }

        public static EditionsContainerPage GetAncestorEditionsContainerPage(PageData page)
        {
            if (page is EditionsContainerPage)
            {
                return page as EditionsContainerPage;
            }

            if (page is EditionPage)
            {
                var parentPage = page.ParentLink.Get<EditionsContainerPage>();
                if(parentPage != null)
                    return parentPage;
            }

            //if page is descendant of an editions page.
            if (page != null && page.ContentLink != null)
            {
                IEnumerable<EditionsContainerPage> editionsPageAncestors = page.ContentLink.GetAncestors<EditionsContainerPage>().ToList();
                if (editionsPageAncestors.Any())
                {
                    return editionsPageAncestors.FirstOrDefault();
                }
            }
            return null;
        }

        public static bool IsEditionsContainerOrEditionPage(PageData page)
        {
            return (page is EditionPage || page is EditionsContainerPage);
        }

        public static string GetFriendlyUrlWithExtension(PageData page, string extension, bool includeHost = true)
        {
            string friendlyUrl = page.GetFriendlyUrl(includeHost);
            return HtmlHelper.FriendlyUrlToUrlWithExtension(friendlyUrl, extension);
        }

        public static PageReference GetPageReferenceFromExternalUrl(Uri uri)
        {
            //var x = new UrlBuilder(uri);
            //var contentReference = UrlResolver.Current.Route(new UrlBuilder(x.Path));

            //url must be a relative url. e.g. starts with "/"
            var contentReference = UrlResolver.Current.Route(new UrlBuilder(uri.PathAndQuery));
            return contentReference.ContentLink.ToPageReference();
        }

        public static PageReference GetPageReferenceFromExternalUrl(string sourceUrl)
        {
            return GetPageReferenceFromExternalUrl(new Uri(sourceUrl));
        }
    }
}
