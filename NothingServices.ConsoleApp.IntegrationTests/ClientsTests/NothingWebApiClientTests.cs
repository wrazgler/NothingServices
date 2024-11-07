using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingServices.ConsoleApp.Clients;
using NothingServices.ConsoleApp.Dtos;
using NothingServices.ConsoleApp.Extensions;

namespace NothingServices.ConsoleApp.IntegrationTests.ClientsTests;

public class NothingWebApiClientTests
{
    [Fact]
    public async Task GetAsync_Equivalent()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var nothingWebApiClient = GetNothingWebApiClient();

            //Act
            var result = await nothingWebApiClient.GetAsync();

            //Assert
            var assert = new NothingModelWebDto[]
            {
                new()
                {
                    Id = 1,
                    Name = "Test",
                }
            };
            Assert.Equivalent(assert, result, true);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task GetAsync_Id_Equivalent()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var nothingWebApiClient = GetNothingWebApiClient();
            var id = 1;

            //Act
            var result = await nothingWebApiClient.GetAsync(id);

            //Assert
            var assert = new NothingModelWebDto()
            {
                Id = id,
                Name = "Test",
            };
            Assert.Equivalent(assert, result, true);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task CreateAsync_Equivalent()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var nothingWebApiClient = GetNothingWebApiClient();
            var createNothingModelWebDto = new CreateNothingModelWebDto()
            {
                Name = "New model",
            };

            //Act
            var result = await nothingWebApiClient.CreateAsync(createNothingModelWebDto);

            //Assert
            var assert = new NothingModelWebDto()
            {
                Id = 2,
                Name = createNothingModelWebDto.Name,
            };
            Assert.Equivalent(assert, result, true);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task UpdateAsync_Equivalent()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var nothingWebApiClient = GetNothingWebApiClient();
            var updateNothingModelWebDto = new UpdateNothingModelWebDto()
            {
                Id = 1,
                Name = "New name",
            };

            //Act
            var result = await nothingWebApiClient.UpdateAsync(updateNothingModelWebDto);

            //Assert
            var assert = new NothingModelWebDto()
            {
                Id = updateNothingModelWebDto.Id,
                Name = updateNothingModelWebDto.Name,
            };
            Assert.Equivalent(assert, result, true);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task DeleteAsync_Equivalent()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var nothingWebApiClient = GetNothingWebApiClient();
            var id = 1;

            //Act
            var result = await nothingWebApiClient.DeleteAsync(id);

            //Assert
            var assert = new NothingModelWebDto()
            {
                Id = id,
                Name = "Test",
            };
            Assert.Equivalent(assert, result, true);
            await StopApp();
        }
        finally
        {
            await StopApp();
        }
    }

    private static INothingWebApiClient GetNothingWebApiClient()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.NothingWebApiClientTesting.json")
            .Build();
        var services = new ServiceCollection();
        services.AddAppConfigs(configuration);
        services.AddAppHttpClient(configuration);
        services.AddTransient<INothingWebApiClient, NothingWebApiClient>();
        var nothingWebApiClient= services
            .BuildServiceProvider()
            .GetRequiredService<INothingWebApiClient>();
        return nothingWebApiClient;
    }

    private static async Task StartApp(int delay = 10000)
    {
        var projectPath = Path.GetFullPath("../../../");
        var dockerFilePath = Path.Combine(projectPath,  "docker-compose.nothing-web-api-client.yml");
        await Process.Start("docker", $"compose -f {dockerFilePath} up -d").WaitForExitAsync();
        await Task.Delay(delay);
    }

    private static async Task StopApp(int beforeDelay = 10000, int afterDelay = 2000)
    {
        await Task.Delay(beforeDelay);
        await Process.Start("docker", "container remove -f -v console_nothing_web_api_test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f console_nothing_web_api_test_nothing_web_api")
            .WaitForExitAsync();
        await Process.Start("docker", "image remove -f console_nothing_web_api_test_nothing_web_api")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f console_nothing_web_api_test_nothing_services_console_nothing_web_api_test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}