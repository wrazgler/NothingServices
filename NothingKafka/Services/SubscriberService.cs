using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingKafka.Dtos;

namespace NothingKafka.Services;

/// <summary>
/// Сервис подписчик на Kafka
/// </summary>
/// <param name="consumerService">Сервис получения сообщений из Kafka</param>
/// <param name="hostEnvironment">Окружение хоста приложения</param>
/// <param name="logger">Экземпляр <see cref="ILogger"/></param>
/// <param name="kafkaConfig">Конфигурация Kafka</param>
/// <param name="kafkaService">Сервис администрирования Kafka</param>
/// <param name="nothingService">Сервис бизнес логики</param>
/// <param name="nothingServiceConfig">Заголовки Kafka сервиса NothingService</param>
public sealed class SubscriberService(
    IConsumerService consumerService,
    IHostEnvironment hostEnvironment,
    ILogger<SubscriberService> logger,
    IKafkaService kafkaService,
    INothingService nothingService,
    IOptions<KafkaConfig> kafkaConfig,
    IOptions<NothingServiceConfig> nothingServiceConfig)
    : BackgroundService
{
    private readonly KafkaConfig _kafkaConfig = kafkaConfig.Value;
    private readonly NothingServiceConfig _nothingServiceConfig = nothingServiceConfig.Value;
    private readonly IConsumerService _consumerService = consumerService;
    private readonly IHostEnvironment _hostEnvironment = hostEnvironment;
    private readonly ILogger<SubscriberService> _logger = logger;
    private readonly IKafkaService _kafkaService = kafkaService;
    private readonly INothingService _nothingService = nothingService;

    /// <summary>
    /// Запуск фонового процесса обработки сообщений из Kafka
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (_hostEnvironment.IsEnvironment("Local"))
            return Task.CompletedTask;

        _ = Task.Run(() => Subscribe(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task Subscribe(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_kafkaService.IsKafkaAvailable())
            {
                try
                {
                    _ = Task.Run(() => _consumerService.SubscribeTopic<GetNothingModelDto>(
                        _nothingServiceConfig.GetModelTopic,
                        (message, token) => _nothingService.Get(message, token),
                        cancellationToken), cancellationToken);
                    _ = Task.Run(() => _consumerService.SubscribeTopic<GetNothingModelsDto>(
                        _nothingServiceConfig.GetModelsTopic,
                        (message, token) => _nothingService.Get(message, token),
                        cancellationToken), cancellationToken);
                    _ = Task.Run(() => _consumerService.SubscribeTopic<CreateNothingModelDto>(
                        _nothingServiceConfig.CreateTopic,
                        (message, token) => _nothingService.Create(message, token),
                        cancellationToken), cancellationToken);
                    _ = Task.Run(() => _consumerService.SubscribeTopic<DeleteNothingModelDto>(
                        _nothingServiceConfig.DeleteTopic,
                        (message, token) => _nothingService.Delete(message, token),
                        cancellationToken), cancellationToken);
                    _ = Task.Run(() => _consumerService.SubscribeTopic<UpdateNothingModelDto>(
                        _nothingServiceConfig.UpdateTopic,
                        (message, token) => _nothingService.Update(message, token),
                        cancellationToken), cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error occurred in ServiceWorker: {Error}", e.Message);
                }
            }
            else
            {
                var message = $"Kafka is not available [{_kafkaConfig.BootstrapServers}]";
                _logger.LogWarning(message);
            }

            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
        }
    }
}