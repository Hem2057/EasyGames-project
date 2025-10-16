using EasyGames.Data;
using EasyGames.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _db;

        public ProfileController(UserManager<ApplicationUser> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var sales = await _db.Sales
                                 .Include(s => s.Lines)
                                 .ThenInclude(l => l.StockItem)
                                 .Where(s => s.UserId == user.Id)
                                 .ToListAsync();

            ViewData["UserTier"] = user.Tier;
            ViewData["LifetimeProfit"] = user.LifetimeProfitContribution;

            return View((user, sales));
        }
    }
}
