using System;
using System.IO;
using System.Text;
using System.Web;
using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.Web.Routing;

namespace EPiPugPigConnector.Fakes
{
    public class FakePageTreeEditionsGenerator : IEditionsGenerator
    {
        private readonly IContentLoader _contentLoader;
        private readonly ILanguageBranchRepository _languageBranchRepository;
        private readonly UrlResolver _urlResolver;

        public FakePageTreeEditionsGenerator(IContentLoader contentLoader, UrlResolver urlResolver,
            ILanguageBranchRepository languageBranchRepository)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _languageBranchRepository = languageBranchRepository;
        }

        public string ContentType
        {
            get { return "text/xml"; }
        }

        public string GetEditions(string language = null)
        {
            var filePath = HttpContext.Current.Server.MapPath("~\\App_Data\\FakeEditions.xml");

            var sb = LoadXmlFile(filePath);
            
            return sb.ToString();;
        }
        
        public string GetEdition(string id)
        {
            var filePath = HttpContext.Current.Server.MapPath("~\\App_Data\\FakeEdition.xml");
            var sb = LoadXmlFile(filePath);

            return sb.ToString(); ;
        }

        private static StringBuilder LoadXmlFile(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(filePath))
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            return sb;
        }
    }
}
