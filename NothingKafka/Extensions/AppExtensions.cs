using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingKafka.Configs;
using NothingKafka.Services;
using NothingServices.Abstractions.Configs;

namespace NothingKafka.Extensions;

/// <summary>
/// Методы расширений для приложения
/// </summary>
internal static class AppExtensions
{
    /// <summary>
    /// Добавить конфигурации приложения в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="configuration">Конфигурация приложения</param>
    /// <returns>Коллекция сервисов с добавленными конфигурациями приложения</returns>
    internal static IServiceCollection AddAppConfigs(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<KafkaConfig>(configuration);
        services.Configure<NothingServiceConfig>(configuration);
        return services;
    }

    /// <summary>
    /// Добавить сервисы приложения в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов с добавленными сервисами приложения</returns>
    internal static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddTransient<IConsumerService, ConsumerService>();
        services.AddTransient<IKafkaService, KafkaService>();
        services.AddTransient<INothingService, NothingService>();
        services.AddTransient<IProducerService, ProducerService>();
        return services;
    }
}