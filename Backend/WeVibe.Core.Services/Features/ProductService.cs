using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeVibe.Core.Contracts.Product;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Services.Abstractions.Features;
using WeVibe.Core.Services.Exceptions;
using WeVibe.Infrastructure.Persistence.DataContext;

namespace WeVibe.Core.Services.Features
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _imageFolderPath = "wwwroot/images/products";
        public ProductService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Images)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> AddProductAsync(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);

            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }

            product.Images = product.Images ?? new List<ProductImage>();

            foreach (var image in createProductDto.Images)
            {
                if (image != null && image.Length > 0)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
                    var filePath = Path.Combine(_imageFolderPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    product.Images.Add(new ProductImage { ImagePath = $"/images/products/{uniqueFileName}" });
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> UpdateProductAsync(int productId, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {productId} not found.");
            }

            product.Name = updateProductDto.Name;
            product.Slug = updateProductDto.Slug;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.Quantity = updateProductDto.Quantity;
            product.CategoryId = updateProductDto.CategoryId;

            if (updateProductDto.ImagesToRemove != null && updateProductDto.ImagesToRemove.Any())
            {
                var imagesToRemove = product.Images.Where(img => updateProductDto.ImagesToRemove.Contains(img.ProductImageId)).ToList();
                foreach (var image in imagesToRemove)
                {
                    _context.ProductImages.Remove(image);
                }
            }

            if (updateProductDto.ImagesToAdd != null && updateProductDto.ImagesToAdd.Any())
            {
                foreach (var image in updateProductDto.ImagesToAdd)
                {
                    if (image != null && image.Length > 0)
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
                        var filePath = Path.Combine(_imageFolderPath, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        product.Images.Add(new ProductImage { ImagePath = $"/images/products/{uniqueFileName}" });
                    }
                }
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }
        public async Task<string> DeleteProductAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            foreach (var image in product.Images)
            {
                var filePath = Path.Combine(_imageFolderPath, image.ImagePath.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return $"Product with ID {productId} has been successfully deleted.";
        }
    }
}
