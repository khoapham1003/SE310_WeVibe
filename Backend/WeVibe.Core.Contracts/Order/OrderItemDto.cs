using WeVibe.Core.Contracts.Product;

namespace WeVibe.Core.Contracts.Order
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductVariantId { get; set; }
        public ProductDto Product { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
    }
}
