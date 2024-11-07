using System.Reflection;
using Microsoft.OpenApi.Models;
using NothingRpcApi.Configs;
using NothingServices.Abstractions.Attributes;
using NothingServices.Abstractions.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace NothingRpcApi.Extensions;

/// <summary>
/// Методы расширений для Swagger
/// </summary>
internal static class SwaggerExtensions
{
    /// <summary>
    /// Добавить конфигурацию Swagger в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов с добавленной конфигурацией Swagger</returns>
    internal static IServiceCollection AddAppSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.SwaggerGeneratorOptions = new SwaggerGeneratorOptions
            {
                IgnoreObsoleteActions = true
            };
            options.IncludeXmlComments(xmlPath);
            options.IncludeGrpcXmlComments(xmlPath, includeControllerXmlComments: true);
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;
            var swaggerVersion = assembly.GetCustomAttribute<OpenApiVersionAttribute>()?.Version;
            var version = $"v{swaggerVersion}";
            var openApiInfo = new OpenApiInfo
            {
                Title = assemblyName,
                Version = version,
            };
            options.SwaggerDoc(version, openApiInfo);
        });
        return services;
    }

    /// <summary>
    /// Добавить графический интерфейс Swagger в конструктор приложения
    /// </summary>
    /// <param name="applicationBuilder">Конструктор приложения</param>
    /// <param name="configuration">Конфигурация приложения</param>
    /// <returns>Конструктор приложения с добавленной графическим интерфейсом Swagger</returns>
    internal static IApplicationBuilder UseAppSwaggerUI(
        this IApplicationBuilder applicationBuilder,
        IConfiguration configuration)
    {
        applicationBuilder.UseSwaggerUI(options =>
        {
            var config = configuration.GetConfig<AppConfig>();
            var assembly = Assembly.GetExecutingAssembly();
            var swaggerVersion = assembly.GetCustomAttribute<OpenApiVersionAttribute>()?.Version;
            var endpoint = $"{config.PathBase}/swagger/v{swaggerVersion}/swagger.json";
            var assemblyName = assembly.GetName().Name;
            var name = $"{assemblyName} v{swaggerVersion}";
            options.SwaggerEndpoint(endpoint, name);
            options.DocumentTitle = $"{assemblyName} Api Swagger Documentation";
            options.DocExpansion(DocExpansion.List);
        });
        return applicationBuilder;
    }
}