namespace NothingKafka.Services;

/// <summary>
/// Сервис отправки сообщений в Kafka
/// </summary>
public interface IProducerService
{
    /// <summary>
    /// Отправить сообщений в Kafka
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="topicName">Заголовок сообщения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task SendMessage<TMessage>(
        TMessage message,
        string topicName,
        CancellationToken cancellationToken = default);
}