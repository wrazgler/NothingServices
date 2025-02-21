namespace NothingKafka.Services;

/// <summary>
/// Сервис администрирования Kafka
/// </summary>
public interface IKafkaService
{
    /// <summary>
    /// Проверить доступность работы Kafka
    /// </summary>
    /// <param name="timeoutInSeconds">Время ожидание ответа</param>
    /// <returns>
    /// Возвращает <see langword="true"/>, если сервис Kafka доступен, и <see langword="false"/>, если нет
    /// </returns>
    bool IsKafkaAvailable(int timeoutInSeconds = 10);

    /// <summary>
    /// Создать заголовок сообщения
    /// </summary>
    /// <param name="topicName">Заголовок сообщения</param>
    Task CreateTopic(string topicName);
}