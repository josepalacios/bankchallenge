using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Repositories;
using Xunit;

namespace TransactionService.UnitTests.Infrastructure;

public class TransactionRepositoryTests : IDisposable
{
    private readonly TransactionDbContext _context;
    private readonly TransactionRepository _repository;

    public TransactionRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TransactionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TransactionDbContext(options);
        _repository = new TransactionRepository(_context);
    }

    [Fact]
    public async Task SaveAsync_AddsTransactionToDatabase()
    {
        // Arrange
        var transaction = new Transaction
        {
            TransactionExternalId = Guid.NewGuid(),
            SourceAccountId = Guid.NewGuid(),
            TargetAccountId = Guid.NewGuid(),
            TransferTypeId = 1,
            Value = 500m,
            Status = Domain.Enums.TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await _repository.CreateAsync(transaction);

        // Assert
        var storedTransaction = await _context.Transactions.FindAsync(transaction.TransactionExternalId);
        Assert.NotNull(storedTransaction);
        Assert.Equal(transaction.Value, storedTransaction.Value);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
