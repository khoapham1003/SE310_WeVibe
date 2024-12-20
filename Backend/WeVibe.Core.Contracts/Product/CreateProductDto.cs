using Microsoft.AspNetCore.Http;

namespace WeVibe.Core.Contracts.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
