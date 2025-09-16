using EasyGames.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Data;

public static class Seed
{
    public const string OwnerRole = "Owner";
    public const string UserRole  = "User";

    public static async Task EnsureSeedAsync(IServiceProvider services)
    {
        using var scope   = services.CreateScope();
        var db            = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleMgr       = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userMgr       = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // I apply pending migrations so a fresh clone "just works"
        await db.Database.MigrateAsync();

        // ----- Roles -----
        foreach (var r in new[] { OwnerRole, UserRole })
            if (!await roleMgr.RoleExistsAsync(r))
                await roleMgr.CreateAsync(new IdentityRole(r)); // I create missing roles once

        // ----- Default Owner User -----
        var ownerEmail = "owner@easygames.local";
        var owner = await userMgr.FindByEmailAsync(ownerEmail);
        if (owner is null)
        {
            owner = new ApplicationUser
            {
                UserName       = ownerEmail,
                Email          = ownerEmail,
                EmailConfirmed = true,
                FullName       = "Site Owner"
            };
            var result = await userMgr.CreateAsync(owner, "Owner#123"); // I keep a dev-friendly password
            if (result.Succeeded)
            {
                await userMgr.AddToRoleAsync(owner, OwnerRole);          // I ensure Owner role is attached
            }
        }

        // ----- Sample Stock Items with images (idempotent upsert by Name) -----
        // I chose high-quality Unsplash images so the catalog looks professional.
        // If you need local images only, put files in wwwroot/images/products and use:
        // ImagePath = "/images/products/chess.jpg"

        var samples = new[]
        {
            new StockItem
            {
                Name        = "C# in Depth (4th Edition)",
                Category    = "Book",
                Price       = 69.00m,
                Quantity    = 12,
                Description = "Master modern C# with deep dives by Jon Skeet. Perfect for intermediate to advanced developers.",
                ImagePath   = "/images/Dotnet-course.jpg" // I added a clean book shot
                // ImagePath = "/images/products/csharp-in-depth.jpg"
            },
            new StockItem
            {
                Name        = "Moto Race(PS5)",
                Category    = "Game",
                Price       = 99.00m,
                Quantity    = 8,
                Description = "Open-world action RPG by FromSoftware. Explore, fight, and forge your own path across the Lands Between.",
                ImagePath   = "/images/moto.jpg" // I added an epic game-style visual
                // ImagePath = "/images/products/elden-ring.jpg"
            },
            new StockItem
            {
                Name        = "Chess Set (Wood)",
                Category    = "Toy",
                Price       = 49.00m,
                Quantity    = 20,
                Description = "Hand-polished wooden chess set with felt bases. Includes folding board for easy storage.",
                ImagePath   = "/images/chess.jpg" // I added an elegant chess photo
                // ImagePath = "/images/products/chess.jpg"
            },
            new StockItem
            {
                Name        = "Machine Learning",
                Category    = "Book",
                Price       = 62.00m,
                Quantity    = 10,
                Description = "A timeless classic on software craftsmanship, practical techniques, and thinking like a pragmatic dev.",
                ImagePath   = "/images/Ai.jpg"
                // ImagePath = "/images/products/pragmatic-programmer.jpg"
            },
            new StockItem
            {
                Name        = "Battle Field",
                Category    = "Game",
                Price       = 98.00m,
                Quantity    = 14,
                Description = "Ergonomic wireless controller with HD rumble and motion controls. Great battery life.",
                ImagePath   = "/images/Battle.webp"
                // ImagePath = "/images/products/switch-pro.jpg"
            },
            new StockItem
            {
                Name        = "LEGO Creator 3-in-1: Street Racer",
                Category    = "Toy",
                Price       = 45.00m,
                Quantity    = 25,
                Description = "3-in-1 build: street racer, hot rod, or speed boat. Imagination and fine-motor fun.",
                ImagePath   = "/images/s-racer.jpg"
                // ImagePath = "/images/products/lego-street-racer.jpg"
            }
        };

        // I upsert by Name so re-running seed updates fields without duplicating rows.
        foreach (var s in samples)
        {
            var existing = await db.StockItems.FirstOrDefaultAsync(x => x.Name == s.Name);
            if (existing is null)
            {
                db.StockItems.Add(s);                 // I insert new item
            }
            else
            {
                // I refresh basic fields to keep demo data current if seed re-runs
                existing.Category    = s.Category;
                existing.Price       = s.Price;
                existing.Quantity    = Math.Max(existing.Quantity, s.Quantity); // I keep higher qty to avoid accidental depletion
                existing.Description = s.Description;
                existing.ImagePath   = s.ImagePath;
                db.StockItems.Update(existing);
            }
        }

        await db.SaveChangesAsync();
    }
}
