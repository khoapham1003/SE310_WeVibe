using System.ComponentModel.DataAnnotations;
using WeVibe.Core.Domain.Common;

namespace WeVibe.Core.Domain.Entities
{
    public class Discount : BaseEntity
    {
        public int DiscountId { get; set; }
        public required string Name { get; set; }
        [Range(0, 100)]
        public decimal Percentage { get; set; }
        public DateTime Duration { get; set; }
        public ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();
    }
}
