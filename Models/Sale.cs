using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyGames.Models
{
    public class Sale
    {
        public int Id { get; set; }

        // Buyer
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // Which shop made the sale (null if from website)
        [ForeignKey("Shop")]
        public int? ShopId { get; set; }
        public Shop? Shop { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Sale details
        public List<SaleLine> Lines { get; set; } = new();

        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }
    }
}
