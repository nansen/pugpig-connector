using System.Web.Mvc;
using EPiPugPigConnector.ManifestOldImplementation;
using EPiServer;
using EPiServer.Web;

namespace EPiPugPigConnector.Controllers
{
    public class ManifestController : Controller
    {
        private readonly IContentRepository _contentRepository;
        private readonly IManifestFactory _manifestFactory;

        public ManifestController(IContentRepository contentRepository, IManifestFactory manifestFactory)
        {
            _contentRepository = contentRepository;
            _manifestFactory = manifestFactory;
        }

        public ActionResult Index(string urlSegment)
        {
            if (urlSegment.ToLower().EndsWith(".segment"))
            {
                var manifestFactory = new ManifestFactory(_contentRepository);
                var model = manifestFactory.LoadModel();
                if (model == null)
                {
                    model = new ManifestModel();
                }

                model.StartPoint = SiteDefinition.Current.RootPage;
                model.IncludeStartPage = true; //TODO: IncludeStartPage.Checked;

                manifestFactory.Save(model);

                manifestFactory.FindPagesAndAddThem();
                manifestFactory.WriteManifestFile();

                return Content("File Generated!");
            }
            else
            {
                return new HttpNotFoundResult();
            }
        }
    }
}
