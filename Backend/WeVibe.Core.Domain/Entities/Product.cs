using WeVibe.Core.Domain.Common;

namespace WeVibe.Core.Domain.Entities
{
    public class Product : BaseEntity
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public string? Slug { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductVariant> ProductVariants { get; set; }
        public ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();
    }
}
