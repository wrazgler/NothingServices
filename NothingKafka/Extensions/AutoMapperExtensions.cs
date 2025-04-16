using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

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
        var configuration = new MapperConfiguration(configurationExpression =>
        {
            configurationExpression.AddMaps(Assembly.GetExecutingAssembly());
        });
        configuration.AssertConfigurationIsValid();
        var mapper = new Mapper(configuration);
        services.AddSingleton<IMapper, Mapper>(_ => mapper);
        return services;
    }
}