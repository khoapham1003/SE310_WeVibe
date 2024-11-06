using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeVibe.Core.Contracts.Product;
using WeVibe.Core.Contracts.ProductVariant;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Services.Abstractions.Features;
using WeVibe.Infrastructure.Persistence.DataContext;

namespace WeVibe.Core.Services.Features
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        public ProductVariantService(ApplicationDbContext context, IMapper mapper, IProductService productService)
        {
            _context = context;
            _mapper = mapper;
            _productService = productService;
        }
        public async Task<ProductVariantDto> CreateAsync(CreateProductVariantDto createDto)
        {
            var product = await _context.Products.FindAsync(createDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            var size = new Size
            {
                Name = createDto.SizeName
            };
            _context.Sizes.Add(size);

            var color = new Color
            {
                Name = createDto.ColorName,
                Hex = createDto.ColorHex
            };
            _context.Colors.Add(color);

            await _context.SaveChangesAsync();

            var productVariant = new ProductVariant
            {
                ProductId = createDto.ProductId,
                SizeId = size.SizeId,
                ColorId = color.ColorId,
                Quantity = createDto.Quantity
            };

            _context.ProductVariants.Add(productVariant);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductVariantDto>(productVariant);
        }
        public async Task<ProductVariantDto> GetProductVariantByIdAsync(int id)
        {
            var productVariant = await _context.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Size)
                .Include(pv => pv.Color)
                .FirstOrDefaultAsync(pv => pv.ProductVariantId == id);

            if (productVariant == null)
                throw new KeyNotFoundException("Product variant not found");

            var product = await _productService.GetProductByIdAsync(productVariant.ProductId);

            var productVariantDto = _mapper.Map<ProductVariantDto>(productVariant);

            productVariantDto.Product = _mapper.Map<ProductDto>(product);

            return productVariantDto;
        }
        public async Task<IEnumerable<ProductVariantDto>> GetAllProductVariantsAsync()
        {
            var productVariantDtos = new List<ProductVariantDto>();

            var productVariants = await _context.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Size)
                .Include(pv => pv.Color)
                .ToListAsync();
            foreach (var variant in productVariants)
            {
                var product = await _productService.GetProductByIdAsync(variant.ProductId);
                var productVariantDto = _mapper.Map<ProductVariantDto>(variant);

                productVariantDto.Product = _mapper.Map<ProductDto>(product);
                productVariantDtos.Add(productVariantDto);
            }

            return productVariantDtos;
        }
        public async Task<ProductVariantDto> UpdateProductVariantAsync(int id, UpdateProductVariantDto updateDto)
        {
            var productVariant = await _context.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Size)
                .Include(pv => pv.Color)
                .FirstOrDefaultAsync(pv => pv.ProductVariantId == id);

            if (productVariant == null)
            {
                throw new KeyNotFoundException("Product variant not found");
            }

            var size = await _context.Sizes
                .FirstOrDefaultAsync(s => s.SizeId == productVariant.SizeId);

            var color = await _context.Colors
                .FirstOrDefaultAsync(c => c.ColorId == productVariant.ColorId);

            if (size == null)
            {
                throw new KeyNotFoundException("Size not found");
            }

            if (color == null)
            {
                throw new KeyNotFoundException("Color not found");
            }

            size.Name = updateDto.SizeName;
            color.Name = updateDto.ColorName;
            color.Hex = updateDto.ColorHex;

            productVariant.Quantity = updateDto.Quantity;

            _context.Sizes.Update(size);
            _context.Colors.Update(color);
            _context.ProductVariants.Update(productVariant);

            await _context.SaveChangesAsync();

            var updatedProduct = await _productService.GetProductByIdAsync(productVariant.ProductId);
            if (updatedProduct == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            var productVariantDto = _mapper.Map<ProductVariantDto>(productVariant);
            productVariantDto.Product = _mapper.Map<ProductDto>(updatedProduct);

            return productVariantDto;
        }
        public async Task<bool> DeleteProductVariantByIdAsync(int id)
        {
            var productVariant = await _context.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Size)
                .Include(pv => pv.Color)
                .FirstOrDefaultAsync(pv => pv.ProductVariantId == id);

            if (productVariant == null)
            {
                throw new KeyNotFoundException("Product variant not found");
            }

            var size = await _context.Sizes.FirstOrDefaultAsync(s => s.SizeId == productVariant.SizeId);
            var color = await _context.Colors.FirstOrDefaultAsync(c => c.ColorId == productVariant.ColorId);

            _context.ProductVariants.Remove(productVariant);

            _context.Sizes.Remove(size);
            _context.Colors.Remove(color);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

    }
}
