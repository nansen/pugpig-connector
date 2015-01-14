using System;

namespace EPiPugPigConnector.Editions.Interfaces.Editions
{
    /// <summary>
    /// Interface for the single Edition element ("Entry") in the Editions.xml OPDS feed.
    /// One Editions.xml can contain many entry elements.
    /// </summary>

    public interface IEditionsEntryElement
    {
        /// <summary>
        /// "The Arty Newspapers Issue#4 - 2014."
        /// </summary>
        string EntryTitle { get; set; }

        /// <summary>
        /// com.mycompany.edition0123456789 - just a unique string of some kind.
        /// </summary>
        string EntryId { get; set; }

        /// <summary>
        /// 2011-08-08T15:02:28+00:00
        /// </summary>
        string EntryUpdated { get; set; }

        /// <summary>
        /// Returns xml formatted datetime value.
        /// </summary>
        /// <param name="dateTimeUpdated"></param>
        /// <returns>2011-08-08T15:02:28+00:00</returns>
        string GetEntryUpdatedFormatted(DateTime dateTimeUpdated);

        /// <summary>
        /// Bob Smith
        /// </summary>
        string EntryAuthorName { get; set; }

        /// <summary>
        /// 2011-08-06
        /// </summary>
        string EntryDcTermsIssued { get; set; }

        /// <summary>
        /// (Can be left empty)
        /// Lorem ipsum dolor et amet.
        /// </summary>
        string EntrySummaryText { get; set; }

        /// <summary>
        /// "/img/cover.jpg"
        /// </summary>
        string EntryLinkCoverImage { get; set; }

        /// <summary>
        /// The link to the Edition.xml file.
        /// "edition1.xml"
        /// </summary>
        string EntryLinkEditionXml { get; set; }
    }
}
