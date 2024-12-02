using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectLapShop.Bl;
using ProjectLapShop.Models;
using ProjectLapShop.Utilities;
namespace ProjectLapShop.Areas.admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")] 
    public class CategoriesController : Controller
    {

        public CategoriesController(ICategories categories)
        {
            clsCategories=categories;
        }
        ICategories clsCategories;

        public IActionResult List()
        {

            return View(clsCategories.GetAll());
        }
        public IActionResult Edit(int? CategoryId)
        {
            var category = new TbCategory();
            if(CategoryId != null)
            {
                category = clsCategories.GetById(Convert.ToInt32( CategoryId));
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TbCategory category,List<IFormFile>Files)
        {
            if (!ModelState.IsValid)
                return View("Edit", category);
            category.ImageName = await Helper.UploadImage(Files, "Categories");
           clsCategories.Save(category);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int CategoryId)
        {
            clsCategories.Delete( CategoryId);
            return RedirectToAction("List");

        }

    }
}
