using System.ComponentModel.DataAnnotations;

namespace EasyGames.Models
{
    public class StockItem
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Range(0, 9999)]
        public decimal Price { get; set; }

        [Range(0, 9999)]
        public int Quantity { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // i added this so each item can display an image in catalog/details
        [StringLength(255)]
        public string? ImagePath { get; set; }
    }
}
