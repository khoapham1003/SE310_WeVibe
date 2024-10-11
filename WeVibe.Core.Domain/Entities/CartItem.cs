using WeVibe.Core.Domain.Common;

namespace WeVibe.Core.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public int ProductVariantId { get; set; }
        // Navigation property
        public Cart Cart { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
