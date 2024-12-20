using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WeVibe.Core.Contracts.Cart;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add item to cart", Description = "Use to add item to cart from ProductDetail page")]
        public async Task<IActionResult> AddToCartAsync([FromBody] AddToCartDto addToCartDto)
        {
            try
            {
                var updatedCart = await _cartService.AddToCartAsync(addToCartDto);

                if (updatedCart == null)
                {
                    return BadRequest("Failed to add item to cart");
                }

                return Ok(updatedCart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("update/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItemAsync(int cartItemId, [FromBody] UpdateCartItemDto updateDto)
        {
            try
            {
                var result = await _cartService.UpdateCartItemAsync(cartItemId, updateDto);
                if (result)
                {
                    var updatedCart = await _cartService.GetCartByUserIdAsync(updateDto.UserId);
                    return Ok(updatedCart);
                }

                return NotFound("Cart item not found");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItemAsync(int cartItemId)
        {
            try
            {
                var result = await _cartService.RemoveCartItemAsync(cartItemId);
                if (result)
                {
                    return Ok("Remove cart item successfully");
                }

                return NotFound("Cart item not found");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCartByUserIdAsync(string userId)
        {
            try
            {
                var cartDto = await _cartService.GetCartByUserIdAsync(userId);
                if (cartDto == null)
                {
                    return NotFound("Cart not found");
                }

                return Ok(cartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
