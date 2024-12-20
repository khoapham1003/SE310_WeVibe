using WeVibe.Core.Contracts.Product;

namespace WeVibe.Core.Contracts.ProductVariant
{
    public class ProductVariantDto
    {
        public int ProductVariantId { get; set; }
        public ProductDto Product { get; set; }
        public int ProductId { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
