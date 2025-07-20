using TransactionService.Domain.Enums;

namespace TransactionService.Application.Events;

public class TransactionStatusUpdatedEvent
{
    public Guid TransactionId { get; set; }
    public TransactionStatus Status { get; set; }
}
