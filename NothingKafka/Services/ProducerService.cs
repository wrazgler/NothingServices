using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;

namespace NothingKafka.Services;

/// <summary>
/// Сервис отправки сообщений в Kafka
/// </summary>
/// <param name="logger">Экземпляр <see cref="ILogger"/></param>
/// <param name="config">Конфигурация Kafka</param>
public sealed class ProducerService(
    ILogger<ProducerService> logger,
    IOptions<KafkaConfig> config)
    : IProducerService
{
    private readonly ILogger<ProducerService> _logger = logger;
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
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _kafkaConfig.BootstrapServers,
        };
        var producerBuilder = new ProducerBuilder<Null, TMessage>(producerConfig);
        using var producer = producerBuilder.Build();
        await producer.ProduceAsync(
            topicName,
            new Message<Null, TMessage>() {Value = message},
            cancellationToken);
    }
}