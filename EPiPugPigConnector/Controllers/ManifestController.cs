using System.Web.Mvc;
using EPiPugPigConnector.Manifest;
using EPiServer;
using EPiServer.Web;

namespace EPiPugPigConnector.Controllers
{
    public class ManifestController : Controller
    {
        private readonly IContentRepository _contentRepository; 

        public ManifestController(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public ActionResult Index()
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
    }
}
