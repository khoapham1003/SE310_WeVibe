namespace WeVibe.Core.Contracts.Transaction
{
    public class CreateTransactionDto
    {
        public string PaymentMethod { get; set; }
        public string RecipientName { get; set; }
        public string Address { get; set; }
        public int OrderId { get; set; }
    }
}
