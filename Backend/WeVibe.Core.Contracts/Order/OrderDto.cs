namespace WeVibe.Core.Contracts.Order
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string RecipientName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public string AddressValue { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
