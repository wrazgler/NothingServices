using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;

namespace NothingKafka.Services;

/// <summary>
/// Сервис администрирования Kafka
/// </summary>
/// <param name="logger">Экземпляр <see cref="ILogger"/></param>
/// <param name="config">Конфигурация Kafka</param>
public sealed class KafkaService(
    ILogger<KafkaService> logger,
    IOptions<KafkaConfig> config)
    : IKafkaService
{
    private readonly ILogger<KafkaService> _logger = logger;
    private readonly KafkaConfig _kafkaConfig = config.Value;

    /// <summary>
    /// Проверить доступность работы Kafka
    /// </summary>
    /// <param name="timeoutInSeconds">Время ожидание ответа</param>
    public bool IsKafkaAvailable(int timeoutInSeconds = 10)
    {
        try
        {
            var adminClientBuilder = ConfigureAdminClientBuilder(_kafkaConfig);
            using var adminClient = adminClientBuilder.Build();
            var meta = adminClient.GetMetadata(TimeSpan.FromSeconds(timeoutInSeconds));
            return meta.Brokers.Count > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Создать заголовок сообщения
    /// </summary>
    /// <param name="topicName">Заголовок сообщения</param>
    public async Task CreateTopic(string topicName)
    {
        var adminClientBuilder = ConfigureAdminClientBuilder(_kafkaConfig);
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

    private static AdminClientBuilder ConfigureAdminClientBuilder(KafkaConfig kafkaConfig)
    {
        var adminClientConfig = new AdminClientConfig
        {
            BootstrapServers = kafkaConfig.BootstrapServers
        };
        var adminClientBuilder = new AdminClientBuilder(adminClientConfig);
        return adminClientBuilder;
    }
}