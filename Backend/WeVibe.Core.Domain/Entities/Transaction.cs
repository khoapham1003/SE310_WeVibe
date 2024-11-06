using WeVibe.Core.Domain.Common;

namespace WeVibe.Core.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PayAmount { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
