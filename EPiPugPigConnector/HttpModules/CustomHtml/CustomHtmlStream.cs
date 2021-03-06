﻿using System;
using System.IO;
using System.Text;
using EPiPugPigConnector.HtmlCrawlers.CustomHtml.Generator;
using EPiPugPigConnector.HtmlCrawlers.CustomHtml.Modifiers;
using EPiPugPigConnector.Utils;

namespace EPiPugPigConnector.HttpModules.CustomHtml
{
    //From: http://patrickdesjardins.com/blog/modify-the-html-output-of-any-of-your-page-before-rendering
    public class CustomHtmlStream : Stream
    {
        private readonly Stream _filter;
        private readonly MemoryStream _cacheStream = new MemoryStream();
        private readonly Uri _currentRequestUri;

        public CustomHtmlStream(Stream filter, Uri currentRequestUri)
        {
            this._filter = filter;
            this._currentRequestUri = currentRequestUri;
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

                //Modify html output here.
                htmlDocument = ModifyHtmlOutput(htmlDocument);

                var buffer = Encoding.UTF8.GetBytes(htmlDocument);
                _filter.Write(buffer, 0, buffer.Length);
                _cacheStream.SetLength(0);
            }

            _filter.Flush();
        }

        private string ModifyHtmlOutput(string htmlDocument)
        {
            StopwatchTimer timer = new StopwatchTimer();

            //adds the .manifest src to the html element, (makes it easier to test the manifest in regular webbrowsers
            // as opposed to only testing via pugpig reader xml. )
            
            var htmlStreamProcessor = new HtmlGenerator();

            htmlStreamProcessor
                .AddModifier(new ManifestUrlModifier(_currentRequestUri))
                .AddModifier(new RelativeHrefUrlModifier(_currentRequestUri, "a", "href"))
                .AddModifier(new RelativeUrlModifier(_currentRequestUri, "link", "href"))
                .AddModifier(new RelativeUrlModifier(_currentRequestUri, "script", "src"))
                .AddModifier(new RelativeUrlModifier(_currentRequestUri, "img", "src"));
                
            var resultHtml = htmlStreamProcessor.GenerateHtml(htmlDocument);
            
            var timeElapsed = timer.Stop();

            return resultHtml;
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