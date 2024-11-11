namespace WeVibe.Core.Contracts.Cart
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
    }
}
