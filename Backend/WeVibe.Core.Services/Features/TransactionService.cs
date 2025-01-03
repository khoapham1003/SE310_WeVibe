using AutoMapper;
using WeVibe.Core.Contracts.Transaction;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Core.Services.Abstractions.Features;
using WeVibe.Infrastructure.Persistence.Repositories;

namespace WeVibe.Core.Services.Features
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository,
            IMapper mapper,
            IOrderRepository orderRepository,
            IProductVariantRepository productVariantRepository,
            IProductRepository productRepository)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
        }
        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto)
        {
            var transaction = _mapper.Map<Transaction>(createTransactionDto);

            var order = await _orderRepository.GetOrderWithTransactionAndItemsAsync(createTransactionDto.OrderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            order.AddressValue = createTransactionDto.Address;
            order.RecipientName = createTransactionDto.RecipientName;

            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveAsync();

            foreach (var orderItem in order.OrderItems)
            {
                var productVariant = await _productVariantRepository.GetByIdAsync(orderItem.ProductVariantId);
                if (productVariant == null)
                {
                    throw new KeyNotFoundException($"Product variant with ID {orderItem.ProductVariantId} not found.");
                }

                // Check stock availability
                if (productVariant.Quantity < orderItem.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for ProductVariant ID {productVariant.ProductVariantId}.");
                }

                var product = await _productRepository.GetByIdAsync(productVariant.ProductId);
                productVariant.Quantity -= orderItem.Quantity;
                product.Quantity -= orderItem.Quantity;
                await _productRepository.UpdateAsync(product);
                await _productVariantRepository.UpdateAsync(productVariant);
            }

            await _productVariantRepository.SaveAsync();
            var totalAmount = order.OrderItems.Sum(item => (item.UnitPrice * item.Quantity));
            transaction.PayAmount = totalAmount;
            if (createTransactionDto.PaymentMethod != "COD")
            {
                transaction.Status = "Paid";
            }
            else
            {
                transaction.Status = "Pending";
            }
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveAsync();

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}