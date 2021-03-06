using System;
using EPiPugPigConnector.Caching;
using EPiPugPigConnector.Editions.Models.Pages;
using EPiPugPigConnector.EPiExtensions;
using EPiPugPigConnector.Helpers;
using EPiPugPigConnector.Logging;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;

namespace EPiPugPigConnector.PageEvents
{
    public class PugPigPageEventsHandler
    {
        public void Instance_PublishingPage(object sender, PageEventArgs e)
        {
            //Occurs before PublishedPage event. "page is about the be published".
            UpdateSubPageChanged(e.Page);
        }

        public void Instance_PublishedPage(object sender, PageEventArgs e)
        {
            //Occurs after page is published.
            UpdateFeedsChanged(e.Page);
        }

        public void Instance_MovingPage(object sender, PageEventArgs e)
        {
            var moveArgs = e as MovePageEventArgs;

            if (moveArgs != null)
            {
                var changedDate = DateTime.Now;

                //update this page change date.
                SetChangedDate(e.Page, changedDate);

                //get original source pugpigsfeeds.
                var sourceOriginalParent = moveArgs.OriginalParent.GetPage();
                var sourceEditionPage = PageHelper.GetAncestorEditionPage(sourceOriginalParent);
                var sourceEditionsContainerPage = PageHelper.GetAncestorEditionsContainerPage(sourceEditionPage);
                //update changedates
                SetChangedDate(sourceEditionPage, changedDate);
                SetChangedDate(sourceEditionsContainerPage, changedDate);

                //get target pugpigsfeeds if different.
                var targetEditionPage = PageHelper.GetAncestorEditionPage(moveArgs.TargetLink.GetPage());
                var targetEditionsContainerPage = PageHelper.GetAncestorEditionsContainerPage(moveArgs.TargetLink.GetPage());

                if (targetEditionPage != null && sourceEditionPage != null)
                {
                    if (!targetEditionPage.PageLink.CompareToIgnoreWorkID(sourceEditionPage.PageLink))
                    {
                        //update changedates
                        SetChangedDate(targetEditionPage, changedDate);
                        SetChangedDate(targetEditionsContainerPage, changedDate);
                    }
                }
                
                if (moveArgs.TargetLink.ID == PageReference.WasteBasket.ID)
                {
                    //page is being deleted/moved to the WasteBasket
                }
            }
        }
        
        public void Instance_DeletedPage(object sender, PageEventArgs e)
        {
            //Already handled in MovingPage event, a MovingPage event is triggered before DeletedPage event.
        }

        /// <summary>
        /// Updates change date for parental Edition and Editioncontainer.
        /// </summary>
        private void UpdateFeedsChanged(PageData page)
        {
            //If sending page is a valid subpage part of an Pugpig edition.
            if (!PageHelper.IsEditionsContainerOrEditionPage(page) && PageHelper.IsValidPugPigPage(page))
            {
                //get the editionpage and the editionspage
                var editionPage = PageHelper.GetAncestorEditionPage(page);
                var editionsContainerPage = PageHelper.GetAncestorEditionsContainerPage(page);

                //set their changedate to currentpage changed date.
                var dateTimePageChanged = page.Changed;
                SetChangedDate(editionPage, dateTimePageChanged);
                SetChangedDate(editionsContainerPage, dateTimePageChanged);
            }

            //Special case: if Edition page, update parent EditionsContainerPage
            if (page is EditionPage)
            {
                //Set changed date on parent EditionsContainerPage
                var editionsContainerPage = PageHelper.GetAncestorEditionsContainerPage(page);
                SetChangedDate(editionsContainerPage, page.Changed);
            }
        }

        private void UpdateSubPageChanged(PageData page)
        {
            //Special behaviour for subpage to avoid a eternal published event loop if SetChangedOnPublish is not true
            //(edition feed pages has this set to true default).

            if (!PageHelper.IsEditionsContainerOrEditionPage(page))
            {
                //Set changed date on the current page since this is not standard EPiServer behaviour. -> used in <updated> in the xml feed
                page.SetChangedOnPublish = true; //http://dodavinkeln.se/post/how-to-set-a-page-updated-programmatically
                page.Changed = DateTime.Now;
                
                LogPageChanged(page);

                //remove page manifest cache:
                PugPigCache.InvalidateManifestCache(page);
            }
        }

        private static void LogPageChanged(PageData page)
        {
            LogHelper.Log(string.Format("Page name {0} id {1} updated change date to {2}", page.PageName,
                page.PageLink.ID, page.Changed.ToString("o")));
        }

        private void SetChangedDate(PageData page, DateTime changed)
        {
            if (page != null)
            {
                var clone = page.CreateWritableClone();
                clone.SetChangedOnPublish = true; 
                clone.Changed = changed;
                DataFactory.Instance.Save(clone,  SaveAction.ForceNewVersion); //avoids triggering the published event again.
                LogPageChanged(page);

                //remove page from episerver cache
                DataFactoryCache.RemovePage(page.ContentLink);
                LogHelper.Log(string.Format("Page id {0} was removed from episerver cache after SaveAction.ForceCurrentVersion", page.ContentLink));

                //remove page xml feed cache:
                PugPigCache.InvalidateFeedCache(page);
            }
        }
    }
}