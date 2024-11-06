using Microsoft.AspNetCore.Mvc;
using WeVibe.Core.Contracts.ProductVariant;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductVariantController : Controller
    {
        private readonly IProductVariantService _productVariantService;
        public ProductVariantController(IProductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductVariant([FromBody] CreateProductVariantDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productVariantService.CreateAsync(createDto);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductVariantById(int id)
        {
            var result = await _productVariantService.GetProductVariantByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductVariants()
        {
            var result = await _productVariantService.GetAllProductVariantsAsync();
            return Ok(result);
        }

        [HttpPut("{productVariantId}")]
        public async Task<IActionResult> UpdateProductVariant(int productVariantId, [FromBody] UpdateProductVariantDto updateDto)
        {
            try
            {
                var updatedProductVariant = await _productVariantService.UpdateProductVariantAsync(productVariantId, updateDto);
                return Ok(updatedProductVariant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{productVariantId}")]
        public async Task<IActionResult> DeleteProductVariantById(int productVariantId)
        {
            try
            {
                var isDeleted = await _productVariantService.DeleteProductVariantByIdAsync(productVariantId);
                if (isDeleted)
                {
                    return NoContent();
                }

                return NotFound("Product variant not found");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
