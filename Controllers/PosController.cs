using EasyGames.Data;
using EasyGames.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Authorize(Roles="Owner")]
public class PosController : Controller
{
    private readonly AppDbContext
 _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public PosController(AppDbContext
 db, UserManager<ApplicationUser> userManager)
    { _db = db; _userManager = userManager; }

    public async Task<IActionResult> Index(int? shopId)
    {
        var shops = await _db.Shops.ToListAsync();
        ViewBag.Shops = shops;
        var selected = shopId ?? shops.FirstOrDefault()?.Id ?? 0;
        ViewBag.ShopId = selected;

        var stock = await _db.ShopStocks.Include(s=>s.StockItem)
                        .Where(s=>s.ShopId==selected).OrderBy(s=>s.StockItem!.Name).ToListAsync();
        return View(stock);
    }

    [HttpPost]
    public async Task<IActionResult> Sell(int shopId, int stockItemId, int qty, string? phone)
    {
        var ss = await _db.ShopStocks.Include(s=>s.StockItem)
                 .FirstOrDefaultAsync(s=>s.ShopId==shopId && s.StockItemId==stockItemId);
        if (ss == null || ss.StockItem == null) return NotFound();
        if (qty <= 0) qty = 1;

        ApplicationUser? user = null;
        if (!string.IsNullOrWhiteSpace(phone))
            user = await _db.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone);

        decimal discount = user?.Tier switch
        {
            "Platinum" => 0.20m, "Gold" => 0.15m, "Silver" => 0.10m, _ => 0m
        };
        var priceEach = ss.StockItem.SellPrice * (1 - discount);
        var lineTotal = priceEach * qty;

        var sale = new Sale { ShopId = shopId, UserId = user?.Id, TotalPrice = lineTotal };
        sale.Lines.Add(new SaleLine { StockItemId = stockItemId, Quantity = qty, UnitPrice = priceEach });

        ss.Quantity = Math.Max(0, ss.Quantity - qty);     // warn-only at zero

        if (user != null)
        {
            var profitPerUnit = ss.StockItem.SellPrice - ss.StockItem.BuyPrice;
            user.LifetimeProfitContribution += profitPerUnit * qty;
            user.Tier = user.LifetimeProfitContribution switch
            {
                >= 10000m => "Platinum",
                >= 5000m  => "Gold",
                >= 2000m  => "Silver",
                _         => "Bronze"
            };
            await _userManager.UpdateAsync(user);
        }

        _db.Sales.Add(sale);
        await _db.SaveChangesAsync();

        TempData["Toast"] = $"Sold {qty} × {ss.StockItem.Name} {(discount>0 ? $"({discount*100:0}% off)" : "")}";
        return RedirectToAction(nameof(Index), new { shopId });
    }
}
