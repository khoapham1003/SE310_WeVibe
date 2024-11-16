using WeVibe.Core.Contracts.ProductVariant;

namespace WeVibe.Core.Contracts.Product
{
    public class ProductDetailDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<ProductVariantDto> ProductVariants { get; set; } = new List<ProductVariantDto>();
    }
}
