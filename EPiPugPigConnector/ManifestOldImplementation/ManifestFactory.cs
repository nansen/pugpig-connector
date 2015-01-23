//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;
//using EPiServer;
//using EPiServer.Core;
//using EPiServer.Data.Dynamic;
//using EPiServer.HtmlParsing;
//using EPiServer.Web;

//namespace EPiPugPigConnector.ManifestOldImplementation
//{
//    public class ManifestFactory : IManifestFactory
//    {
//        //#region Singleton

//        //private static ManifestFactory _instance;
      
//        //public static ManifestFactory Instance
//        //{
//        //    get
//        //    {
//        //        if (_instance == null)
//        //        {
//        //            _instance = new ManifestFactory();            
//        //        }

//        //        return _instance;
//        //    }
//        //}

//        //#endregion

//        private readonly IContentRepository _contentRepository;

//        public Dictionary<string, string> LocalURLs { get; set; }
//        public DynamicDataStore store
//        {
//            get
//            {
//               return  DynamicDataStoreFactory.Instance.GetStore(typeof(ManifestModel)) ?? DynamicDataStoreFactory.Instance.CreateStore(typeof(ManifestModel));  
//            }
       
//        }

//        public ManifestModel Settings { get; set; }

//        public ManifestFactory(IContentRepository contentRepository)
//        {
//             LocalURLs = new Dictionary<string, string>();
//             _contentRepository = contentRepository;
//        }

//        public ManifestModel LoadModel()
//        {         
//            return store.Items<ManifestModel>().FirstOrDefault();
//        }

//        public void Save(ManifestModel model)
//        {         
//            store.Save(model);
//        }

//        /// <summary>
//        /// Find all pages under the page set in the settings area in admin mode
//        /// Get all the links from that page and include them
//        /// </summary>
//        public void FindPagesAndAddThem()
//        {
//            var model = LoadModel();
//            if (model != null)
//            {
//                if (!ContentReference.IsNullOrEmpty(model.StartPoint))
//                {
//                    try
//                    {
//                        var allChildren = _contentRepository.GetDescendents(model.StartPoint).ToList();
//                        if (model.IncludeStartPage)
//                        {
//                            allChildren.Add(model.StartPoint);
//                        }
//                        foreach (var pr in allChildren)
//                        {
//                            var pd = _contentRepository.Get<PageData>(pr);
//                            UpdateFile(pd.ToString(), GetFriendlyUrl(pd));
//                            UpdateLinksOnPage(pd);
//                        }

//                    }
//                    catch (Exception) { }
//                }
//            }
//        }


//        /// <summary>
//        /// Create a string to be saved in the Manifest file
//        /// </summary>
//        /// <returns></returns>
//        public string MakeManifestFileAsString()
//        {
//            StringBuilder strb = new StringBuilder();
//            strb.Append("CACHE MANIFEST" + Environment.NewLine);
//            strb.AppendFormat("# This file was generated at {0}" + Environment.NewLine, DateTime.Now);
//            strb.Append("CACHE:" + Environment.NewLine);
//            foreach (string filePath in LocalURLs.Values)
//            {
//                strb.AppendFormat(String.Format("{0}" + Environment.NewLine, filePath));
//            }

//            strb.Append("NETWORK:"+Environment.NewLine);
//            strb.Append("*" + Environment.NewLine);

//            return strb.ToString();
//        }

//        /// <summary>
//        /// Write the Manifest file in the root fot he site
//        /// </summary>
//        public void WriteManifestFile()
//        {
//            try
//            {
//                TextWriter writer = new StreamWriter(HttpContext.Current.Request.MapPath("~/mobilepack.manifest"));
//                writer.WriteLine(MakeManifestFileAsString());
//                writer.Close();
//            }
//            catch { };
//        }

