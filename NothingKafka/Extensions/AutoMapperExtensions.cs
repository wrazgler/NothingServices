using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NothingKafka.Dtos;
using NothingKafka.Models;

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
            configuration.CreateMap<NothingModel, NothingModelDto>();
            configuration.CreateMap<CreateNothingModelDto, NothingModel>()
                .ForMember(model => model.Name, member => member.MapFrom(dto => dto.Name.Trim()));
        });
        return services;
    }
}