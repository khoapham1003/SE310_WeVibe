using Microsoft.AspNetCore.Mvc;
using WeVibe.Core.Contracts.Category;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _categoryService.AddCategoryAsync(request);

            if (category == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating category");
            }

            return Ok("New Category created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid data."); // Return a Bad Request if the DTO is null
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(id, request);
                return Ok("Category updated successfully."); // Return Ok with a success message
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Category with ID {id} not found."); // Return NotFound if the category doesn't exist
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 for any other exceptions
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
