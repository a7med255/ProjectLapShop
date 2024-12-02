using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectLapShop.Bl;
using ProjectLapShop.Models;
using ProjectLapShop.Utilities;
namespace ProjectLapShop.Areas.admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    public class ItemsController : Controller
    {

        public ItemsController(IItems items, ICategories categories, IItemTypes itemTypes,IOs os)
        {
            Clsitems = items;
            ClsCategories = categories;
            oClsItemTypes= itemTypes;
            ClsOs= os;  
        }

        IItems Clsitems ;
        ICategories ClsCategories ;
        IItemTypes oClsItemTypes;
        IOs ClsOs;

        public IActionResult List()
        {
            ViewBag.lstCategories = ClsCategories.GetAll();
            return View(Clsitems.GetAllItemsData(null));
        }
        public IActionResult Search(int id)
        {
            ViewBag.lstCategories = ClsCategories.GetAll();
            return View("List",Clsitems.GetAllItemsData(id));
        }
        public IActionResult Edit(int? itemId)
        {
            var ItemId = new Models.TbItem();
            ViewBag.lstCategories= ClsCategories.GetAll();
            ViewBag.lstItemType = oClsItemTypes.GetAll();
            ViewBag.lstOs = ClsOs.GetAll();
                
            if (itemId != null)
            {
                ItemId = Clsitems.GetById(Convert.ToInt32(itemId));
            }
            return View(ItemId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TbItem item, List<IFormFile> Files)
        {
            if (!ModelState.IsValid)
                return View("Edit", item);
            item.ImageName = await Helper.UploadImage(Files,"Items");
            Clsitems.Save(item);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int itemId)
        {
            Clsitems.Delete(itemId);
            return RedirectToAction("List");

        }
        
    }
}
    