using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingServices.WPFApp.Commands;
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
    public async Task OpenApiSelectionCommand_Success()
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
                var command = host.Services.GetRequiredService<OpenApiSelectionCommand>();
                command.Execute(null);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var result = mainWindowVM.ApiSelectionVM.Active;

            //Assert
            Assert.True(result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task OpenCreateNothingModelCommand_Success()
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
                var command = host.Services.GetRequiredService<IOpenCreateNothingModelCommand>();
                command.Execute(null);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var dialogVM = host.Services.GetRequiredService<IDialogVM>();
            var result = dialogVM.Content;

            //Assert
            Assert.IsType<CreateNothingModelVM>(result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task GRpcApi_OpenNothingModelsListCommand_Success()
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
                var strategy = hostThread.Services.GetRequiredService<NothingRpcApiClientStrategy>();
                var command = host.Services.GetRequiredService<IOpenNothingModelsListCommand>();
                command.Execute(strategy);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
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
    public async Task GRpcApi_OpenUpdateNothingModelCommand_Success()
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
                var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
                var nothingModel = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                    ?? throw new NullReferenceException();
                var command = host.Services.GetRequiredService<IOpenUpdateNothingModelCommand>();
                command.Execute(nothingModel);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var dialogVM = host.Services.GetRequiredService<IDialogVM>();
            var result = dialogVM.Content;

            //Assert
            Assert.IsType<UpdateNothingModelVM>(result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task GRpcApi_OpenDeleteNothingModelCommand_Success()
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
                var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
                var nothingModel = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                    ?? throw new NullReferenceException();
                var command = host.Services.GetRequiredService<IOpenDeleteNothingModelCommand>();
                command.Execute(nothingModel);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var dialogVM = host.Services.GetRequiredService<IDialogVM>();
            var result = dialogVM.Content;

            //Assert
            Assert.IsType<DeleteNothingModelVM>(result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task GRpcApi_CreateCommand_Success()
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
                var createNothingModelVM = host.Services.GetRequiredService<ICreateNothingModelVMFactory>().Create();
                createNothingModelVM.Name = "New model";
                var command = host.Services.GetRequiredService<ICreateCommand>();
                command.Execute(createNothingModelVM);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var result = mainWindowVM.NothingModelsListVM.NothingModels?.Last().Name;

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
    public async Task GRpcApi_UpdateCommand_Success()
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
                var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
                var nothingModel = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                    ?? throw new NullReferenceException();
                var updateNothingModelVM = host.Services.GetRequiredService<IUpdateNothingModelVMFactory>().Create(nothingModel);
                updateNothingModelVM.Name = "New Name";
                var command = host.Services.GetRequiredService<IUpdateCommand>();
                command.Execute(updateNothingModelVM);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var result = mainWindowVM.NothingModelsListVM.NothingModels?.Single().Name;

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
    public async Task GRpcApi_DeleteCommand_Success()
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
                var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
                var nothingModel = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                    ?? throw new NullReferenceException();
                var deleteNothingModelVM = host.Services.GetRequiredService<IDeleteNothingModelVMFactory>().Create(nothingModel);
                var command = host.Services.GetRequiredService<IDeleteCommand>();
                command.Execute(deleteNothingModelVM);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var result = mainWindowVM.NothingModelsListVM.NothingModels
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
    public async Task WebApi_OpenNothingModelsListCommand_Success()
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
                var strategy = hostThread.Services.GetRequiredService<NothingWebApiClientStrategy>();
                var command = host.Services.GetRequiredService<IOpenNothingModelsListCommand>();
                command.Execute(strategy);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
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
    public async Task WebApi_OpenUpdateNothingModelCommand_Success()
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
                var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
                var nothingModel = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                    ?? throw new NullReferenceException();
                var command = host.Services.GetRequiredService<IOpenUpdateNothingModelCommand>();
                command.Execute(nothingModel);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var dialogVM = host.Services.GetRequiredService<IDialogVM>();
            var result = dialogVM.Content;

            //Assert
            Assert.IsType<UpdateNothingModelVM>(result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task WebApi_OpenDeleteNothingModelCommand_Success()
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
                var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
                var nothingModel = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                    ?? throw new NullReferenceException();
                var command = host.Services.GetRequiredService<IOpenDeleteNothingModelCommand>();
                command.Execute(nothingModel);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var dialogVM = host.Services.GetRequiredService<IDialogVM>();
            var result = dialogVM.Content;

            //Assert
            Assert.IsType<DeleteNothingModelVM>(result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task WebApi_CreateCommand_Success()
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
                var createNothingModelVM = host.Services.GetRequiredService<ICreateNothingModelVMFactory>().Create();
                createNothingModelVM.Name = "New model";
                var command = host.Services.GetRequiredService<ICreateCommand>();
                command.Execute(createNothingModelVM);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var result = mainWindowVM.NothingModelsListVM.NothingModels?.Last().Name;

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
    public async Task WebApi_UpdateCommand_Success()
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
                var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
                var nothingModel = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                    ?? throw new NullReferenceException();
                var updateNothingModelVM = host.Services.GetRequiredService<IUpdateNothingModelVMFactory>().Create(nothingModel);
                updateNothingModelVM.Name = "New Name";
                var command = host.Services.GetRequiredService<IUpdateCommand>();
                command.Execute(updateNothingModelVM);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var result = mainWindowVM.NothingModelsListVM.NothingModels?.Single().Name;

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
    public async Task WebApi_DeleteCommand_Success()
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
                var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
                var nothingModel = mainWindowVM.NothingModelsListVM.NothingModels?.Single()
                    ?? throw new NullReferenceException();
                var deleteNothingModelVM = host.Services.GetRequiredService<IDeleteNothingModelVMFactory>().Create(nothingModel);
                var command = host.Services.GetRequiredService<IDeleteCommand>();
                command.Execute(deleteNothingModelVM);
            });
            uiThread.SetApartmentState(ApartmentState.STA);

            //Act
            uiThread.Start(host);
            Thread.Sleep(2000);
            var mainWindowVM = host.Services.GetRequiredService<IMainWindowVM>();
            var result = mainWindowVM.NothingModelsListVM.NothingModels
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
        await Process.Start("docker", "volume remove -f wpf_app_test_wpf_app_test_postgres_nothing_grpc_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f wpf_app_test_wpf_app_test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}