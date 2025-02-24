using Microsoft.AspNetCore.Mvc;
using ProjectLapShop.Areas.admin.Controllers;
using ProjectLapShop.Bl;
using ProjectLapShop.Models;

namespace ProjectLapShop.Controllers
{

    public class HomeController : Controller
    {
        IItems ClsItems;
        ISliders ClsSliders;
        ICategories ClsCategories;
        public HomeController(IItems items, ISliders sliders ,ICategories categories)
        {
            ClsItems = items;
            ClsSliders = sliders;
            ClsCategories = categories;
        }
        public IActionResult Index()
        {
           VwHome vwHome = new VwHome();
            vwHome.lstAllItems=ClsItems.GetAllItemsData(null).Take(20).ToList();
            vwHome.lstRecommenedItems = ClsItems.GetAllItemsData(null).Skip(60).Take(8).ToList();
            vwHome.lstNewItems = ClsItems.GetAllItemsData(null).Skip(90).Take(8).ToList();
            vwHome.lstFreeDelivary = ClsItems.GetAllItemsData(null).Skip(200).Take(8).ToList();
            vwHome.lstSliders = ClsSliders.GetAll();
            vwHome.lstCategories = ClsCategories.GetAll().Take(4).ToList();


            return View(vwHome);
        }
    }
}
