using EasyGames.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Controllers;

// Public catalog where users browse stock (books/games/toys)
public class CatalogController : Controller
{
    private readonly AppDbContext _db;
    public CatalogController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? category, string? q)
    {
        var query = _db.StockItems.AsQueryable();
        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(s => s.Category == category);
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(s => s.Name.Contains(q));
        var items = await query.OrderBy(s => s.Name).ToListAsync();
        return View(items);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.StockItems.FindAsync(id);
        return item is null ? NotFound() : View(item);
    }
}
