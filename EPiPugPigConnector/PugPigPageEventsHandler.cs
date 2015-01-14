using System;
using System.Collections.Generic;
using System.Linq;
using EPiPugPigConnector.Editions;
using EPiPugPigConnector.Editions.Models.Pages;
using EPiPugPigConnector.EPiExtensions;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.ServiceLocation;

namespace EPiPugPigConnector
{
    public class PugPigPageEventsHandler
    {
        public PugPigPageEventsHandler()
        {

        }

        public void Instance_MovedPage(object sender, PageEventArgs e)
        {
            UpdatePugPigFeedsChangedDate(e.Page);
        }

        public void Instance_PublishedPage(object sender, PageEventArgs e)
        {
            UpdatePugPigFeedsChangedDate(e.Page);
        }

        public void Instance_DeletedPage(object sender, PageEventArgs e)
        {
            UpdatePugPigFeedsChangedDate(e.Page);
        }

        private void UpdatePugPigFeedsChangedDate(PageData page)
        {
            //if page is a valid pugpig page
            if (PageHelper.IsValidPugPigPage(page))
            {
                //get the editionpage and the editionspage
                var editionPage = PageHelper.GetAncestorEditionPage(page);
                var editionsContainerPage = PageHelper.GetAncestorEditionsContainerPage(page);

                //set their changedate to currentpage changed date.
                //set changed date on the current page as well since this is not standard episerver behaviour. -> will update the updated value in the xml feed with changed prop.

                var dateTimePageChanged = DateTime.Now;
                SetChangedDate(page, dateTimePageChanged);
                SetChangedDate(editionPage, dateTimePageChanged);
                SetChangedDate(editionsContainerPage, dateTimePageChanged);
            }
        }

        private void SetChangedDate(PageData page, DateTime changed)
        {
            if (page != null)
            {
                DisableSetChangedForEditionPageTypes(page);

                var clone = page.CreateWritableClone();
                clone.Property["PageChanged"].Value = changed; 
                DataFactory.Instance.Save(clone, SaveAction.Publish, AccessLevel.NoAccess);

                EnableSetChangedForEditionPageTypes(page);
            }
        }

        /// <summary>
        /// To avoid auto update of change value on the publish event.
        /// </summary>
        private static void DisableSetChangedForEditionPageTypes(PageData page)
        {
            if (page is EditionPage || page is EditionsContainerPage)
            {
                page.SetChangedOnPublish = false;
            }
        }

        private static void EnableSetChangedForEditionPageTypes(PageData page)
        {
            if (page is EditionPage || page is EditionsContainerPage)
            {
                page.SetChangedOnPublish = true;
            }
        }
    }
}