using AutoMapper;
using WeVibe.Core.Contracts.Transaction;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.Core.Services.Features
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto)
        {
            var transaction = _mapper.Map<Transaction>(createTransactionDto);
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveAsync();

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
