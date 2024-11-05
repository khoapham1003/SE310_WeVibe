using AutoMapper;
using WeVibe.Core.Contracts.Category;
using WeVibe.Core.Contracts.Product;
using WeVibe.Core.Domain.Entities;

namespace WeVibe.Core.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Category Mapping Profile
            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CreateCategoryDto>()
                .ReverseMap();

            CreateMap<Category, UpdateCategoryDto>()
                .ReverseMap();
            //Product Mapping Profile
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            CreateMap<ProductImage, ProductImageDto>();

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
        }
    }
}
