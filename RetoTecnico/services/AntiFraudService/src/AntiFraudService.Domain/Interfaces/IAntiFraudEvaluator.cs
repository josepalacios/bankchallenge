using AntiFraudService.Domain.Enums;
using AntiFraudService.Domain.Models;
using System.Threading.Tasks;

namespace AntiFraudService.Domain.Interfaces;

public interface IAntiFraudEvaluator
{
    Task<TransactionStatus> EvaluateAsync(AntiFraudEvaluationInput input);
}
