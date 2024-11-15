using System.ComponentModel.DataAnnotations;
using WeVibe.Core.Domain.Common;

namespace WeVibe.Core.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        [Key]
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public Product Product { get; set; }
    }
}
