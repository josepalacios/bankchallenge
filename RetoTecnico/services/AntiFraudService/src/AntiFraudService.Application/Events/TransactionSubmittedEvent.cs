namespace AntiFraudService.Application.Events;

public class TransactionSubmittedEvent
{
    public Guid TransactionId { get; set; }
    public Guid SourceAccountId { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }
}