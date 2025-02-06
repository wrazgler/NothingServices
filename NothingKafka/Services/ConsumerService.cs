using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;

namespace NothingKafka.Services;

/// <summary>
/// Сервис получения сообщений из Kafka
/// </summary>
/// <param name="logger">Экземпляр <see cref="ILogger"/></param>
/// <param name="kafkaService">Сервис администрирования Kafka</param>
/// <param name="config">Конфигурация Kafka</param>
public sealed class ConsumerService(
    ILogger<ConsumerService> logger,
    IKafkaService kafkaService,
    IOptions<KafkaConfig> config)
    : IConsumerService
{
    private readonly ILogger<ConsumerService> _logger = logger;
    private readonly IKafkaService _kafkaService = kafkaService;
    private readonly KafkaConfig _kafkaConfig = config.Value;

    /// <summary>
    /// Получить сообщение из Kafka
    /// </summary>
    /// <param name="topicName">Заголовок сообщения</param>
    /// <param name="callbackAction">Действие обратного вызова для заголовка</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task SubscribeTopic<TMessage>(
        string topicName,
        Action<TMessage, CancellationToken> callbackAction,
        CancellationToken cancellationToken = default)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaConfig.BootstrapServers,
            GroupId = _kafkaConfig.ConsumerGroup,
        };
        var consumerBuilder = new ConsumerBuilder<Null, TMessage>(consumerConfig);
        using var consumer = consumerBuilder.Build();
        consumer.Subscribe(topicName);
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = await Task.Run(() => consumer.Consume(cancellationToken), cancellationToken);
                callbackAction(consumeResult.Message.Value, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                var message = $"Consumption of kafka topic {topicName} was cancelled";
                _logger.LogDebug(message);
                consumer.Close();
            }
            catch (ConsumeException consumeException)
                when (consumeException.Message.Contains("topic not available"))
            {
                consumer.Close();
                await _kafkaService.CreateTopic(topicName);
            }
            catch (Exception ex)
            {
                var message = $"Error consuming from kafka topic {topicName}: {ex}";
                _logger.LogError(ex, message);
                consumer.Close();
            }
            _logger.LogDebug("New message was consumed from {TopicName}", topicName);
        }

        consumer.Close();
    }
}