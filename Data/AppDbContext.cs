using EasyGames.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Data
{
    // Make sure your IdentityDbContext uses your ApplicationUser type
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Your app entities
        public DbSet<StockItem> StockItems { get; set; } = default!;

        // If you added orders later, include these too:
        // public DbSet<Order> Orders => Set<Order>();
        // public DbSet<OrderLine> OrderLines => Set<OrderLine>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- IMPORTANT ----------
            // SQL Server: keep DECIMAL(18,2)
            // SQLite: map decimal -> double (SQLite lacks true decimal precision)
            if (Database.ProviderName?.Contains("Sqlite") == true)
            {
                // Convert decimal to double for SQLite
                modelBuilder.Entity<StockItem>()
                            .Property(p => p.Price)
                            .HasConversion<double>();

                // If you have these entities, convert their decimals too:
                // modelBuilder.Entity<Order>()
                //             .Property(p => p.Total)
                //             .HasConversion<double>();
                //
                // modelBuilder.Entity<OrderLine>()
                //             .Property(p => p.Price)
                //             .HasConversion<double>();
            }
            else
            {
                // SQL Server or other providers: keep proper precision
                modelBuilder.Entity<StockItem>()
                            .Property(p => p.Price)
                            .HasPrecision(18, 2);

                // If present:
                // modelBuilder.Entity<Order>()
                //             .Property(p => p.Total)
                //             .HasPrecision(18, 2);
                // modelBuilder.Entity<OrderLine>()
                //             .Property(p => p.Price)
                //             .HasPrecision(18, 2);
            }
        }
    }
}
