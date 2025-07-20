namespace AntiFraudService.UnitTests;

using FluentAssertions;
using AntiFraudService.Domain.Enums;
using AntiFraudService.Domain.Models;
using AntiFraudService.Domain.Services;

public class AntiFraudEvaluatorTests
{
    private readonly AntiFraudEvaluator _evaluator;

    public AntiFraudEvaluatorTests()
    {
        _evaluator = new AntiFraudEvaluator();
    }

    [Fact]
    public async Task EvaluateAsync_ShouldReject_WhenValueIsGreaterThan2000()
    {
        var input = new AntiFraudEvaluationInput
        {
            SourceAccountId = Guid.NewGuid(),
            Value = 2500
        };

        var result = await _evaluator.EvaluateAsync(input);

        result.Should().Be(TransactionStatus.Rejected);
    }

    [Fact]
    public async Task EvaluateAsync_ShouldApprove_WhenFirstTransactionAndBelowLimit()
    {
        var input = new AntiFraudEvaluationInput
        {
            SourceAccountId = Guid.NewGuid(),
            Value = 1000
        };

        var result = await _evaluator.EvaluateAsync(input);

        result.Should().Be(TransactionStatus.Approved);
    }

    [Fact]
    public async Task EvaluateAsync_ShouldReject_WhenDailyTotalExceedsLimit()
    {
        var sourceAccountId = Guid.NewGuid();

        await _evaluator.EvaluateAsync(new AntiFraudEvaluationInput
        {
            SourceAccountId = sourceAccountId,
            Value = 1500
        });

        var result = await _evaluator.EvaluateAsync(new AntiFraudEvaluationInput
        {
            SourceAccountId = sourceAccountId,
            Value = 600
        });

        result.Should().Be(TransactionStatus.Rejected);
    }
}
