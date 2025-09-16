using EasyGames.Data;
using EasyGames.Extensions;
using EasyGames.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Controllers;

// Session-based cart; requires login to Checkout/PlaceOrder
public class CartController : Controller
{
    private const string CartKey = "CART";
    private readonly AppDbContext _db;
    public CartController(AppDbContext db) => _db = db;

    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetJson<List<CartItem>>(CartKey) ?? new();
        ViewBag.Total = cart.Sum(c => c.Price * c.Qty);
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int id, int qty = 1)
    {
        var item = await _db.StockItems.FindAsync(id);
        if (item is null || qty <= 0) return BadRequest("Invalid item/qty.");

        var cart = HttpContext.Session.GetJson<List<CartItem>>(CartKey) ?? new();
        var existing = cart.FirstOrDefault(c => c.StockItemId == id);
        if (existing is null)
            cart.Add(new CartItem { StockItemId = id, Name = item.Name, Price = item.Price, Qty = qty });
        else
            existing.Qty += qty;

        HttpContext.Session.SetJson(CartKey, cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateQty(int id, int qty)
    {
        var cart = HttpContext.Session.GetJson<List<CartItem>>(CartKey) ?? new();
        var line = cart.FirstOrDefault(c => c.StockItemId == id);
        if (line is null) return NotFound();
        line.Qty = Math.Max(1, qty);
        HttpContext.Session.SetJson(CartKey, cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remove(int id)
    {
        var cart = HttpContext.Session.GetJson<List<CartItem>>(CartKey) ?? new();
        cart.RemoveAll(c => c.StockItemId == id);
        HttpContext.Session.SetJson(CartKey, cart);
        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult Checkout()
    {
        var cart = HttpContext.Session.GetJson<List<CartItem>>(CartKey) ?? new();
        if (!cart.Any()) return RedirectToAction(nameof(Index));
        ViewBag.Total = cart.Sum(c => c.Price * c.Qty);
        return View(cart);
    }

    // Mock order placement: simply decrements stock quantities
    [Authorize, HttpPost]
    public async Task<IActionResult> PlaceOrder()
    {
        var cart = HttpContext.Session.GetJson<List<CartItem>>(CartKey) ?? new();
        if (!cart.Any()) return RedirectToAction(nameof(Index));

        foreach (var c in cart)
        {
            var item = await _db.StockItems.FirstOrDefaultAsync(s => s.Id == c.StockItemId);
            if (item is null || item.Quantity < c.Qty)
            {
                TempData["err"] = $"Not enough stock for {c.Name}.";
                return RedirectToAction(nameof(Index));
            }
            item.Quantity -= c.Qty;
        }
        await _db.SaveChangesAsync();
        HttpContext.Session.Remove(CartKey);

        TempData["msg"] = "Order placed (mock). Thanks!";
        return RedirectToAction("Index", "Catalog");
    }
}
