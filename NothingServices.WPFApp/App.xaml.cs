using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingServices.WPFApp.Extensions;
using NothingServices.WPFApp.Services;
using System.Windows;

namespace NothingServices.WPFApp;

/// <summary>
/// Главный класс работы приложения
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Инициализация приложения, вызывается событием <see cref="Application.Startup"/>
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddEnvironmentVariables();
        });
        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddAppConfigs(context.Configuration);
            services.AddAppHttpClient(context.Configuration);
            services.AddAppClients(context.Configuration);
            services.AddAppServices();
            services.AddAppViews();
        });
        var host = hostBuilder.Build();
        host.Services.GetRequiredService<StartupService>().Start();
        base.OnStartup(e);
    }
}