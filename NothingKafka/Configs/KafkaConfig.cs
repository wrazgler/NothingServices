namespace NothingKafka.Configs;

/// <summary>
/// Конфигурация Kafka
/// </summary>
public sealed class KafkaConfig
{
    /// <summary>
    /// Сервер Bootstrap
    /// </summary>
    [ConfigurationKeyName("KAFKA_BOOTSTRAP_SERVERS")]
    public required string BootstrapServers { get; init; }

    /// <summary>
    /// Имя группы получателей
    /// </summary>
    [ConfigurationKeyName("KAFKA_CONSUMER_GROUP")]
    public string? ConsumerGroup { get; init; }
}