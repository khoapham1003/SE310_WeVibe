namespace WeVibe.Core.Domain.Entities
{
    public class ProductDiscount
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
