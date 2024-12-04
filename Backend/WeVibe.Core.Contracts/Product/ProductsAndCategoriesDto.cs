using WeVibe.Core.Contracts.Category;

namespace WeVibe.Core.Contracts.Product
{
    public class ProductsAndCategoriesDto
    {
        public IEnumerable<ProductDto> Products { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
    }

}
