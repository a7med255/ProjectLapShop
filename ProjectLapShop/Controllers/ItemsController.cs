using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectLapShop.Bl;
using ProjectLapShop.Models;


namespace ProjectLapShop.Controllers
{
    [AllowAnonymous]
    public class ItemsController : Controller
    {
        public ItemsController(IItems items,IItemImages images)
        {
            ClsItems = items;
            ClsItemImages= images;
        }
        IItems ClsItems;
        IItemImages ClsItemImages;
        public async Task<IActionResult> ItemsDetails(int id)
        {
            var item =  ClsItems.GetItemId(id);
            if (item == null)
            {
                return NotFound();
            }

            var vwDetails = new VwDetails
            {
                VwItem = item,
                lstRecommendedItems = ClsItems.GetRecommendedItems(id).Take(12).ToList(),
                lstItemImages = ClsItemImages.GetById(id)
            };
            return View(vwDetails);
        }
        public IActionResult ItemList()
        {
            return View();
        }
        }
}
