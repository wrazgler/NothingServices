using NothingRpcApi.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;

namespace NothingRpcApi.Extensions;

/// <summary>
/// Методы расширений для приложения
/// </summary>
public static class AppExtensions
{
    /// <summary>
    /// Добавить конфигурации приложения в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="configuration">Конфигурация приложения</param>
    /// <returns>Коллекция сервисов с добавленными конфигурациями приложения</returns>
    public static IServiceCollection AddAppConfigs(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions();
        return services;
    }

    /// <summary>
    /// Добавить базовый адрес Url в конструктор приложения
    /// </summary>
    /// <param name="applicationBuilder">Конструктор приложения</param>
    /// <param name="configuration">Конфигурация приложения</param>
    /// <returns>Конструктор приложения с добавленным базовым адресом Url</returns>
    /// <exception cref="ConfigurationNullException{TConfig}">
    /// Ошибка, возникшая при получении конфигурации.
    /// </exception>
    public static IApplicationBuilder UseAppPathBase(
        this IApplicationBuilder applicationBuilder,
        IConfiguration configuration)
    {
        var config = configuration.GetConfig<AppConfig>();
        applicationBuilder.UsePathBase(config.PathBase);
        return applicationBuilder;
    }
}