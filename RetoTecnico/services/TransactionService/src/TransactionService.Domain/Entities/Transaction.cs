using TransactionService.Domain.Enums;

namespace TransactionService.Domain.Entities;

public class Transaction
{
    public Guid SourceAccountId { get; set; }
    public Guid TargetAccountId { get; set; }
    public int TransferTypeId { get; set; }
    public decimal Value { get; set; }
    public Guid TransactionExternalId { get; set; }
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
