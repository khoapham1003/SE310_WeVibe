using WeVibe.Core.Domain.Common;

namespace WeVibe.Core.Domain.Entities
{
    public class ProductVariant : BaseEntity
    {
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        //Navigation Properties
        public Product Product { get; set; }
        public Size Size { get; set; }
        public Color Color { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
