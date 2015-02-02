using System;
using System.IO;
using System.Text;
using EPiPugPigConnector.Caching;

namespace EPiPugPigConnector.HttpModules.Manifest
{
    //From: http://patrickdesjardins.com/blog/modify-the-html-output-of-any-of-your-page-before-rendering
    public class CustomHtmlStream : Stream
    {
        private readonly Stream filter;
        private System.Uri currentRequestUri;
        private readonly MemoryStream cacheStream = new MemoryStream();

        public bool HasError { get; set; }

        public CustomHtmlStream(Stream filter, System.Uri currentRequestUri)
        {
            this.filter = filter;
            this.currentRequestUri = currentRequestUri;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            cacheStream.Write(buffer, 0, count);
        }

        public override void Flush()
        {
            if (cacheStream.Length > 0)
            {
                string htmlDocument = Encoding.UTF8.GetString(cacheStream.ToArray(), 0, (int)cacheStream.Length);

                //Get the manifest
                htmlDocument = GetManifest(htmlDocument);

                var buffer = Encoding.UTF8.GetBytes(htmlDocument);
                this.filter.Write(buffer, 0, buffer.Length);
                cacheStream.SetLength(0);
            }

            this.filter.Flush();
        }

        private string GetManifest(string htmlDocument)
        {
            string cacheKey = currentRequestUri.ToString(); //should map against page.GetFriendlyUrl(includeHost: true)
            var cacheType = PugPigCacheType.Manifest;

            if (PugPigCache.IsSet(cacheType, cacheKey))
            {
                //Get from cache
                return (string)PugPigCache.Get(cacheType, cacheKey);
            }
            else
            {
                //Create manifest output here.
                var htmlProcessor = new RelativeUrlHtmlProcessor(currentRequestUri);
                htmlDocument = htmlProcessor.ProcessHtml(htmlDocument);
                
                //Add to cache
                PugPigCache.Set(cacheType, cacheKey, htmlDocument);
                return htmlDocument;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.filter.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.filter.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.filter.Read(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return this.filter.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.filter.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.filter.CanWrite; }
        }

        public override long Length
        {
            get { return this.filter.Length; }
        }

        public override long Position
        {
            get { return this.filter.Position; }
            set { this.filter.Position = value; }
        }
    }
}