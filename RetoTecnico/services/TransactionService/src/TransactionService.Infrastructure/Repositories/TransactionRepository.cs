namespace TransactionService.Infrastructure.Repositories;

using TransactionService.Domain.Interfaces;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository : ITransactionRepository
{
    private readonly TransactionDbContext _context;

    public TransactionRepository(TransactionDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(Guid transactionId, TransactionStatus status)
    {
        var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionExternalId == transactionId);
        if (transaction != null)
        {
            transaction.Status = status;
            await _context.SaveChangesAsync();
        }
    }

    //public Task<List<Transaction>> GetBySourceAccountAndDateAsync(Guid sourceAccountId, DateTime date)
    //{
    //    var sameDay = _context.Transactions.Where(t =>
    //        t.SourceAccountId == sourceAccountId &&
    //        t.CreatedAt.Date == date.Date).ToList();

    //    return Task.FromResult(sameDay);
    //}

    public async Task<List<Transaction>> GetBySourceAccountAndDateAsync(Guid sourceAccountId, DateTime date)
    {
        var startOfDay = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        var endOfDay = startOfDay.AddDays(1);

        return await _context.Transactions
            .Where(t => t.SourceAccountId == sourceAccountId &&
                        t.CreatedAt >= startOfDay &&
                        t.CreatedAt < endOfDay)
            .ToListAsync();
    }
}