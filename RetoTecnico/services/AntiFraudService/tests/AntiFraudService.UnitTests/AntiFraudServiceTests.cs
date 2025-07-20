namespace AntiFraudService.UnitTests;

using AntiFraudService.Application.Events;
using AntiFraudService.Domain.Enums;
using AntiFraudService.Domain.Interfaces;
using AntiFraudService.Domain.Models;
using FluentAssertions;
using Moq;
using Xunit;

public class AntiFraudServiceTests
{
    [Fact]
    public async Task ValidateTransaction_ShouldCallEvaluatorAndReturnStatus()
    {
        var mockEvaluator = new Mock<IAntiFraudEvaluator>();
        mockEvaluator
            .Setup(x => x.EvaluateAsync(It.IsAny<AntiFraudEvaluationInput>()))
            .ReturnsAsync(TransactionStatus.Approved);

        var service = new AntiFraudService.Application.Services.AntiFraudService(mockEvaluator.Object);

        var evt = new TransactionSubmittedEvent
        {
            SourceAccountId = Guid.NewGuid(),
            Value = 1200,
            TransactionId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        var result = await service.ValidateTransactionAsync(evt);

        result.Should().Be(TransactionStatus.Approved);
        mockEvaluator.Verify(x => x.EvaluateAsync(It.IsAny<AntiFraudEvaluationInput>()), Times.Once);
    }
}
