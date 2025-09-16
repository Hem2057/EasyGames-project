namespace EasyGames.Models
{
    public class CartItem
    {
        public int StockItemId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Qty { get; set; }

        // i added this so the cart view can show category labels
        public string Category { get; set; } = "";
    }
}
