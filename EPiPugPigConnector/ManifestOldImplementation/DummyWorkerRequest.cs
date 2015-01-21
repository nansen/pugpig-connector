using System;
using System.IO;
using System.Text;
using System.Web;

namespace EPiPugPigConnector.ManifestOldImplementation
{
    public class DummyWorkerRequest : HttpWorkerRequest
    {
        protected bool hasRuntimeInfo = true;
        protected string appPhysPath;
        protected string appVirtPath;
        protected string installDir;
        protected string page;
        protected string pathInfo;
        protected string queryString;
        protected string hostName;
        protected TextWriter output;

        public TextWriter OutputWriter
        {
            get
            {
                return this.output;
            }
        }

        public override string MachineConfigPath
        {
            get
            {
                return (string)null;
            }
        }

        public override string MachineInstallDirectory
        {
            get
            {
                return (string)null;
            }
        }

        public DummyWorkerRequest(string AppPhysPath, string AppVirtPath, string Page, string Query, string hostName, TextWriter Output)
        {
            this.queryString = Query;
            this.output = Output;
            this.page = Page;
            this.appPhysPath = AppPhysPath;
            this.appVirtPath = AppVirtPath;
            this.hostName = hostName;
        }

        public override void EndOfRequest()
        {
        }

        public override void FlushResponse(bool finalFlush)
        {
        }

        public override string GetAppPath()
        {
            return this.appVirtPath;
        }

        public override string GetAppPathTranslated()
        {
            return this.appPhysPath;
        }

        public override string GetFilePath()
        {
            return this.GetPathInternal(false);
        }

        public override string GetFilePathTranslated()
        {
            return this.GetPathInternal(false).Replace('/', '\\');
        }

        public override string GetHttpVerbName()
        {
            return "GET";
        }

        public override string GetHttpVersion()
        {
            return "HTTP/1.0";
        }

        public override string GetLocalAddress()
        {
            return "127.0.0.1";
        }

        public override int GetLocalPort()
        {
            return 80;
        }

        public override string GetPathInfo()
        {
            return string.Empty;
        }

        private string GetPathInternal(bool includePathInfo)
        {
            string str = VirtualPathUtility.ToAbsolute(this.page, this.appVirtPath);
            if (includePathInfo && this.pathInfo != null)
                return str + this.pathInfo;
            else
                return str;
        }

        public override string GetQueryString()
        {
            return this.queryString;
        }

        public override string GetRawUrl()
        {
            string queryString = this.GetQueryString();
            if (!string.IsNullOrEmpty(queryString))
                return this.GetPathInternal(true) + "?" + queryString;
            else
                return this.GetPathInternal(true);
        }

        public override string GetRemoteAddress()
        {
            return "127.0.0.1";
        }

        public override int GetRemotePort()
        {
            return 0;
        }

        public override string GetServerVariable(string name)
        {
            return string.Empty;
        }

        public override string GetUriPath()
        {
            return this.GetPathInternal(true);
        }

        public override IntPtr GetUserToken()
        {
            return IntPtr.Zero;
        }

        public override string MapPath(string path)
        {
            throw new NotImplementedException();
        }

        public override string GetKnownRequestHeader(int index)
        {
            if (index == 28)
                return this.hostName;
            else
                return base.GetKnownRequestHeader(index);
        }

        public override void SendKnownResponseHeader(int index, string value)
        {
        }

        public override void SendResponseFromFile(IntPtr handle, long offset, long length)
        {
        }

        public override void SendResponseFromFile(string filename, long offset, long length)
        {
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
            this.output.Write(Encoding.UTF8.GetChars(data, 0, length));
        }

        public override void SendStatus(int statusCode, string statusDescription)
        {
        }

        public override void SendUnknownResponseHeader(string name, string value)
        {
        }
    }
}
