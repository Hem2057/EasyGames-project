using EasyGames.Data;
using EasyGames.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyGames.Controllers;

[Authorize(Roles = Seed.OwnerRole)] // Only Owner can view/manage users
public class AdminUsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly RoleManager<IdentityRole> _roleMgr;

    public AdminUsersController(UserManager<ApplicationUser> userMgr, RoleManager<IdentityRole> roleMgr)
    {
        _userMgr = userMgr;
        _roleMgr = roleMgr;
    }

    // GET: /AdminUsers
    public async Task<IActionResult> Index()
    {
        var users = _userMgr.Users.ToList();
        var vm = new List<UserRow>();
        foreach (var u in users)
        {
            var roles = await _userMgr.GetRolesAsync(u);
            vm.Add(new UserRow
            {
                Id = u.Id,
                Email = u.Email ?? "",
                FullName = u.FullName ?? "",
                Roles = string.Join(", ", roles),
                Tier = u.Tier ?? "Bronze", 
                LifetimeProfitContribution = u.LifetimeProfitContribution
            });
        }
        ViewBag.AllRoles = _roleMgr.Roles.Select(r => r.Name!).ToList();
        return View(vm);
    }

    // POST: /AdminUsers/AddRole
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRole(string userId, string role)
    {
        var user = await _userMgr.FindByIdAsync(userId);
        if (user is null) return NotFound();

        if (!await _roleMgr.RoleExistsAsync(role))
            await _roleMgr.CreateAsync(new IdentityRole(role));

        await _userMgr.AddToRoleAsync(user, role);
        TempData["msg"] = $"Added role '{role}' to {user.Email}.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /AdminUsers/RemoveRole
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveRole(string userId, string role)
    {
        var user = await _userMgr.FindByIdAsync(userId);
        if (user is null) return NotFound();

        await _userMgr.RemoveFromRoleAsync(user, role);
        TempData["msg"] = $"Removed role '{role}' from {user.Email}.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /AdminUsers/Delete
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string userId)
    {
        var user = await _userMgr.FindByIdAsync(userId);
        if (user is null) return NotFound();

        await _userMgr.DeleteAsync(user);
        TempData["msg"] = $"Deleted {user.Email}.";
        return RedirectToAction(nameof(Index));
    }

    public class UserRow
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Roles { get; set; } = "";
        public string Tier { get; set; } = "Bronze";
    public decimal LifetimeProfitContribution { get; set; } = 0;
    }
}
