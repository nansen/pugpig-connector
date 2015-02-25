using System.Net;

namespace EPiPugPigConnector.WebClients
{
    /// <summary>
    /// System web client.
    /// </summary>
    public class SystemWebClient : WebClient, IWebClient
    {
        public string BaseUrl
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}
