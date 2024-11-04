using AutoMapper;
using WeVibe.Core.Contracts.Category;
using WeVibe.Core.Domain.Entities;

namespace WeVibe.Core.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CreateCategoryDto>()
                .ReverseMap();

            CreateMap<Category, UpdateCategoryDto>()
                .ReverseMap();
        }
    }
}
