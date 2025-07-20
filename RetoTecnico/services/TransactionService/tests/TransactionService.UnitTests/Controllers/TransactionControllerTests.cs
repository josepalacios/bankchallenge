using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using TransactionService.API.Controllers;
using TransactionService.Application.Commands;
using TransactionService.Application.Events;
using TransactionService.Application.Interfaces;
using TransactionService.Domain.Interfaces;

namespace TransactionService.UnitTests.Controllers;

public class TransactionControllerTests
{
    private readonly Mock<ITransactionService> _transactionServiceMock;
    private readonly TransactionController _controller;

    public TransactionControllerTests()
    {
        _transactionServiceMock = new Mock<ITransactionService>();

        _controller = new TransactionController(_transactionServiceMock.Object);
    }

    [Fact]
    public async Task CreateTransaction_ReturnsOk_WithTransactionId()
    {
        // Arrange
        var command = new SubmitTransactionCommand
        {
            SourceAccountId = Guid.NewGuid(),
            TargetAccountId = Guid.NewGuid(),
            TransferTypeId = 1,
            Value = 150.75m
        };

        var generatedId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        _transactionServiceMock
            .Setup(s => s.SubmitTransactionAsync(It.IsAny<SubmitTransactionCommand>()))
            .ReturnsAsync(generatedId);

        // Act
        var result = await _controller.CreateTransaction(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        // Serialize and deserialize to access anonymous object as a strong type
        var json = JsonConvert.SerializeObject(okResult.Value);
        var response = JsonConvert.DeserializeObject<CreateTransactionResponseDto>(json);

        Assert.NotNull(response);
        Assert.Equal(generatedId, response!.TransactionExternalId);
        Assert.True(response.CreatedAt <= DateTime.UtcNow);
    }
}

public class CreateTransactionResponseDto
{
    public Guid TransactionExternalId { get; set; }
    public DateTime CreatedAt { get; set; }
}

