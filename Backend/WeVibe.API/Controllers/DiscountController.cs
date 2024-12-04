using Microsoft.AspNetCore.Mvc;
using WeVibe.Core.Contracts.Discount;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Services.Abstractions.Features;
using WeVibe.Core.Services.Features;

namespace WeVibe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountDto createDiscountDto)
        {
            try
            {
                var createdDiscount = await _discountService.CreateDiscountAsync(createDiscountDto);
                return Ok(createdDiscount);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscountById(int id)
        {
            try
            {
                var discount = await _discountService.GetDiscountByIdAsync(id);
                return Ok(discount);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiscounts()
        {
            try
            {
                var discounts = await _discountService.GetAllDiscountsAsync();
                return Ok(discounts);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DiscountDto discountDto)
        {
            try
            {
                var updatedDiscount = await _discountService.UpdateDiscountAsync(id, discountDto);
                return Ok(updatedDiscount);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }          
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _discountService.DeleteAsync(id);
                return Ok(success);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }            
        }
    }
}
