namespace WeVibe.Core.Contracts.Transaction
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PayAmount { get; set; }
        public int OrderId { get; set; }
    }

}
