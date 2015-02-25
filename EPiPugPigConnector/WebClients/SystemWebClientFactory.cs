using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiPugPigConnector.WebClients
{
    /// <summary>
    /// System web client factory.
    /// </summary>
    public class SystemWebClientFactory : IWebClientFactory
    {
        #region IWebClientFactory implementation

        public IWebClient Create()
        {
            return new SystemWebClient();
        }

        #endregion
    }
}
