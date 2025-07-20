using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using AntiFraudService.Application.Interfaces;
using AntiFraudService.Application.Events;

public class TransactionSubmittedConsumer
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IAntiFraudService _antiFraudService;
    private readonly ILogger<TransactionSubmittedConsumer> _logger;

    public TransactionSubmittedConsumer(ConsumerConfig config,IAntiFraudService antiFraudService,ILogger<TransactionSubmittedConsumer> logger)
    {
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _antiFraudService = antiFraudService;
        _logger = logger;
    }

    public async Task StartConsumingAsync(string topic, CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var message = JsonSerializer.Deserialize<TransactionSubmittedEvent>(consumeResult.Message.Value);

                _logger.LogInformation("Received message: {Message}", consumeResult.Message.Value);

                if (message != null)
                {
                    var result = await _antiFraudService.ValidateTransactionAsync(message);
                    _logger.LogInformation("Transaction {TransactionId} evaluated as {Result}", message.TransactionId, result);
                }
            }
            catch (ConsumeException e)
            {
                _logger.LogError("Consume error: {Reason}", e.Error.Reason);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing transaction");
            }
        }

        _consumer.Close();
    }
}