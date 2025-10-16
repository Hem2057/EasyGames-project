using EasyGames.Data;
using EasyGames.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Authorize(Roles="Owner")]
public class ShopsController : Controller
{
    private readonly AppDbContext
 _db;
    public ShopsController(AppDbContext
 db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Shops.OrderBy(s=>s.Name).ToListAsync());
    public IActionResult Create() => View(new Shop());

    [HttpPost]
    public async Task<IActionResult> Create(Shop shop)
    {
        if (!ModelState.IsValid) return View(shop);
        _db.Shops.Add(shop);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var shop = await _db.Shops.FindAsync(id);
        return shop == null ? NotFound() : View(shop);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Shop shop)
    {
        if (!ModelState.IsValid) return View(shop);
        _db.Shops.Update(shop);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var shop = await _db.Shops.FindAsync(id);
        if (shop != null) { _db.Shops.Remove(shop); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}
