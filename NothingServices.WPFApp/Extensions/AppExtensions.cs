using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingServices.Abstractions.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;
using NothingServices.WPFApp.Clients;
using NothingServices.WPFApp.Configs;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.Views;

namespace NothingServices.WPFApp.Extensions;

/// <summary>
/// Методы расширений для приложения
/// </summary>
public static class AppExtensions
{
    /// <summary>
    /// Добавить клиенты внешних сервисов приложения в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="configuration">Конфигурация приложения</param>
    /// <returns>Коллекция сервисов с добавленными клиентами внешних сервисов приложения</returns>
    /// <exception cref="ConfigurationNullException{TConfig}">
    /// Ошибка, возникшая при получении конфигурации.
    /// </exception>
    public static IServiceCollection AddAppClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetConfig<NothingRpcApiClientConfig>();
        services.AddTransient(_ => new NothingRpcService.NothingRpcServiceClient(config.GrpcChannel));
        services.AddTransient<INothingWebApiClient, NothingWebApiClient>();
        return services;
    }

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
        services.Configure<NothingWebApiClientConfig>(configuration);
        return services;
    }

    /// <summary>
    /// Добавить сервисы приложения в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов с добавленными сервисами приложения</returns>
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IAppVersionProvider, AppVersionProvider>();
        services.AddScoped<INotificator, Notificator>();
        services.AddScoped<StartupService>();
        return services;
    }

    /// <summary>
    /// Добавить представления приложения в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов с добавленными представления приложения</returns>
    public static IServiceCollection AddAppViews(this IServiceCollection services)
    {
        services.AddScoped<MainWindow>();

        services.AddScoped<ApiSelectionVM>();
        services.AddScoped<MainWindowVM>();

        services.AddScoped<GRpcApiButtonVM>();
        services.AddScoped<RestApiButtonVM>();

        return services;
    }

    /// <summary>
    /// Добавить сконфигурированный экземпляр <see cref="IHttpClientFactory"/> в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="configuration">Конфигурация приложения</param>
    /// <returns>Коллекция сервисов с добавленными сконфигурированным экземпляром <see cref="IHttpClientFactory"/></returns>
    /// <exception cref="ConfigurationNullException{TConfig}">
    /// Ошибка, возникшая при получении конфигурации.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// Ошибка, возникшая при получении доступа к файлу.
    /// </exception>
    public static IServiceCollection AddAppHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetConfig<CertificateConfig>();
        if(!File.Exists(config.FileName))
            throw new FileNotFoundException($"Не удалось найти файл {config.FileName}");
        var certificate = new X509Certificate2(config.FileName, config.Password);
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(certificate);
        services.AddHttpClient(nameof(WPFApp), _ => {})
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        return services;
    }
}