using WeVibe.Core.Contracts.Category;
using WeVibe.Core.Domain.Entities;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<CategoryDto> AddCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<string> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(int id);
    }
}
