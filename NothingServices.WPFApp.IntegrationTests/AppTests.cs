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
    public async Task CreateCommand_GRpcApi_Success()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var nothingModelsListVM = host.Services.GetRequiredService<NothingModelsListVM>();
            var mainWindowManager = host.Services.GetRequiredService<IMainWindowManager>();
            mainWindowManager.Strategy = host.Services.GetRequiredService<NothingRpcApiClientStrategy>();
            var createNothingModelVM = host.Services.GetRequiredService<ICreateNothingModelVMFactory>().Create();
            createNothingModelVM.Name = "New model";
            var command = host.Services.GetRequiredService<ICreateCommand>();

            //Act
            command.Execute(createNothingModelVM);
            await Task.Delay(2000);
            var result = nothingModelsListVM.NothingModels?.Last().Name;

            //Assert
            var expected = "New model";
            Assert.Equal(expected, result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task CreateCommand_WebApi_Success()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var nothingModelsListVM = host.Services.GetRequiredService<NothingModelsListVM>();
            var mainWindowManager = host.Services.GetRequiredService<IMainWindowManager>();
            mainWindowManager.Strategy = host.Services.GetRequiredService<NothingWebApiClientStrategy>();
            var createNothingModelVM = host.Services.GetRequiredService<ICreateNothingModelVMFactory>().Create();
            createNothingModelVM.Name = "New model";
            var command = host.Services.GetRequiredService<ICreateCommand>();

            //Act
            command.Execute(createNothingModelVM);
            await Task.Delay(2000);
            var result = nothingModelsListVM.NothingModels?.Last().Name;

            //Assert
            var expected = "New model";
            Assert.Equal(expected, result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task DeleteCommand_GRpcApi_Success()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var nothingModelsListVM = host.Services.GetRequiredService<NothingModelsListVM>();
            var mainWindowManager = host.Services.GetRequiredService<IMainWindowManager>();
            mainWindowManager.Strategy = host.Services.GetRequiredService<NothingRpcApiClientStrategy>();
            mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            var nothingModelVM = nothingModelsListVM.NothingModels?.Single()
                ?? throw new NullReferenceException();
            var deleteNothingModelVM = host.Services.GetRequiredService<IDeleteNothingModelVMFactory>().Create(nothingModelVM);
            var command = host.Services.GetRequiredService<IDeleteCommand>();

            //Act
            command.Execute(deleteNothingModelVM);
            await Task.Delay(2000);
            var result = nothingModelsListVM.NothingModels
                ?? throw new NullReferenceException();

            //Assert
            Assert.Empty(result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task DeleteCommand_WebApi_Success()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var nothingModelsListVM = host.Services.GetRequiredService<NothingModelsListVM>();
            var mainWindowManager = host.Services.GetRequiredService<IMainWindowManager>();
            mainWindowManager.Strategy = host.Services.GetRequiredService<NothingWebApiClientStrategy>();
            mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            var nothingModelVM = nothingModelsListVM.NothingModels?.Single()
                ?? throw new NullReferenceException();
            var deleteNothingModelVM = host.Services.GetRequiredService<IDeleteNothingModelVMFactory>().Create(nothingModelVM);
            var command = host.Services.GetRequiredService<IDeleteCommand>();

            //Act
            command.Execute(deleteNothingModelVM);
            await Task.Delay(2000);
            var result = nothingModelsListVM.NothingModels
                ?? throw new NullReferenceException();

            //Assert
            Assert.Empty(result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

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

    [Fact]
    public async Task OpenDeleteNothingModelCommand_GRpcApi_DeleteNothingModelVM_Id_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var mainWindowManager = host.Services.GetRequiredService<IMainWindowManager>();
            mainWindowManager.Strategy = host.Services.GetRequiredService<NothingRpcApiClientStrategy>();
            mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            var nothingModelVM = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                ?? throw new NullReferenceException();
            var command = host.Services.GetRequiredService<IOpenDeleteNothingModelCommand>();

            //Act
            command.Execute(nothingModelVM);
            var dialogVM = host.Services.GetRequiredService<IDialogVM>();
            var deleteNothingModelVM = dialogVM.Content?.DataContext as DeleteNothingModelVM;
            var result = deleteNothingModelVM?.Id;

            //Assert
            var expected = nothingModelVM.Id;
            Assert.Equal(expected, result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task OpenDeleteNothingModelCommand_WebApi_DeleteNothingModelVM_Id_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var startupService = host.Services.GetRequiredService<StartupService>();
            startupService.Start();
            var mainWindowManager = host.Services.GetRequiredService<IMainWindowManager>();
            mainWindowManager.Strategy = host.Services.GetRequiredService<NothingWebApiClientStrategy>();
            mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var nothingModelVM = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                ?? throw new NullReferenceException();
            var command = host.Services.GetRequiredService<IOpenDeleteNothingModelCommand>();
            command.Execute(nothingModelVM);

            //Act
            command.Execute(nothingModelVM);
            var dialogVM = host.Services.GetRequiredService<IDialogVM>();
            var deleteNothingModelVM = dialogVM.Content?.DataContext as DeleteNothingModelVM;
            var result = deleteNothingModelVM?.Id;

            //Assert
            var expected = nothingModelVM.Id;
            Assert.Equal(expected, result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task OpenNothingModelsListCommand_GRpcApi_NothingModels_Single()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var strategy = host.Services.GetRequiredService<NothingRpcApiClientStrategy>();
            var command = host.Services.GetRequiredService<IOpenNothingModelsListCommand>();

            //Act
            command.Execute(strategy);
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
    public async Task OpenNothingModelsListCommand_WebApi_NothingModels_Single()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var strategy = host.Services.GetRequiredService<NothingWebApiClientStrategy>();
            var command = host.Services.GetRequiredService<IOpenNothingModelsListCommand>();

            //Act
            command.Execute(strategy);
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
    public async Task OpenUpdateNothingModelCommand_GRpcApi_UpdateNothingModelVM_Name_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var mainWindowManager = host.Services.GetRequiredService<IMainWindowManager>();
            mainWindowManager.Strategy = host.Services.GetRequiredService<NothingRpcApiClientStrategy>();
            mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            var nothingModelVM = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                ?? throw new NullReferenceException();
            var command = host.Services.GetRequiredService<IOpenUpdateNothingModelCommand>();

            //Act
            command.Execute(nothingModelVM);
            var dialogVM = host.Services.GetRequiredService<IDialogVM>();
            var updateNothingModelVM = dialogVM.Content?.DataContext as UpdateNothingModelVM;
            var result = updateNothingModelVM?.Name;

            //Assert
            var expected = nothingModelVM.Name;
            Assert.Equal(expected, result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task OpenUpdateNothingModelCommand_WebApi_UpdateNothingModelVM_Name_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var mainWindowManager = host.Services.GetRequiredService<IMainWindowManager>();
            mainWindowManager.Strategy = host.Services.GetRequiredService<NothingWebApiClientStrategy>();
            mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            var nothingModelVM = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                ?? throw new NullReferenceException();
            var command = host.Services.GetRequiredService<IOpenUpdateNothingModelCommand>();

            //Act
            command.Execute(nothingModelVM);
            var dialogVM = host.Services.GetRequiredService<IDialogVM>();
            var updateNothingModelVM = dialogVM.Content?.DataContext as UpdateNothingModelVM;
            var result = updateNothingModelVM?.Name;

            //Assert
            var expected = nothingModelVM.Name;
            Assert.Equal(expected, result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task UpdateCommand_GRpcApi_Success()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var nothingModelsListVM = host.Services.GetRequiredService<NothingModelsListVM>();
            var mainWindowManager = host.Services.GetRequiredService<IMainWindowManager>();
            mainWindowManager.Strategy = host.Services.GetRequiredService<NothingRpcApiClientStrategy>();
            mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            var nothingModelVM = nothingModelsListVM.NothingModels?.Single()
                ?? throw new NullReferenceException();
            var updateNothingModelVM = host.Services.GetRequiredService<IUpdateNothingModelVMFactory>().Create(nothingModelVM);
            updateNothingModelVM.Name = "New Name";
            var command = host.Services.GetRequiredService<IUpdateCommand>();

            //Act
            command.Execute(updateNothingModelVM);
            await Task.Delay(2000);
            var result = nothingModelsListVM.NothingModels?.Single().Name;

            //Assert
            var expected = "New Name";
            Assert.Equal(expected, result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task UpdateCommand_WebApi_Success()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var host = GetHost();
            var nothingModelsListVM = host.Services.GetRequiredService<NothingModelsListVM>();
            var mainWindowManager = host.Services.GetRequiredService<IMainWindowManager>();
            mainWindowManager.Strategy = host.Services.GetRequiredService<NothingWebApiClientStrategy>();
            mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            var nothingModelVM = nothingModelsListVM.NothingModels?.Single()
                ?? throw new NullReferenceException();
            var updateNothingModelVM = host.Services.GetRequiredService<IUpdateNothingModelVMFactory>().Create(nothingModelVM);
            updateNothingModelVM.Name = "New Name";
            var command = host.Services.GetRequiredService<IUpdateCommand>();

            //Act
            command.Execute(updateNothingModelVM);
            await Task.Delay(2000);
            var result = nothingModelsListVM.NothingModels?.Single().Name;

            //Assert
            var expected = "New Name";
            Assert.Equal(expected, result);
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