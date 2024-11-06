namespace WeVibe.Core.Domain.Entities
{
    public class Color
    {
        public int ColorId { get; set; }
        public required string Name { get; set; }
        public string Hex { get; set; }
        public ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
