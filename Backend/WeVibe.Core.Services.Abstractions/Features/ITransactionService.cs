using WeVibe.Core.Contracts.Transaction;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface ITransactionService
    {
        Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto);
    }
}
