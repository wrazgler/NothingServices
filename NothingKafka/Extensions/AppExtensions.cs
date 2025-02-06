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
        return services;
    }
}