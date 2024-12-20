using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeVibe.Core.Contracts.Product
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<IFormFile> ImagesToAdd { get; set; } = new List<IFormFile>();
        public ICollection<int> ImagesToRemove { get; set; } = new List<int>();
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}
