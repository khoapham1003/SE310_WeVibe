using AutoMapper;
using WeVibe.Core.Contracts.Cart;
using WeVibe.Core.Contracts.Category;
using WeVibe.Core.Contracts.Product;
using WeVibe.Core.Contracts.ProductVariant;
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
            //ProductVariant Mapping Profile
            CreateMap<ProductVariant, ProductVariantDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.ColorHex, opt => opt.MapFrom(src => src.Color.Hex));
            //Cart Mapping Profile
            CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<CartItem, CartItemDto>();

            CreateMap<AddToCartDto, CartItem>()
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Discount, opt => opt.Ignore());

            CreateMap<UpdateCartItemDto, CartItem>()
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore());
        }
    }
}
