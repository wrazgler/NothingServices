using System.Diagnostics;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Extensions;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.IntegrationTests.StrategiesTests;

public class NothingWebApiClientStrategyTests
{
    [Fact]
    public async Task GetNothingModels_Equivalent()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy();

            //Act
            var result = await nothingWebApiClientStrategy.GetNothingModels();

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
    public async Task CreateNothingModel_Equivalent()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy();
            var createNothingModelVM = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
            {
                Name = "Name"
            };

            //Act
            var nothingModelVM = await nothingWebApiClientStrategy.CreateNothingModel(createNothingModelVM);
            var result = nothingModelVM.Name;

            //Assert
            var expected = "Name";
            Assert.Equal(expected, result);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    private static NothingWebApiClientStrategy GetNothingWebApiClientStrategy()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.NothingWebApiClientStrategy.Testing.json")
            .Build();
        var nothingWebApiClientStrategy = new ServiceCollection()
            .AddAppAutoMapper()
            .AddAppConfigs(configuration)
            .AddAppHttpClient(configuration)
            .AddAppClients(configuration)
            .AddAppServices()
            .AddAppViewModels()
            .AddTransient(_ => Mock.Of<IDeleteNothingModelView>())
            .AddTransient(_ => Mock.Of<IUpdateNothingModelView>())
            .AddTransient(_ => Dispatcher.CurrentDispatcher)
            .BuildServiceProvider()
            .GetRequiredService<NothingWebApiClientStrategy>();
        return nothingWebApiClientStrategy;
    }

    private static async Task StartApp(int delay = 10000)
    {
        var projectPath = Path.GetFullPath("../../../");
        var dockerFilePath = Path.Combine(projectPath, "docker-compose.nothing-web-api-client-strategy-test.yml");
        await Process.Start("docker", $"compose -f {dockerFilePath} up -d").WaitForExitAsync();
        await Task.Delay(delay);
    }

    private static async Task StopApp(int beforeDelay = 10000, int afterDelay = 2000)
    {
        await Task.Delay(beforeDelay);
        await Process.Start("docker", "container remove -f -v wpf_nothing_web_api_client_strategy_test_postgres_db")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f wpf_nothing_web_api_client_strategy_test_nothing_web_api")
            .WaitForExitAsync();
        await Process.Start("docker", "image remove -f wpf_nothing_web_api_client_strategy_test_nothing_web_api")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f wpf_nothing_web_api_client_strategy_nothing_services_wpf_app_test_postgres_db")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}