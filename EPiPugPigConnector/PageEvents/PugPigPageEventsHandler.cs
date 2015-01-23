using System;
using EPiPugPigConnector.Editions.Models.Pages;
using EPiPugPigConnector.EPiExtensions;
using EPiPugPigConnector.Helpers;
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

            //TODO: Clear affected cache for xml files and manifest files here
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

            //TODO: Clear affected cache for xml files and manifest files here
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
            if (!PageHelper.IsEditionsContainerOrEditionPage(page))
            {
                //Set changed date on the current page since this is not standard EPiServer behaviour. -> used in <updated> in the xml feed
                page.SetChangedOnPublish = true; //http://dodavinkeln.se/post/how-to-set-a-page-updated-programmatically
                page.Changed = DateTime.Now;
            }
        }

        private void SetChangedDate(PageData page, DateTime changed)
        {
            if (page != null)
            {
                var clone = page.CreateWritableClone();
                clone.SetChangedOnPublish = true; 
                clone.Changed = changed;
                DataFactory.Instance.Save(clone, SaveAction.ForceCurrentVersion); //avoids triggering the published event again.

                //remove page from episerver cache
                DataFactoryCache.RemovePage(page.ContentLink);
            }
        }
    }
}