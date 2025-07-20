using TransactionService.Application.Commands;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Interfaces;

public interface ITransactionService
{
    Task<Guid> SubmitTransactionAsync(SubmitTransactionCommand command);
    Task<List<Transaction>> GetBySourceAccountAndDateAsync(Guid sourceAccountId, DateTime date);
}
