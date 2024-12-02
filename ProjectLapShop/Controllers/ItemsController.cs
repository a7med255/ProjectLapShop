using Microsoft.AspNetCore.Mvc;
using ProjectLapShop.Bl;
using ProjectLapShop.Models;


namespace ProjectLapShop.Controllers
{
    public class ItemsController : Controller
    {
        public ItemsController(IItems items,IItemImages images)
        {
            ClsItems = items;
            ClsItemImages= images;
        }
        IItems ClsItems;
        IItemImages ClsItemImages;
        public IActionResult ItemsDetails(int id)
        {
            var item=ClsItems.GetItemId(id);
            VwDetails vwDetails = new VwDetails();
            vwDetails.VwItem = item;
            vwDetails.lstRecommendedItems = ClsItems.GetRecommendedItems(id).Take(12).ToList();
            vwDetails.lstItemImages = ClsItemImages.GetById(id);
            
            return View(vwDetails);
        }
    }
}
