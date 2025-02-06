namespace NothingKafka.Services;

/// <summary>
/// Сервис получения сообщений из Kafka
/// </summary>
public interface IConsumerService
{
    /// <summary>
    /// Получить сообщение из Kafka
    /// </summary>
    /// <param name="topicName">Заголовок сообщения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    IAsyncEnumerable<TMessage> SubscribeTopic<TMessage>(
        string topicName,
        CancellationToken cancellationToken = default);
}