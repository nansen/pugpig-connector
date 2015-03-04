using System;
using System.IO;
using System.Text;
using EPiPugPigConnector.Caching;
using EPiPugPigConnector.HtmlCrawlers.Manifest.Crawlers;
using EPiPugPigConnector.HtmlCrawlers.Manifest.Generator;
using EPiPugPigConnector.WebClients;

namespace EPiPugPigConnector.HttpModules.Manifest
{
    //From: http://patrickdesjardins.com/blog/modify-the-html-output-of-any-of-your-page-before-rendering
    public class ManifestStream : Stream
    {
        private readonly Stream _filter;
        private readonly System.Uri _currentRequestUri;
        private readonly IWebClient _webClient;
        private readonly MemoryStream _cacheStream = new MemoryStream();

        public bool HasError { get; set; }

        public ManifestStream(Stream filter, System.Uri currentRequestUri, IWebClient webClient)
        {
            this._filter = filter;
            this._currentRequestUri = currentRequestUri;
            _webClient = webClient;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _cacheStream.Write(buffer, 0, count);
        }

        public override void Flush()
        {
            if (_cacheStream.Length > 0)
            {
                string htmlDocument = Encoding.UTF8.GetString(_cacheStream.ToArray(), 0, (int)_cacheStream.Length);

                //Get the manifest
                htmlDocument = GetManifest(htmlDocument);

                var buffer = Encoding.UTF8.GetBytes(htmlDocument);
                this._filter.Write(buffer, 0, buffer.Length);
                _cacheStream.SetLength(0);
            }

            this._filter.Flush();
        }

        private string GetManifest(string htmlDocument)
        {
            string cacheKey = _currentRequestUri.ToString(); //should map against page.GetFriendlyUrl(includeHost: true)
            var cacheType = PugPigCacheType.Manifest;

            //if (PugPigCache.IsSet(cacheType, cacheKey))
            //{
            //    //Get from cache
            //    return (string)PugPigCache.Get(cacheType, cacheKey);
            //}
            //else
            //{

            var manifestGenerator = new ManifestGenerator();
            manifestGenerator
                .AddCrawler(new RelativeUrlCrawler(_currentRequestUri, "link", "href"))
                .AddCrawler(new RelativeUrlCrawler(_currentRequestUri, "script", "src"))
                .AddCrawler(new RelativeUrlCrawler(_currentRequestUri, "img", "src"))
                .AddCrawler(new CssCrawler(_webClient, _currentRequestUri));

            var manifestFile = manifestGenerator.GenerateManifest(htmlDocument);
            
            //Add to cache
            PugPigCache.Set(cacheType, cacheKey, htmlDocument);
            return manifestFile;
            //}
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this._filter.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this._filter.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._filter.Read(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return this._filter.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this._filter.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this._filter.CanWrite; }
        }

        public override long Length
        {
            get { return this._filter.Length; }
        }

        public override long Position
        {
            get { return this._filter.Position; }
            set { this._filter.Position = value; }
        }
    }
}