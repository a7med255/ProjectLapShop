using Microsoft.EntityFrameworkCore;
using ProjectLapShop.Models;

namespace ProjectLapShop.Bl
{

    public interface IWishlistService
    {
        Task AddToWishlistAsync(string userId, int productId);
        Task<List<WishlistItem>> GetWishlistAsync(string userId);
        Task RemoveFromWishlistAsync(string userId, int productId);
    }

    public class WishlistService : IWishlistService
    {
        private readonly LapShopContext _context;

        public WishlistService(LapShopContext context)
        {
            _context = context;
        }

        public async Task AddToWishlistAsync(string userId, int productId)
        {
            if (!_context.TbWishlistItems.Any(w => w.UserId == userId && w.ProductId == productId))
            {
                var wishlistItem = new WishlistItem
                {
                    UserId = userId,
                    ProductId = productId,
                    DateAdded = DateTime.UtcNow
                };
                _context.TbWishlistItems.Add(wishlistItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<WishlistItem>> GetWishlistAsync(string userId)
        {
            return await _context.TbWishlistItems
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task RemoveFromWishlistAsync(string userId, int productId)
        {
            var wishlistItem = await _context.TbWishlistItems
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (wishlistItem != null)
            {
                _context.TbWishlistItems.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
