using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiPugPigConnector.Editions.Models.Pages;
using EPiPugPigConnector.EPiExtensions;
using EPiServer.Core;
using EPiServer.Filters;

namespace EPiPugPigConnector.Editions
{
    public static class PageHelper
    {
        /// <summary>
        /// Checks if the page is a valid page for visitors. The page should be published, not in trash, have a template, and accesslevel read for current user.
        /// </summary>
        public static bool IsPublishedAndAvailableForDisplay(PageData page, bool requirePageTemplate = true, bool requireVisibleInMenu = false)
        {
            bool isValid = false;
            
            var pageFakeList = new List<PageData>{ page };
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
            //if page is descendant of an edition page.
            IEnumerable<EditionPage> editionPageAncestors = page.ContentLink.GetAncestors<EditionPage>().ToList();
            if (editionPageAncestors.Any())
            {
                return editionPageAncestors.FirstOrDefault();
            }
            return null;
        }

        public static EditionsContainerPage GetAncestorEditionsContainerPage(PageData page)
        {
            if (page is EditionPage)
            {
                return page.ParentLink.Get<EditionsContainerPage>();
            }

            //if page is descendant of an editions page.
            IEnumerable<EditionsContainerPage> editionsPageAncestors = page.ContentLink.GetAncestors<EditionsContainerPage>().ToList();
            if (editionsPageAncestors.Any())
            {
                return editionsPageAncestors.FirstOrDefault();
            }
            return null;
        }
    }
}
