using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingServices.WPFApp.Extensions;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels;

namespace NothingServices.WPFApp.IntegrationTests;

public class AppTests
{
    [Fact]
    public async Task NothingModels_Contain_Single_Element()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var app = new App();
            var host = GetHost();
            var startupService = host.Services.GetRequiredService<StartupService>();
            //var mainWindowVM = host.Services.GetRequiredService<MainWindowVM>();

            //Act
            startupService.Start();
            app.InitializeComponent();
            var mainWindowVM = app.MainWindow?.DataContext as MainWindowVM;
            var result = mainWindowVM?.NothingModelsListVM.NothingModels
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
            .AddJsonFile("appsettings.AppTesting.json")
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
        await Process.Start("docker", "volume remove -f wpf_test_nothing_services_wpf_app_test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}