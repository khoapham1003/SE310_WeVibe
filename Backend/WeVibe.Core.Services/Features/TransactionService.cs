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
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, 
            IMapper mapper,
            IOrderRepository orderRepository)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
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

            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveAsync();

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

            await _transactionRepository.SaveAsync();

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
