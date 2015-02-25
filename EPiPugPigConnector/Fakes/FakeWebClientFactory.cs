using System;
using System.IO;
using EPiPugPigConnector.WebClients;

namespace EPiPugPigConnector.Fakes
{
    public class FakeWebClientFactory : IWebClientFactory
    {
        public IWebClient Create()
        {
            return new FakeWebClient();
        }

        public IWebClient Create(string baseUrl)
        {
            return new FakeWebClient
            {
                BaseUrl = baseUrl
            };
        }
    }

    public class FakeWebClient : IWebClient
    {
        private string _fileToDownload;

        public string BaseUrl { get; set; }
        
        public byte[] DownloadData(Uri address)
        {
            // TODO: Hard coded to this file for now - Refactor to take any file.
            var cssFilePath = BaseUrl + @"\\TestFiles\\ManifestAssets\\Static\\css\\TestImageAssetsParsingFromBundle.css";
            
            //string cssFilePath = ;
            FileInfo cssFile = new FileInfo(cssFilePath);
            var streamReader = File.OpenText(cssFile.FullName);

            var bytes = ReadFully(streamReader.BaseStream);

            return bytes;
        }

        public byte[] UploadData(Uri address, byte[] data)
        {
            throw new NotImplementedException();
        }
        
        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
