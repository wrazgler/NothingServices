using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Extensions;

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
            configuration.CreateMap<CreateNothingModelVM, CreateNothingModelDto>();
            configuration.CreateMap<CreateNothingModelVM, CreateNothingModelWebDto>();
            configuration.CreateMap<DeleteNothingModelVM, NothingModelIdDto>();
            configuration.CreateMap<UpdateNothingModelVM, UpdateNothingModelDto>();
            configuration.CreateMap<UpdateNothingModelVM, UpdateNothingModelWebDto>();
        });
        return services;
    }
}