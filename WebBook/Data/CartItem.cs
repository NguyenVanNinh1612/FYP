namespace WebBook.Data
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; } 
        public byte Discount { get; set; }
        //public decimal PriceSale { get; set; }
        public int MaxQuantity { get; set; }
        public int Quantity { get; set; }
        // public decimal TotalPrice => PriceSale > 0 ? PriceSale * Quantity : Price * Quantity;
        public decimal TotalPrice => (Price - (Price * (decimal)0.01 * Discount))*Quantity;
    }
}
