
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectLapShop.Areas.admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
//admin
//m@gmail.com
//ahmedH@123->pass
//customer
//mans.gmail.com
//ahmed@H123->pass