using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        [HttpGet("products-and-categories")]
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
    }
}
