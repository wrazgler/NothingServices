using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingServices.Abstractions.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;
using NothingServices.WPFApp.Clients;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Configs;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.Views;

namespace NothingServices.WPFApp.Extensions;

/// <summary>
/// Методы расширений для приложения
/// </summary>
internal static class AppExtensions
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
    internal static IServiceCollection AddAppClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetConfig<NothingRpcApiClientConfig>();
        services.AddScoped(_ => new NothingRpcService.NothingRpcServiceClient(config.GrpcChannel));
        services.AddScoped<INothingWebApiClient, NothingWebApiClient>();
        return services;
    }

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
        services.Configure<NothingWebApiClientConfig>(configuration);
        return services;
    }

    /// <summary>
    /// Добавить сервисы приложения в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов с добавленными сервисами приложения</returns>
    internal static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<CancellationTokenSource>();
        services.AddScoped<IAppVersionProvider, AppVersionProvider>();
        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IMainWindowManager, MainWindowManager>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<StartupService>();

        services.AddScoped<ICreateNothingModelVMFactory, CreateNothingModelVMFactory>();
        services.AddScoped<IDeleteNothingModelVMFactory, DeleteNothingModelVMFactory>();
        services.AddScoped<INothingModelVMFactory, NothingModelVMFactory>();
        services.AddScoped<IUpdateNothingModelVMFactory, UpdateNothingModelVMFactory>();

        services.AddScoped<NothingRpcApiClientStrategy>();
        services.AddScoped<NothingWebApiClientStrategy>();
        return services;
    }

    /// <summary>
    /// Добавить данные представлений приложения в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов с добавленными данными представлений приложения</returns>
    internal static IServiceCollection AddAppViewModels(this IServiceCollection services)
    {
        services.AddScoped<ApiSelectionVM>();
        services.AddScoped<IDialogVM, DialogVM>();
        services.AddScoped<IMainWindowVM, MainWindowVM>();
        services.AddScoped<NothingModelsListVM>();

        services.AddScoped<IBackButtonVM, BackButtonVM>();
        services.AddScoped<IGRpcApiButtonVM, GRpcApiButtonVM>();
        services.AddScoped<IRestApiButtonVM, RestApiButtonVM>();

        services.AddScoped<ICloseDialogCommand, CloseDialogCommand>();
        services.AddScoped<ICreateCommand, CreateCommand>();
        services.AddScoped<IDeleteCommand, DeleteCommand>();
        services.AddScoped<OpenApiSelectionCommand>();
        services.AddScoped<IOpenCreateNothingModelCommand, OpenCreateNothingModelCommand>();
        services.AddScoped<IOpenDeleteNothingModelCommand, OpenDeleteNothingModelCommand>();
        services.AddScoped<IOpenNothingModelsListCommand, OpenNothingModelsListCommand>();
        services.AddScoped<IOpenUpdateNothingModelCommand, OpenUpdateNothingModelCommand>();
        services.AddScoped<IUpdateCommand, UpdateCommand>();

        return services;
    }

    /// <summary>
    /// Добавить представления приложения в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов с добавленными представления приложения</returns>
    internal static IServiceCollection AddAppViews(this IServiceCollection services)
    {
        services.AddScoped<ICreateNothingModelView, CreateNothingModelView>();
        services.AddScoped<IDeleteNothingModelView, DeleteNothingModelView>();
        services.AddScoped<IMainWindow, MainWindow>();
        services.AddScoped<IUpdateNothingModelView, UpdateNothingModelView>();
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
    internal static IServiceCollection AddAppHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetConfig<CertificateConfig>();
        if(!File.Exists(config.FileName))
            throw new FileNotFoundException($"Не удалось найти файл {config.FileName}");
#if NET9_0_OR_GREATER
        var certificate = X509CertificateLoader.LoadPkcs12FromFile(config.FileName, config.Password);
#else
        var certificate = new X509Certificate2(config.FileName, config.Password);
#endif
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(certificate);
        services.AddHttpClient(nameof(WPFApp), _ => {})
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        return services;
    }
}