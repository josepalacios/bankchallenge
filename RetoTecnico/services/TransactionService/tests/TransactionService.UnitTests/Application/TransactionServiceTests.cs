using Moq;
using TransactionService.Application.Commands;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Enums;
using TransactionService.Domain.Interfaces;
using Xunit;

namespace TransactionService.UnitTests.Application;

public class TransactionServiceTests
{
    private readonly Mock<ITransactionRepository> _mockRepository;
    private readonly Mock<IKafkaProducer> _mockKafkaProducer;
    private readonly TransactionService.Application.Services.TransactionService _service;

    public TransactionServiceTests()
    {
        _mockRepository = new Mock<ITransactionRepository>();
        _mockKafkaProducer = new Mock<IKafkaProducer>();
        _service = new TransactionService.Application.Services.TransactionService(_mockRepository.Object, _mockKafkaProducer.Object);
    }

    [Fact]
    public async Task CreateTransactionAsync_SavesTransaction_And_ProducesKafkaEvent()
    {
        // Arrange
        var command = new SubmitTransactionCommand
        {
            SourceAccountId = Guid.NewGuid(),
            TargetAccountId = Guid.NewGuid(),
            TransferTypeId = 2,
            Value = 1000m
        };

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        _mockKafkaProducer.Setup(k => k.ProduceAsync(It.IsAny<string>(), It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.SubmitTransactionAsync(command);

        // Assert
        _mockRepository.Verify(r => r.CreateAsync(It.Is<Transaction>(t =>
            t.SourceAccountId == command.SourceAccountId &&
            t.TargetAccountId == command.TargetAccountId &&
            t.TransferTypeId == command.TransferTypeId &&
            t.Value == command.Value &&
            t.Status == TransactionStatus.Pending)), Times.Once);

        _mockKafkaProducer.Verify(k => k.ProduceAsync("transaction-submitted", It.IsAny<object>()), Times.Once);

        Assert.NotEqual(Guid.Empty, result);
    }
}
