using System.Runtime.CompilerServices;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;

namespace NothingKafka.Services;

/// <summary>
/// Сервис получения сообщений из Kafka
/// </summary>
/// <param name="logger">Экземпляр <see cref="ILogger"/></param>
/// <param name="config">Конфигурация Kafka</param>
public sealed class ConsumerService(
    ILogger<ConsumerService> logger,
    IOptions<KafkaConfig> config)
    : IConsumerService
{
    private readonly ILogger<ConsumerService> _logger = logger;
    private readonly KafkaConfig _kafkaConfig = config.Value;

    /// <summary>
    /// Получить сообщение из Kafka
    /// </summary>
    /// <param name="topicName">Заголовок сообщения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async IAsyncEnumerable<TMessage> SubscribeTopic<TMessage>(
        string topicName,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
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
            ConsumeResult<Null, TMessage> consumeResult;
            try
            {
                consumeResult = await Task.Run(() => consumer.Consume(cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                var message = $"Consumption of kafka topic {topicName} was cancelled";
                _logger.LogDebug(message);
                consumer.Close();
                yield break;
            }
            catch (ConsumeException consumeException)
                when (consumeException.Message.Contains("topic not available"))
            {
                consumer.Close();
                await CreateTopic(topicName);
                yield break;
            }
            catch (Exception ex)
            {
                var message = $"Error consuming from kafka topic {topicName}: {ex}";
                _logger.LogError(ex, message);
                consumer.Close();
                yield break;
            }
            _logger.LogDebug("New message was consumed from {TopicName}", topicName);
            yield return consumeResult.Message.Value;
        }

        consumer.Close();
    }

    private async Task CreateTopic(string topicName)
    {
        var adminClientConfig = new AdminClientConfig
        {
            BootstrapServers = _kafkaConfig.BootstrapServers
        };
        var adminClientBuilder = new AdminClientBuilder(adminClientConfig);
        using var adminClient = adminClientBuilder.Build();
        try
        {
            var topicSpecification = new TopicSpecification
            {
                Name = topicName,
                ReplicationFactor = 1,
                NumPartitions = 1
            };
            await adminClient.CreateTopicsAsync([topicSpecification]);
            _logger.LogInformation("Topic [{TopicName}] created", topicName);
        }
        catch (CreateTopicsException ex)
        {
            var result = ex.Results[0];
            var message = $"An error occured creating topic {result.Topic}: {result.Error.Reason}";
            _logger.LogError(ex, message);
        }
    }
}