using System;
using EPiPugPigConnector.EPiExtensions;
using EPiServer.Core;

namespace EPiPugPigConnector.Editions.Models.Pages.Helpers
{
    public class EditionHelper
    {
        public static EditionPage GetEditionPage(string id)
        {
            var pageReference = PageReference.Parse(id);

            if (!PageReference.IsNullOrEmpty(pageReference))
            {
                var page = pageReference.Get<EditionPage>();

                if (page.CheckPublishedStatus(PagePublishedStatus.Published) && !page.IsDeleted)
                {
                    return page;
                }
                else
                {
                    throw new Exception(string.Format("EditionPage is not published, page id: {0}", pageReference.ID));
                }
            }
            throw new Exception(string.Format("EditionPage with page id: {0} could not be found.", pageReference.ID));
        }
    }
}