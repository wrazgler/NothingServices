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
    bool IsKafkaAvailable(int timeoutInSeconds = 10);

    /// <summary>
    /// Создать заголовок сообщения
    /// </summary>
    /// <param name="topicName">Заголовок сообщения</param>
    Task CreateTopic(string topicName);
}