namespace WeVibe.Core.Contracts.Cart
{
    public class UpdateCartItemDto
    {
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }
}
