namespace AntiFraudService.Domain.Models;

public class AntiFraudEvaluationInput
{
    public Guid SourceAccountId { get; set; }
    public decimal Value { get; set; }
}