using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingServices.WPFApp.Extensions;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels;

namespace NothingServices.WPFApp.IntegrationTests;

public class AppTests
{
    [Fact]
    public async Task NothingModels_gRpcApi_Contain_Single_Element()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var uiThread = new Thread(parameter =>
            {
                var hostThread = parameter as IHost ?? throw new ArgumentNullException(nameof(parameter));
                var startupService = hostThread.Services.GetRequiredService<StartupService>();
                startupService.Start();
                var mainWindowManager = hostThread.Services.GetRequiredService<IMainWindowManager>();
                mainWindowManager.Strategy = hostThread.Services.GetRequiredService<NothingRpcApiClientStrategy>();
                mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(5000);
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var result = mainWindowVM.NothingModelsListVM.NothingModels
                         ?? throw new NullReferenceException();

            //Assert
            Assert.Single(result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task NothingModels_WebApiClient_Contain_Single_Element()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var uiThread = new Thread(parameter =>
            {
                var hostThread = parameter as IHost ?? throw new ArgumentNullException(nameof(parameter));
                var startupService = hostThread.Services.GetRequiredService<StartupService>();
                startupService.Start();
                var mainWindowManager = hostThread.Services.GetRequiredService<IMainWindowManager>();
                mainWindowManager.Strategy = hostThread.Services.GetRequiredService<NothingWebApiClientStrategy>();
                mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(5000);
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var result = mainWindowVM.NothingModelsListVM.NothingModels
                ?? throw new NullReferenceException();

            //Assert
            Assert.Single(result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    private static IHost GetHost()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.App.Testing.json")
            .Build();
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddEnvironmentVariables();
        });
        hostBuilder.ConfigureServices((_, services) =>
        {
            services.AddAppAutoMapper();
            services.AddAppConfigs(configuration);
            services.AddAppHttpClient(configuration);
            services.AddAppClients(configuration);
            services.AddAppServices();
            services.AddAppViewModels();
            services.AddAppViews();
        });
        var host = hostBuilder.Build();
        return host;
    }

    private static async Task StartApp(int delay = 10000)
    {
        var projectPath = Path.GetFullPath("../../../");
        var dockerFilePath = Path.Combine(projectPath, "docker-compose.app-test.yml");
        await Process.Start("docker", $"compose -f {dockerFilePath} up -d").WaitForExitAsync();
        await Task.Delay(delay);
    }

    private static async Task StopApp(int beforeDelay = 10000, int afterDelay = 2000)
    {
        await Task.Delay(beforeDelay);
        await Process.Start("docker", "container remove -f -v wpf_app_test_postgres_nothing_grpc_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f -v wpf_app_test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f wpf_app_test_nothing_grpc_api")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f wpf_app_test_nothing_web_api")
            .WaitForExitAsync();
        await Process.Start("docker", "image remove -f wpf_app_test_nothing_grpc_api")
            .WaitForExitAsync();
        await Process.Start("docker", "image remove -f wpf_app_test_nothing_web_api")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f wpf_app_test_nothing_services_wpf_app_test_postgres_nothing_grpc_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f wpf_app_test_nothing_services_wpf_app_test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}