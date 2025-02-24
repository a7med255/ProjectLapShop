using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectLapShop.Bl;
using ProjectLapShop.Models;

namespace ProjectLapShop.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;
        private readonly UserManager<ApplicationUser> _userManager;
        public WishlistController(IWishlistService wishlistService , UserManager<ApplicationUser> userManager)
        {
            _wishlistService = wishlistService;
            _userManager = userManager;

        }

        public async Task<IActionResult> List()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var wishlist = await _wishlistService.GetWishlistAsync(userId);
            return View(wishlist);
        }

        public async Task<IActionResult> Add(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            await _wishlistService.AddToWishlistAsync(userId, productId);
            return RedirectToAction("/Home/Index");
        }

        public async Task<IActionResult> Remove(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            await _wishlistService.RemoveFromWishlistAsync(userId, productId);
            return RedirectToAction("/Wishlist/List");
        }
    }
}
