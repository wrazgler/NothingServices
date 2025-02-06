namespace NothingKafka.Configs;

/// <summary>
/// Заголовки Kafka сервиса NothingService
/// </summary>
public sealed class NothingServiceConfig
{
    /// <summary>
    /// Заголовок создать модель
    /// </summary>
    [ConfigurationKeyName("NOTHING_SERVICE_CREATE_TOPIC")]
    public required string CreateTopic { get; init; }

    /// <summary>
    /// Заголовок удалить модель
    /// </summary>
    [ConfigurationKeyName("NOTHING_SERVICE_Delete_TOPIC")]
    public required string DeleteTopic { get; init; }

    /// <summary>
    /// Заголовок получить модель
    /// </summary>
    [ConfigurationKeyName("NOTHING_SERVICE_GET_MODEL_TOPIC")]
    public required string GetModelTopic { get; init; }

    /// <summary>
    /// Заголовок получить коллекцию моделей
    /// </summary>
    [ConfigurationKeyName("NOTHING_SERVICE_GET_MODELS_TOPIC")]
    public required string GetModelsTopic { get; init; }

    /// <summary>
    /// Заголовок обновить модель
    /// </summary>
    [ConfigurationKeyName("NOTHING_SERVICE_UPDATE_TOPIC")]
    public required string UpdateTopic { get; init; }
}