using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingKafka.Serializers;

namespace NothingKafka.Services;

/// <summary>
/// Сервис отправки сообщений в Kafka
/// </summary>
/// <param name="config">Конфигурация Kafka</param>
public sealed class ProducerService(IOptions<KafkaConfig> config)
    : IProducerService
{
    private readonly KafkaConfig _kafkaConfig = config.Value;

    /// <summary>
    /// Отправить сообщений в Kafka
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="topicName">Заголовок сообщения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task SendMessage<TMessage>(
        TMessage message,
        string topicName,
        CancellationToken cancellationToken = default)
        where TMessage : class
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _kafkaConfig.BootstrapServers,
        };
        var producerBuilder = new ProducerBuilder<Null, TMessage>(producerConfig);
        producerBuilder.SetValueSerializer(new KafkaSerializer<TMessage>());
        using var producer = producerBuilder.Build();
        var producerMessage = new Message<Null, TMessage>()
        {
            Value = message
        };
        await producer.ProduceAsync(topicName, producerMessage, cancellationToken);
    }
}