//        //Update a page if an event is changed
//        public void UpdatePage(PageData pd)
//        {
//            UpdateFile(pd.PageGuid.ToString(), GetFriendlyUrl(pd));
//            if (pd != null)
//            {
//                UpdateLinksOnPage(pd);
//            }
//        }

//        private void UpdateFile(string key, string file)
//        {
//            //never add the manifest file to the offline cache
//            if (!file.ToLower().Contains(".manifest"))
//            {
//                if (key.StartsWith("/") || file.StartsWith("/"))
//                    return;

//                if (LocalURLs.Keys.Contains(key))
//                {
//                    LocalURLs[key] = file;
//                }
//                else
//                {
//                    LocalURLs.Add(key, file);
//                }
//            }
//        }

//        /// <summary>
//        /// Get links on a specific page and save them
//        /// </summary>
//        /// <param name="pd"></param>
//        public void UpdateLinksOnPage(PageData pd)
//        {
//            var html = GetHtmlFromPage(pd);
//            var sr = new HtmlStreamReader(html);
//            var linkResult = sr.OfType<ElementFragment>().SelectMany(ee => ee.Attributes).
//                Where(a => (a.Token == AttributeToken.Src || a.Token == AttributeToken.Href)).Select(a => a.UnquotedValue);

//            foreach (var s in linkResult.ToList())
//            {
//                var url2 = new UrlBuilder(s);
//                Global.UrlRewriteProvider.ConvertToInternal(url2);
//                var pageref = PermanentLinkUtility.GetContentReference(url2);
//                if (pageref != PageReference.EmptyReference)
//                {
//                    //UpdateFile(_contentRepository.Get<PageData>(pageref).PageGuid.ToString(), s);
//                }
//                else if(!s.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
//                {
//                    if (s.Contains("mailto") || s.LastIndexOf('/') == s.Length - 1)
//                        continue;

//                    var relS = s.IndexOf('/') == 0 ? string.Concat("..", s) : s;
                    
//                    if (relS.StartsWith("/"))
//                        relS = s.Substring(1, s.Length - 1);

//                    UpdateFile(relS, relS);
//                }
//            }
//        }

//        /// <summary>
//        /// Get friendly url from PageData
//        /// </summary>
//        /// <param name="pd"></param>
//        /// <returns></returns>
//        private string GetFriendlyUrl(PageData pd)
//        {
//            UrlBuilder url = new UrlBuilder(pd.LinkURL); 
//            EPiServer.Global.UrlRewriteProvider.ConvertToExternal(url, pd.PageLink, System.Text.UTF8Encoding.UTF8);
//            return url.ToString();
//        }

//        private string GetHtmlFromPage(PageData pageData)
//        {
//            string pageVersionUri = GetPageVersionUri(pageData);

//            UrlBuilder internalUrl = new UrlBuilder(pageVersionUri);
//            UrlBuilder urlBuilder = new UrlBuilder(pageVersionUri);
//            Global.UrlRewriteProvider.ConvertToExternal(urlBuilder, (object)pageData.PageLink, Encoding.UTF8);

//            var uri = string.Concat(SiteDefinition.Current.SiteUrl.ToString(), urlBuilder.Uri.ToString());

//            string htmlByDummyRequest = GetHtmlByWebRequest(uri);
            
            
//            return Global.UrlRewriteProvider.GetHtmlRewriter().RewriteString(internalUrl, urlBuilder, Encoding.UTF8, htmlByDummyRequest);
//        }

//        private string GetPageVersionUri(PageData pageData)
//        {
//            return new UrlBuilder(UriSupport.BuildUrlWithPageReference(pageData.LinkURL, pageData.PageLink))
//            {
//                QueryLanguage = pageData.LanguageBranch
//            }.Uri.ToString();
//        }

//        private static string GetHtmlByWebRequest(string pageUrl)
//        {
//            //var fullUrl = GetFullExternalUrl(pageUrl);

//            System.Net.WebRequest req = System.Net.WebRequest.Create(pageUrl);
//               System.Net.WebResponse resp = req.GetResponse();
//               System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

