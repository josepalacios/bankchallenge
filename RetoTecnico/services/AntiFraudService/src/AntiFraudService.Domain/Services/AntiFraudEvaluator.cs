using AntiFraudService.Domain.Enums;
using AntiFraudService.Domain.Interfaces;
using AntiFraudService.Domain.Models;
using System.Collections.Concurrent;

namespace AntiFraudService.Domain.Services;

public class AntiFraudEvaluator : IAntiFraudEvaluator
{
    private static readonly ConcurrentDictionary<(Guid AccountId, DateOnly Date), decimal> DailyTotals = new();

    public Task<TransactionStatus> EvaluateAsync(AntiFraudEvaluationInput input)
    {
        if (input.Value > 2000)
            return Task.FromResult(TransactionStatus.Rejected);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var key = (input.SourceAccountId, today);

        var newTotal = DailyTotals.AddOrUpdate(
            key,
            input.Value,
            (_, currentTotal) => currentTotal + input.Value
        );

        var status = newTotal > 20000
            ? TransactionStatus.Rejected
            : TransactionStatus.Approved;

        return Task.FromResult(status);

    }
}