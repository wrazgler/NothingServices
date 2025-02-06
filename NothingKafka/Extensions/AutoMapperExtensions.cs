using AutoMapper;

namespace NothingKafka.Extensions;

/// <summary>
/// Методы расширений для <see cref="Mapper"/>
/// </summary>
internal static class AutoMapperExtensions
{
    /// <summary>
    /// Добавить конфигурацию <see cref="Mapper"/> в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов с добавленной конфигурацией <see cref="Mapper"/></returns>
    internal static IServiceCollection AddAppAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(configuration =>
        {
            configuration.AllowNullCollections = true;
        });
        return services;
    }
}