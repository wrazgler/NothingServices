using AutoMapper;
using NothingWebApi.Dtos;
using NothingWebApi.Models;

namespace NothingWebApi.Extensions;

/// <summary>
/// Методы расширений для <see cref="Mapper"/>
/// </summary>
public static class AutoMapperExtensions
{
    /// <summary>
    /// Добавить конфигурацию <see cref="Mapper"/> в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов с добавленной конфигурацией <see cref="Mapper"/></returns>
    public static IServiceCollection AddAppAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(configuration =>
        {
            configuration.AllowNullCollections = true;
            configuration.CreateMap<NothingModel, NothingModelDto>();
            configuration.CreateMap<CreateNothingModelDto, NothingModel>();
        });
        return services;
    }
}