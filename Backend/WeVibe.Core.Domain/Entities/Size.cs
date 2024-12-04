using WeVibe.Core.Domain.Common;

namespace WeVibe.Core.Domain.Entities
{
    public class Size : BaseEntity
    {
        public int SizeId { get; set; }
        public required string Name { get; set; }
        public ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
