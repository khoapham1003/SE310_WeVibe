using AutoMapper;
using WeVibe.Core.Contracts.Product;
using WeVibe.Core.Contracts.ProductVariant;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.Core.Services.Features
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        public ProductVariantService(
            IProductVariantRepository productVariantRepository,
            IProductRepository productRepository,
            IColorRepository colorRepository,
            ISizeRepository sizeRepository,
            IMapper mapper,
            IProductService productService)
        {
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _mapper = mapper;
            _productService = productService;
        }
        public async Task<ProductVariantDto> CreateAsync(CreateProductVariantDto createDto)
        {
            var product = await _productRepository.GetByIdAsync(createDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            var size = new Size
            {
                Name = createDto.SizeName
            };
            await _sizeRepository.AddAsync(size);
            await _sizeRepository.SaveAsync();

            var color = new Color
            {
                Name = createDto.ColorName,
                Hex = createDto.ColorHex
            };
            await _colorRepository.AddAsync(color);
            await _colorRepository.SaveAsync();

            var productVariant = new ProductVariant
            {
                ProductId = createDto.ProductId,
                SizeId = size.SizeId,
                ColorId = color.ColorId,
                Quantity = createDto.Quantity
            };

            await _productVariantRepository.AddAsync(productVariant);
            await _productVariantRepository.SaveAsync();

            return _mapper.Map<ProductVariantDto>(productVariant);
        }
        public async Task<ProductVariantDto> GetProductVariantByIdAsync(int id)
        {
            var productVariant = await _productVariantRepository.GetProductVariantByIdAsync(id);

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

            var productVariants = await _productVariantRepository.GetAllProductVariantsAsync();
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
            var productVariant = await _productVariantRepository.GetProductVariantByIdAsync(id);

            if (productVariant == null)
            {
                throw new KeyNotFoundException("Product variant not found");
            }

            var size = await _sizeRepository.GetByIdAsync(productVariant.SizeId);

            var color = await _colorRepository.GetByIdAsync(productVariant.ColorId);

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

            await _sizeRepository.UpdateAsync(size);
            await _colorRepository.UpdateAsync(color);
            await _productVariantRepository.UpdateAsync(productVariant);

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
            var productVariant = await _productVariantRepository.GetProductVariantByIdAsync(id);

            if (productVariant == null)
            {
                throw new KeyNotFoundException("Product variant not found");
            }

            var size = await _sizeRepository.GetByIdAsync(productVariant.SizeId);
            var color = await _colorRepository.GetByIdAsync(productVariant.ColorId);

            await _productVariantRepository.DeleteAsync(productVariant.ProductId);

            await _sizeRepository.DeleteAsync(size.SizeId);
            await _colorRepository.DeleteAsync(color.ColorId);

            return true;
        }

    }
}
