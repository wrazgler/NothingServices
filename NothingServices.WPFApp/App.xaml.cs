using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingServices.WPFApp.Extensions;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp;

/// <summary>
/// Главный класс работы приложения
/// </summary>
public partial class App
{
    private IHost AppHost { get; } = CreateHost();

    /// <summary>
    /// Инициализация приложения, вызывается событием <see cref="Application.Startup"/>
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
        AppHost.Services.GetRequiredService<StartupService>().Start();
        base.OnStartup(e);
    }

    /// <summary>
    /// Закрытие приложения, вызывается событием <see cref="Application.Exit"/>
    /// </summary>
    protected override void OnExit(ExitEventArgs e)
    {
        AppHost.Services.GetRequiredService<CancellationTokenSource>().Cancel();
        AppHost.Services.GetRequiredService<IHostApplicationLifetime>().StopApplication();
        base.OnExit(e);
    }

    private static IHost CreateHost()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddEnvironmentVariables();
        });
        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddAppAutoMapper();
            services.AddAppConfigs(context.Configuration);
            services.AddAppHttpClient(context.Configuration);
            services.AddAppClients(context.Configuration);
            services.AddAppServices();
            services.AddAppViewModels();
            services.AddAppViews();
        });
        var host = hostBuilder.Build();
        return host;
    }
}