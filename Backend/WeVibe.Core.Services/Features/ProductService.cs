using AutoMapper;
using WeVibe.Core.Contracts.Product;
using WeVibe.Core.Contracts.ProductVariant;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Core.Services.Abstractions.Features;
using WeVibe.Core.Services.Exceptions;

namespace WeVibe.Core.Services.Features
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IMapper _mapper;
        private readonly string _imageFolderPath = "wwwroot/images/products";
        public ProductService(IProductRepository productRepository, IProductImageRepository productImageRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllWithCategoryAsync();

            return products.Select(product => new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Slug = product.Slug,
                Images = _mapper.Map<ICollection<ProductImageDto>>(product.Images),
                Description = product.Description,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            });
        }
        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

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

            await _productRepository.AddAsync(product);
            await _productRepository.SaveAsync();

            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> UpdateProductAsync(int productId, UpdateProductDto updateProductDto)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {productId} not found.");
            }

            product.Name = updateProductDto.Name;
            product.Slug = updateProductDto.Slug;
            product.Description = updateProductDto.Description;
            product.Quantity = updateProductDto.Quantity;
            product.CategoryId = updateProductDto.CategoryId;

            if (updateProductDto.ImagesToRemove != null && updateProductDto.ImagesToRemove.Any())
            {
                var imagesToRemove = product.Images.Where(img => updateProductDto.ImagesToRemove.Contains(img.ProductImageId)).ToList();
                foreach (var image in imagesToRemove)
                {
                    await _productImageRepository.DeleteAsync(image.ProductImageId);
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

            await _productRepository.SaveAsync();

            return _mapper.Map<ProductDto>(product);
        }
        public async Task<string> DeleteProductAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

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

            await _productRepository.DeleteAsync(productId);
            await _productRepository.SaveAsync();

            return $"Product with ID {productId} has been successfully deleted.";
        }
        public async Task<ProductDetailDto> GetProductDetailByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductDetailAsync(productId);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {productId} not found.");
            }

            var productDetailDto = new ProductDetailDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                Images = product.Images.Select(img => new ProductImageDto
                {
                    ProductImageId = img.ProductImageId,
                    ProductId = img.ProductId,
                    ImagePath = img.ImagePath
                }).ToList(),
                ProductVariants = product.ProductVariants.Select(pv => new ProductVariantDto
                {
                    ProductVariantId = pv.ProductVariantId,
                    ProductId = pv.ProductId,
                    SizeName = pv.Size.Name,
                    ColorName = pv.Color.Name,
                    ColorHex = pv.Color.Hex,
                    Price = pv.Price,
                    Quantity = pv.Quantity
                }).ToList()
            };

            return productDetailDto;
        }
        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);

            if (!products.Any())
            {
                throw new NotFoundException($"No products found for Category ID {categoryId}.");
            }

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

    }
}
