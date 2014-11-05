using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector
{
    public interface IEditionsGenerator
    {
        /// <summary>
		/// Gets the contenttype this sitemap is outputting, for a xml sitemap, this would be "text/xml".
		/// </summary>
		string ContentType { get; }

        string GetEditions(string language = null);
        string GetEdition(string id);
    }
}
