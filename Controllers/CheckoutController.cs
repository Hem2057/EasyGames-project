using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EasyGames.Data;
using EasyGames.Models;
using System.Linq;
using System.Threading.Tasks;
using EasyGames; // for SessionExtensions

namespace EasyGames.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _db;

        public CheckoutController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // Load cart from session
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Show grand total
            ViewBag.GrandTotal = cart.Sum(i => i.Price * i.Qty);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Complete()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
                return RedirectToAction("Index", "Cart");

            // Simulate checkout: reduce stock and clear cart
            foreach (var item in cart)
            {
                var stock = _db.StockItems.FirstOrDefault(s => s.Id == item.StockItemId);
                if (stock != null)
                {
                    stock.Quantity -= item.Qty;
                    if (stock.Quantity < 0)
                        stock.Quantity = 0;
                }
            }

            await _db.SaveChangesAsync();

            // clear the cart
            HttpContext.Session.SetObjectAsJson("Cart", new List<CartItem>());

            TempData["Success"] = "Checkout complete! Stock updated.";
            return RedirectToAction("Index", "Catalog");
        }
    }
}
