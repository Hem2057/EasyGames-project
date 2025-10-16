using EasyGames.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<StockItem> StockItems { get; set; } = default!;
        public DbSet<Shop> Shops { get; set; } = default!;
        public DbSet<ShopStock> ShopStocks { get; set; } = default!;
        public DbSet<Sale> Sales { get; set; } = default!;
        public DbSet<SaleLine> SaleLines { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (Database.ProviderName?.Contains("Sqlite") == true)
            {
                modelBuilder.Entity<StockItem>().Property(p => p.Price).HasConversion<double>();
                modelBuilder.Entity<StockItem>().Property(p => p.BuyPrice).HasConversion<double>();
                modelBuilder.Entity<StockItem>().Property(p => p.SellPrice).HasConversion<double>();
            }
            else
            {
                modelBuilder.Entity<StockItem>().Property(p => p.Price).HasPrecision(18, 2);
                modelBuilder.Entity<StockItem>().Property(p => p.BuyPrice).HasPrecision(18, 2);
                modelBuilder.Entity<StockItem>().Property(p => p.SellPrice).HasPrecision(18, 2);
            }

            modelBuilder.Entity<ShopStock>()
                .HasOne(ss => ss.Shop)
                .WithMany(s => s.ShopStocks)
                .HasForeignKey(ss => ss.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleLine>()
                .HasOne(l => l.Sale)
                .WithMany(s => s.Lines)
                .HasForeignKey(l => l.SaleId);
        }
    }
}
