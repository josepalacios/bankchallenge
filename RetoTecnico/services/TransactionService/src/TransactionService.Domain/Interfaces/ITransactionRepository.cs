using TransactionService.Domain.Entities;
using TransactionService.Domain.Enums;

namespace TransactionService.Domain.Interfaces;

public interface ITransactionRepository
{
    Task CreateAsync(Transaction transaction);
    Task UpdateStatusAsync(Guid transactionId, TransactionStatus status);
    Task<List<Transaction>> GetBySourceAccountAndDateAsync(Guid sourceAccountId, DateTime date);
}