//               return sr.ReadToEnd().Trim();
//        }

//        private static string GetHtmlByDummyRequest(string pageUrl)
//        {
              
//            DummyWorkerRequest dummyRequest = GetDummyRequest(pageUrl);
//            HttpContext current = HttpContext.Current;
//            try
//            {
//                HttpContext.Current = new HttpContext((HttpWorkerRequest) dummyRequest);
//                HttpContext.Current.ApplicationInstance = current.ApplicationInstance;
//                HttpContext.Current.User = current.User;
//                foreach (object index in (IEnumerable) current.Items.Keys)
//                    HttpContext.Current.Items[index] = current.Items[index];
//                HttpContext.Current.Handler = new MvcHttpHandler();
//                //lock (HttpContext.Current)
//                //{
//                //    //(IHttpHandler) BuildManager.CreateInstanceFromVirtualPath(dummyRequest.GetFilePath(), typeof (object));
//                //    typeof (HttpContext).InvokeMember("_ProfileDelayLoad",
//                //        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, (Binder) null,
//                //        (object) HttpContext.Current, new object[1]
//                //        {
//                //            (object) true
//                //        });
//                //    HttpContext.Current.Handler.ProcessRequest(HttpContext.Current);
//                //    HttpContext.Current.Response.Flush();
//                //}

//                var uri = "";
                

//                if (HttpContext.Current.Response.StatusCode >= 200 && HttpContext.Current.Response.StatusCode < 300)
//                    return "";
//                string str2;
//                if (HttpContext.Current.Response.RedirectLocation != null)
//                    str2 = "Redirect not supported";
//                else
//                    str2 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Status code not supported",
//                        new object[1]
//                        {
//                            (object) HttpContext.Current.Response.StatusCode
//                        });
//                throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}. {1}",
//                    new object[2]
//                    {
//                        (object) str2,
//                        (object) "Use side by side"
//                    }));
//            }
//            catch (InvalidCastException ex)
//            {
//                StringBuilder stringBuilder = new StringBuilder();
//                stringBuilder.Append("Failed to generate html");
//                if (ex.Message.Contains("System.Web.Hosting"))
//                {
//                    stringBuilder.Append(" ");
//                    stringBuilder.Append("Trace Warning");
//                }
//                //_log.Warn((object)"Unable to generate html for comparison", (Exception)ex);
//                return "Error fired when trying to create html comparsion";
//                //TODO:  Create or return error page. Utilities.CreateErrorPage(((object)stringBuilder).ToString());
//            }
//            catch (Exception ex)
//            {
//                return "Error fired when trying to create html comparsion";
//            }
//            finally
//            {
//                HttpContext.Current = current;
//            }
        
//    }

//        private static string GetFullExternalUrl(string pageUrl)
//        {
//            Url url = new Url(pageUrl);
//            string localPath = SiteDefinition.Current.SiteUrl.ToString();
//            string page = VirtualPathUtility.ToAppRelative(url.Path, localPath);

//            return page;
//        }

//        private static DummyWorkerRequest GetDummyRequest(string pageUrl)
//        {
//            Url url = new Url(pageUrl);
//            string localPath = SiteDefinition.Current.SiteUrl.LocalPath; //Settings.Instance.SiteUrl.LocalPath;
//            string Page = VirtualPathUtility.ToAppRelative(url.Path, localPath);
//            string Query = url.Query.TrimStart(new char[1]
//              {
//                '?'
//              });
//            StringWriter stringWriter = new StringWriter();
//            return new DummyWorkerRequest(HttpContext.Current.Request.PhysicalApplicationPath, localPath, Page, Query, HttpContext.Current.Request.Headers["Host"], (TextWriter)stringWriter);
//        }
//    }

//    public interface IManifestFactory
//    {
//        void WriteManifestFile();
//        void FindPagesAndAddThem();
//    }
//}
