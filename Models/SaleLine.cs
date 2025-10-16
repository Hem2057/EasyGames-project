using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyGames.Models
{
    public class SaleLine
    {
        public int Id { get; set; }

        [ForeignKey("Sale")]
        public int SaleId { get; set; }
        public Sale? Sale { get; set; }

        [ForeignKey("StockItem")]
        public int StockItemId { get; set; }
        public StockItem? StockItem { get; set; }

        [Range(1, 999)]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }
    }
}
