using Microsoft.AspNetCore.Mvc;
using ProjectLapShop.Bl;
using ProjectLapShop.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
namespace ProjectLapShop.Controllers
{
    public class OrderController : Controller
    {
        IItems itemService; 
        public OrderController(IItems item)
        {
            itemService = item;
        }
        public IActionResult Cart()
        {
            string sesstionCart=string .Empty;
            if (HttpContext.Request.Cookies["Cart"] != null)
                sesstionCart = HttpContext.Request.Cookies["Cart"];
            var cart = JsonConvert.DeserializeObject<ShoppingCart>(sesstionCart);
            return View(cart);
        }
        public IActionResult MyOrder()
        {
            return View();
        }
        [Authorize]
        public IActionResult OrderSuccess()
        {
            return View();
        }
        public IActionResult AddToCart(int itemId )
        {
            ShoppingCart cart;
            if(HttpContext.Request.Cookies["Cart"] != null)
                cart= JsonConvert.DeserializeObject<ShoppingCart>(HttpContext.Request.Cookies["Cart"]);
            else
                cart = new ShoppingCart();
            var item=itemService.GetById( itemId );
            var itemInList=cart.lstItems.Where(a=>a.ItemId==itemId).FirstOrDefault();
            if (itemInList != null)
            {
                itemInList.Qty++;
                itemInList.Total= itemInList.Qty*itemInList.Price;
            }
            else
            {
                cart.lstItems.Add(new ShoppingCartItem
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    ImageName = item.ImageName,
                    Price = item.SalesPrice,
                    Qty = 1,
                    Total = item.SalesPrice
                });
            }
            cart.Total=cart.lstItems.Sum(a=>a.Total);
            HttpContext.Response.Cookies.Append("Cart",JsonConvert.SerializeObject(cart));

            return RedirectToAction("Cart");
        }
    }
}
