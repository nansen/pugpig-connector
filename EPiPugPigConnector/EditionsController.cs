using System;
using System.Web.Mvc;

namespace EPiPugPigConnector
{
    /// <summary>
    /// Controller handling editions.xml requests. 
    /// </summary>
    public class EditionsController : Controller
    {
        private readonly IEditionsGenerator _generator;

        public EditionsController(IEditionsGenerator generator)
        {
            _generator = generator;
        }

        public ActionResult Editions()
        {
            string lang = null;
            var editions = _generator.GetEditions(lang);
            if (editions == null)
            {
                return new HttpNotFoundResult();
            }

            return Content(editions, _generator.ContentType);
        }

        public ActionResult Edition(string id)
        {
            if (id == null)
            {
                return new HttpNotFoundResult();
            }

            var edition = _generator.GetEdition(id);
            return Content(edition, _generator.ContentType);
        }
    }
}
