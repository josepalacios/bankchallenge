namespace TransactionService.Infrastructure.Kafka.Producers;

using Confluent.Kafka;
using System.Text.Json;
using TransactionService.Domain.Interfaces;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(ProducerConfig config)
    {
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync<T>(string topic, T message)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var kafkaMessage = new Message<Null, string> { Value = json };
            await _producer.ProduceAsync(topic, kafkaMessage);
        }
        catch (ProduceException<Null, string> ex)
        {
            Console.WriteLine($"Kafka producer error: {ex.Error.Reason}");
            throw;
        }
    }
}
