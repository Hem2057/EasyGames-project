using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyGames.Models
{
    public class ShopStock
    {
        public int Id { get; set; }

        [ForeignKey("Shop")]
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }

        [ForeignKey("StockItem")]
        public int StockItemId { get; set; }
        public StockItem? StockItem { get; set; }

        [Range(0, 9999)]
        public int Quantity { get; set; }
    }
}
