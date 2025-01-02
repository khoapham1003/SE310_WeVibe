using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WeVibe.Core.Contracts.Product;
using WeVibe.Core.Services.Abstractions.Features;
using WeVibe.Core.Services.Exceptions;

namespace WeVibe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet("all-products")]
        [SwaggerOperation(Summary = "Get all products", Description = "Use in admin page")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        [HttpGet("products-and-categories")]
        [SwaggerOperation(Summary = "Get all Products and Categories", Description = "Use in home page")]
        public async Task<IActionResult> GetAllProductsAndCategories()
        {
            var products = await _productService.GetAllProductsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();
            var responseDto = new ProductsAndCategoriesDto
            {
                Products = products,
                Categories = categories
            };

            return Ok(responseDto);
        }

        [HttpGet("{productId}")]
        [SwaggerOperation(Summary = "Get Product By Id", Description = "Use to find product in admin page")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(productId);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create Product", Description = "Use to create product in admin page")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProductDto)
        {
            if (createProductDto.Images == null || createProductDto.Images.Count == 0)
            {
                return BadRequest("Product images are required.");
            }

            var product = await _productService.AddProductAsync(createProductDto);

            if (product == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating product.");
            }

            return Ok(product);
        }
        [HttpPut("{productId}")]
        [SwaggerOperation(Summary = "Update product", Description = "")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromForm] UpdateProductDto updateProductDto)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(productId, updateProductDto);
                return Ok(updatedProduct);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                var successMessage = await _productService.DeleteProductAsync(productId);
                return Ok(successMessage);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("product-detail-{productId}")]
        [SwaggerOperation(Summary = "Product Detail with Variant", Description = "Show Product Detail and related ProductVariant List")]
        public async Task<IActionResult> ProductDetail(int productId)
        {
            try
            {
                var product = await _productService.GetProductDetailByIdAsync(productId);
                return Ok(product);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("category/{categoryId}")]
        [SwaggerOperation(Summary = "Get products by category",Description = "Retrieve a list of products filtered by a specific category.")]
        [SwaggerResponse(200, "List of products for the given category", typeof(IEnumerable<ProductDto>))]
        [SwaggerResponse(404, "No products found for the given category")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryAsync(categoryId);
                return Ok(products);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search products by name", Description = "Retrieve all products containing the given letters in their name.")]
        public async Task<IActionResult> SearchProductsByName([FromQuery] string searchString)
        {
            try
            {
                var products = await _productService.SearchProductsByNameAsync(searchString);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
