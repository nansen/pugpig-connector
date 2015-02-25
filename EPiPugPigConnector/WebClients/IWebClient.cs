using System;

namespace EPiPugPigConnector.WebClients
{
    /// <summary>
    /// Abstracting the HTTP call behind an interface as we cant
    /// Mock WebClient - http://brunov.info/blog/2013/07/30/tdd-mocking-system-net-webclient/
    /// </summary>
    public interface IWebClient
    {
        // Required methods (subset of `System.Net.WebClient` methods).
        byte[] DownloadData(Uri address);
        byte[] UploadData(Uri address, byte[] data);

        string BaseUrl { get; set; }
    }

    public interface IWebClientFactory
    {
        IWebClient Create();
    }
}
