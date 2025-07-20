using AntiFraudService.Application.Events;
using AntiFraudService.Domain.Enums;

namespace AntiFraudService.Application.Interfaces;

public interface IAntiFraudService
{
    Task<TransactionStatus> ValidateTransactionAsync(TransactionSubmittedEvent transactionEvent);
}