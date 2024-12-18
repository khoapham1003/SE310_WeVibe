using AutoMapper;
using WeVibe.Core.Contracts.Cart;
using WeVibe.Core.Contracts.Category;
using WeVibe.Core.Contracts.Discount;
using WeVibe.Core.Contracts.Order;
using WeVibe.Core.Contracts.Product;
using WeVibe.Core.Contracts.ProductVariant;
using WeVibe.Core.Contracts.Transaction;
using WeVibe.Core.Domain.Entities;

namespace WeVibe.Core.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Category Mapping Profile
            CreateMap<Category, CategoryDto>()
                .ReverseMap();
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
            //Discount Mapping Profile
            CreateMap<CreateDiscountDto, Discount>();

            CreateMap<Discount, DiscountDto>().ReverseMap();
            //Transaction Mapping Profile
            CreateMap<Transaction, TransactionDto>().ReverseMap();

            CreateMap<Transaction, CreateTransactionDto>().ReverseMap();
            //Order Mapping Profile
            CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            // Map OrderItem to OrderItemDto
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.ProductVariant.Size.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.ProductVariant.Color.Name));

            // Map Order to OrderHistoryDto
            CreateMap<Order, OrderHistoryDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Transaction.PaymentMethod))
                .ForMember(dest => dest.PayAmount, opt => opt.MapFrom(src => src.Transaction.PayAmount))
                .ForMember(dest => dest.TransactionStatus, opt => opt.MapFrom(src => src.Transaction.Status));
        }
    }
}
