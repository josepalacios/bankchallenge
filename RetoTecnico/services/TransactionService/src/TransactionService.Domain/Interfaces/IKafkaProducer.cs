namespace TransactionService.Domain.Interfaces;

public interface IKafkaProducer
{
    Task ProduceAsync<T>(string topic, T message);
}