using EasyGames.Data;
using EasyGames.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Controllers;

[Authorize(Roles = "Owner")] // Only Owners can access stock management
public class StockController : Controller
{
    private readonly AppDbContext _db;
    public StockController(AppDbContext db) => _db = db;

    // GET: /Stock
    public async Task<IActionResult> Index()
    {
        var items = await _db.StockItems.ToListAsync();
        return View(items);
    }

    // GET: /Stock/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.StockItems.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    // GET: /Stock/Create
    public IActionResult Create() => View();

    // POST: /Stock/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StockItem item)
    {
        if (ModelState.IsValid)
        {
            _db.Add(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(item);
    }

    // GET: /Stock/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.StockItems.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    // POST: /Stock/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StockItem item)
    {
        if (id != item.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _db.Update(item);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.StockItems.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(item);
    }

    // GET: /Stock/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.StockItems.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    // POST: /Stock/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.StockItems.FindAsync(id);
        if (item != null)
        {
            _db.StockItems.Remove(item);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
