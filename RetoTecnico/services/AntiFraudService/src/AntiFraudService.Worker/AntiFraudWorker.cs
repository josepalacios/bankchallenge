using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class AntiFraudWorker : BackgroundService
{
    private readonly TransactionSubmittedConsumer _consumer;
    private readonly ILogger<AntiFraudWorker> _logger;
    private readonly string _topic;

    public AntiFraudWorker(TransactionSubmittedConsumer consumer, ILogger<AntiFraudWorker> logger, IConfiguration configuration)
    {
        _consumer = consumer;
        _logger = logger;
        _topic = configuration["Kafka:Topic"]!;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AntiFraud Worker started.");
        
        try
        {
            await _consumer.StartConsumingAsync(_topic, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("AntiFraud Worker cancellation requested.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in AntiFraud Worker.");
        }
        finally
        {
            _logger.LogInformation("AntiFraud Worker stopped.");
        }
    }
}
