using System;
using System.Collections.Generic;
using System.Linq;
using WeVibe.Core.Domain.Common;

namespace WeVibe.Core.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public required string Status { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string AddressValue { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
