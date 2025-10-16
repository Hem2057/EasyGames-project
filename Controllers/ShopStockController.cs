using EasyGames.Data;
using EasyGames.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



[Authorize(Roles="Owner")] // demo: proprietor role could be added later
public class ShopStockController : Controller
{
    private readonly AppDbContext
 _db;
    public ShopStockController(AppDbContext
 db) => _db = db;

    public async Task<IActionResult> Index(int shopId)
    {
        var shop = await _db.Shops.FindAsync(shopId);
        if (shop == null) return NotFound();
        ViewBag.Shop = shop;

        ViewBag.OwnerItems = await _db.StockItems.OrderBy(s=>s.Name).ToListAsync();
        var rows = await _db.ShopStocks.Include(s=>s.StockItem)
                    .Where(s=>s.ShopId==shopId).ToListAsync();
        return View(rows);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int shopId, int stockItemId, int qty)
    {
        if (qty <= 0) qty = 1;
        var item = await _db.StockItems.FindAsync(stockItemId);
        if (item == null) return NotFound();

        var ss = await _db.ShopStocks.FirstOrDefaultAsync(s => s.ShopId==shopId && s.StockItemId==stockItemId);
        if (ss == null)
        {
            ss = new ShopStock { ShopId = shopId, StockItemId = stockItemId, Quantity = 0 };
            _db.ShopStocks.Add(ss);
        }
        ss.Quantity += qty; // inventories are separate per PDF
        await _db.SaveChangesAsync();
        TempData["Toast"] = "Added to shop inventory.";
        return RedirectToAction(nameof(Index), new { shopId });
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, int qty, int shopId)
    {
        var ss = await _db.ShopStocks.FindAsync(id);
        if (ss == null) return NotFound();
        ss.Quantity = Math.Max(0, qty);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { shopId });
    }
}
