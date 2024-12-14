using Microsoft.AspNetCore.Mvc;
using WeVibe.Core.Services.Abstractions.Features;
using WeVibe.Core.Services.Features;

namespace WeVibe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductVariantService _productVariantService;
        public HomeController(IProductService productService, IProductVariantService productVariantService)
        {
            _productService = productService;
            _productVariantService = productVariantService;
        }
    }
}
