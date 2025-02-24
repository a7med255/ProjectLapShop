using Microsoft.AspNetCore.Mvc;
using ProjectLapShop.Bl;
using ProjectLapShop.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Stripe.Checkout;
namespace ProjectLapShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IItems _itemService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISalesInvoice _clsSalesInvoice;
        private readonly ISalesInvoiceItems _clsSalesInvoiceItems;

        public OrderController(IItems itemService, UserManager<ApplicationUser> userManager,
            ISalesInvoiceItems salesInvoiceItems, ISalesInvoice salesInvoice)
        {
            _itemService = itemService;
            _userManager = userManager;
            _clsSalesInvoice = salesInvoice;
            _clsSalesInvoiceItems = salesInvoiceItems;
        }

        private ShoppingCart GetCartFromCookies()
        {
            var cartJson = HttpContext.Request.Cookies["Cart"];
            return string.IsNullOrEmpty(cartJson) ? new ShoppingCart()
                : JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
        }

        private void UpdateCartCookie(ShoppingCart cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Response.Cookies.Append("Cart", cartJson, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Use Secure if using HTTPS
                Expires = DateTime.Now.AddMinutes(30)
            });
        }

        public IActionResult Cart()
        {
            var cart = GetCartFromCookies();
            return View(cart);
        }

        public async Task<IActionResult> MyOrder()
        {
            try
            {
                // Get the current logged-in user
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Unable to retrieve user information.";
                    return RedirectToAction("Index", "Home");
                }

                // Fetch orders for the current user
                var userId = Guid.Parse(user.Id);
                var orders = _clsSalesInvoice.GetOrdersByCustomerId(userId);
                
                // Pass the orders to the view
                return View(orders);
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                TempData["ErrorMessage"] = "An error occurred while fetching your orders.";
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        public async Task<IActionResult> OrderDetailsAsync(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                ViewBag.User = user.FirstName + " " + user.LastName;
                var order = _clsSalesInvoice.GetById(id);
                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToAction("MyOrder");
                }

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching the order details.";
                return RedirectToAction("MyOrder");
            }
        }

        [Authorize]
        public IActionResult AddToCart(int itemId)
        {
            var cart = GetCartFromCookies();
            var item = _itemService.GetById(itemId);

            if (item == null)
            {
                TempData["ErrorMessage"] = "Item not found.";
                return RedirectToAction("Cart");
            }

            var itemInCart = cart.lstItems.FirstOrDefault(a => a.ItemId == itemId);

            if (itemInCart != null)
            {
                itemInCart.Qty++;
                itemInCart.Total = itemInCart.Qty * itemInCart.Price;
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

            cart.TotalCard = cart.lstItems.Sum(a => a.Total);
            UpdateCartCookie(cart);

            return RedirectToAction("Cart");
        }
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCartFromCookies();
            if (!cart.lstItems.Any())
            {
                TempData["ErrorMessage"] = "No items in the cart to process.";
                return RedirectToAction("Cart");
            }
            var domin = "http://localhost:5002/";
            var option = new SessionCreateOptions
            {
                SuccessUrl=domin+$"Order/OrderSuccess",
                CancelUrl=domin+$"Order/Cart",
                LineItems=new List<SessionLineItemOptions>(),
                Mode="payment",
                CustomerEmail="ah8455545@gmail.com"

            };

            foreach (var item in cart.lstItems)
            {
                var SessionListItem = new SessionLineItemOptions
                {
                    PriceData=new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * item.Qty * 100),
                        Currency = "inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ItemName.ToString(),
                        },
                    },
                    Quantity=item.Qty,
                };
                option.LineItems.Add(SessionListItem);
            }
            var service=new SessionService();
            Session session = service.Create(option);

            Response.Headers.Add("Location",session.Url);

            return new StatusCodeResult(303);
        }

        [Authorize]
        public async Task<IActionResult> OrderSuccess()
        {
            var cart = GetCartFromCookies();

            if (!cart.lstItems.Any())
            {
                TempData["ErrorMessage"] = "No items in the cart to process.";
                return RedirectToAction("Cart");
            }

            try
            {
                await SaveOrder(cart);
                HttpContext.Response.Cookies.Delete("Cart");
            }
            catch (Exception ex)
            {
                // Log error (e.g., using ILogger)
                TempData["ErrorMessage"] = "An error occurred while processing your order.";
                return RedirectToAction("Cart");
            }

            return View();
        }

        [Authorize]
        private async Task SaveOrder(ShoppingCart shoppingCart)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) throw new Exception("User not found.");

                var lstInvoiceItems = shoppingCart.lstItems.Select(item => new TbSalesInvoiceItem
                {
                    ItemId = item.ItemId,
                    Qty = item.Qty,
                    InvoicePrice = item.Price
                }).ToList();

                var salesInvoice = new TbSalesInvoice
                {
                    InvoiceDate = DateTime.Now,
                    CustomerId = Guid.Parse(user.Id),
                    DelivryDate = DateTime.Now.AddDays(5),
                    CreatedBy = user.Id,
                    CreatedDate = DateTime.Now
                };

                _clsSalesInvoice.Save(salesInvoice, lstInvoiceItems, true);
            }
            catch (Exception ex)
            {
                // Log error
                throw;
            }
        }

        public IActionResult RemoveItem(int id)
        {
            var cart = GetCartFromCookies();

            var itemToRemove = cart.lstItems.FirstOrDefault(i => i.ItemId == id);
            if (itemToRemove != null)
            {
                cart.lstItems.Remove(itemToRemove);
                cart.TotalCard = cart.lstItems.Sum(a => a.Total);
                UpdateCartCookie(cart);
            }

            return RedirectToAction("Cart");
        }
    }
}
