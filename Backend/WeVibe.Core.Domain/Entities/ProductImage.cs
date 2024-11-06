using System.ComponentModel.DataAnnotations;

namespace WeVibe.Core.Domain.Entities
{
    public class ProductImage
    {
        [Key]
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public Product Product { get; set; }
    }
}
