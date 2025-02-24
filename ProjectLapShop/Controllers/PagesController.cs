using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectLapShop.Bl;

namespace ProjectLapShop.Controllers
{
    public class PagesController : Controller
    {
        IPages ClsPage;
        public PagesController(IPages page)
        {
            ClsPage=page;
        }
        // GET: PagesController
        public ActionResult Index(int pageId)
        {
            var page=ClsPage.GetById(pageId);
            return View(page);
        }
    }
}
