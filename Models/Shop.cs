using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasyGames.Models
{
    public class Shop
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Location { get; set; }

        // Navigation
        public List<ShopStock> ShopStocks { get; set; } = new();
    }
}
