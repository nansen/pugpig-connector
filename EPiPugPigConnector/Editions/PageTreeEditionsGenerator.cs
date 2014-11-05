using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector.Editions
{
    public class PageTreeEditionsGenerator : IEditionsGenerator
    {
        public string ContentType { get; private set; }
        public string GetEditions(string language = null)
        {
            throw new NotImplementedException();
        }

        public string GetEdition(string id)
        {
            throw new NotImplementedException();
        }
    }
}
