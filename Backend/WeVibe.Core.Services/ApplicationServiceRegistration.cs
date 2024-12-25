using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeVibe.Core.Services.Abstractions.Features;
using WeVibe.Core.Services.Features;
using WeVibe.Core.Services.Mapper;

namespace WeVibe.Core.Services
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductVariantService, ProductVariantService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
