namespace WeVibe.Core.Contracts.Cart
{
    public class CartDto
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public decimal TotalPrice => CartItems.Sum(item => (item.UnitPrice - item.Discount) * item.Quantity);
    }
}
