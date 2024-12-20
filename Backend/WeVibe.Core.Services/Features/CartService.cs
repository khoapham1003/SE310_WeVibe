using AutoMapper;
using WeVibe.Core.Contracts.Cart;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.Core.Services.Features
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IMapper _mapper;

        public CartService(
            ICartRepository cartRepository,
            IProductVariantRepository productVariantRepository,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productVariantRepository = productVariantRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> GetCartByUserIdAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null) return null;

            foreach (var cartItem in cart.CartItems)
            {
                var productVariant = await _productVariantRepository.GetByIdAsync(cartItem.ProductVariantId);
                if (productVariant != null)
                {
                    cartItem.UnitPrice = productVariant.Price;
                }
            }

            await _cartRepository.SaveAsync();

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> AddToCartAsync(AddToCartDto addToCartDto)
        {
            var cart = await _cartRepository.GetCartWithItemsByUserIdAsync(addToCartDto.UserId);

            if (cart == null)
            {
                cart = new Cart { UserId = addToCartDto.UserId };
                await _cartRepository.AddAsync(cart);
            }

            var productVariant = await _productVariantRepository.GetByIdAsync(addToCartDto.ProductVariantId);
            if (productVariant == null) throw new KeyNotFoundException("Product variant not found");

            var cartItem = _mapper.Map<CartItem>(addToCartDto);
            cartItem.UnitPrice = productVariant.Price;
            cartItem.Discount = addToCartDto.Discount;

            cart.CartItems.Add(cartItem);

            await _cartRepository.SaveAsync();
            return _mapper.Map<CartDto>(cart); 
        }
        public async Task<bool> UpdateCartItemAsync(int cartItemId, UpdateCartItemDto updateDto)
        {
            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found");
            }

            cartItem.Quantity = updateDto.Quantity;
            cartItem.Discount = updateDto.Discount;

            await _cartRepository.SaveAsync();
            return true;
        }
        public async Task<bool> RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found");
            }

            await _cartRepository.DeleteCartItemAsync(cartItemId);

            await _cartRepository.SaveAsync();
            return true;
        }
    }
}
