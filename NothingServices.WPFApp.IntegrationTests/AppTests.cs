using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Extensions;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.IntegrationTests;

public class AppTests
{
    [Fact]
    public void OpenApiSelectionCommand_ApiSelectionVM_Active_True()
    {
        //Arrange
        var host = GetHost();
        var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
        var command = host.Services.GetRequiredService<OpenApiSelectionCommand>();

        //Act
        command.Execute(null);
        var result = mainWindowVM.ApiSelectionVM.Active;

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void OpenApiSelectionCommand_NothingModelsListVM_Active_False()
    {
        //Arrange
        var host = GetHost();
        var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
        var command = host.Services.GetRequiredService<OpenApiSelectionCommand>();

        //Act
        command.Execute(null);
        var result = mainWindowVM.NothingModelsListVM.Active;

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void OpenCreateNothingModelCommand_Dialog_Open_True()
    {
        //Arrange
        var host = GetHost();
        var command = host.Services.GetRequiredService<IOpenCreateNothingModelCommand>();

        //Act
        command.Execute(null);
        var result = host.Services.GetRequiredService<IDialogVM>().Open;

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void OpenCreateNothingModelCommand_Dialog_Content_DataContext_Is_CreateNothingModelVM()
    {
        //Arrange
        var host = GetHost();
        var command = host.Services.GetRequiredService<IOpenCreateNothingModelCommand>();

        //Act
        command.Execute(null);
        var result = host.Services.GetRequiredService<IDialogVM>().Content?.DataContext;

        //Assert
        Assert.IsType<CreateNothingModelVM>(result);
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
            services.AddAppViewModels();
            services.AddScoped<IAppVersionProvider, AppVersionProvider>();
            services.AddScoped<IDialogService, DialogService>();
            services.AddScoped<IMainWindowManager, MainWindowManager>();
            services.AddScoped(_ => Mock.Of<INotificationService>());
            services.AddScoped<StartupService>();
            services.AddScoped<ICreateNothingModelVMFactory, CreateNothingModelVMFactory>();
            services.AddScoped<IDeleteNothingModelVMFactory, DeleteNothingModelVMFactory>();
            services.AddScoped<INothingModelVMFactory, NothingModelVMFactory>();
            services.AddScoped<IUpdateNothingModelVMFactory, UpdateNothingModelVMFactory>();
            services.AddScoped<NothingRpcApiClientStrategy>();
            services.AddScoped<NothingWebApiClientStrategy>();
            services.AddScoped(_ => Mock.Of<ICreateNothingModelView>());
            services.AddScoped(_ => Mock.Of<IDeleteNothingModelView>());
            services.AddScoped(_ => Mock.Of<IMainWindow>());
            services.AddScoped(_ => Mock.Of<IUpdateNothingModelView>());
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
        await Process.Start("docker", "volume remove -f wpf_app_test_wpf_app_test_postgres_nothing_grpc_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f wpf_app_test_wpf_app_test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}