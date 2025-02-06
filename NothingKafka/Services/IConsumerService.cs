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
    /// <param name="callbackAction">Действие обратного вызова для заголовка</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task SubscribeTopic<TMessage>(
        string topicName,
        Action<TMessage, CancellationToken> callbackAction,
        CancellationToken cancellationToken = default);
}