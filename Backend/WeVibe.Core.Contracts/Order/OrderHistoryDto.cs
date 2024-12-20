namespace WeVibe.Core.Contracts.Order
{
    public class OrderHistoryDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public string AddressValue { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PayAmount { get; set; }
        public string TransactionStatus { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

}
