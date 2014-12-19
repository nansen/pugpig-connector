using System.IO;
using System.Text;

namespace EPiPugPigConnector.HttpModules.RelativeUrlModule
{
    //From: http://patrickdesjardins.com/blog/modify-the-html-output-of-any-of-your-page-before-rendering
    public class CustomHtmlStream : Stream
    {
        private readonly Stream filter;
        private readonly MemoryStream cacheStream = new MemoryStream();
        private System.Uri currentRequestUri;

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

                //todo: modify html output here.
                htmlDocument = htmlDocument.Replace("</body>", "<h1>PIGPUG CONNECTOR WAS HERE!</h1></body>");

                UrlToRelativeUrlHtmlProcessor htmlProcessor = new UrlToRelativeUrlHtmlProcessor(currentRequestUri);
                htmlDocument = htmlProcessor.ProcessHtml(htmlDocument);
                
                var buffer = Encoding.UTF8.GetBytes(htmlDocument);
                this.filter.Write(buffer, 0, buffer.Length);
                cacheStream.SetLength(0);
            }

            this.filter.Flush();
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