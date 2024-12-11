using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Clients;
using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.Extensions;
using NothingWebApi.DbContexts;

namespace NothingServices.WPFApp.IntegrationTests.ClientsTests;

public class NothingWebApiClientTests
{
    [Fact]
    public async Task Get_Equivalent()
    {
        await StopApp();
        var process = await StartApp();
        try
        {
            //Arrange
            var nothingWebApiClient = GetNothingWebApiClient();

            //Act
            var result = await nothingWebApiClient.Get();

            //Assert
            var expected = new NothingModelWebDto[]
            {
                new()
                {
                    Id = 1,
                    Name = "Test",
                }
            };
            Assert.Equivalent(expected, result, true);
            await StopApp(process);
        }
        finally
        {
            await StopApp(process);
        }
    }

    [Fact]
    public async Task Get_Id_Equivalent()
    {
        await StopApp();
        var process = await StartApp();
        try
        {
            //Arrange
            var nothingWebApiClient = GetNothingWebApiClient();
            var id = 1;

            //Act
            var result = await nothingWebApiClient.Get(id);

            //Assert
            var expected = new NothingModelWebDto()
            {
                Id = id,
                Name = "Test",
            };
            Assert.Equivalent(expected, result, true);
            await StopApp(process);
        }
        finally
        {
            await StopApp(process);
        }
    }

    [Fact]
    public async Task Create_Equivalent()
    {
        await StopApp();
        var process = await StartApp();
        try
        {
            //Arrange
            var nothingWebApiClient = GetNothingWebApiClient();
            var createNothingModelWebDto = new CreateNothingModelWebDto()
            {
                Name = "New model",
            };

            //Act
            var result = await nothingWebApiClient.Create(createNothingModelWebDto);

            //Assert
            var expected = new NothingModelWebDto()
            {
                Id = 2,
                Name = createNothingModelWebDto.Name,
            };
            Assert.Equivalent(expected, result, true);
            await StopApp(process);
        }
        finally
        {
            await StopApp(process);
        }
    }

    [Fact]
    public async Task Update_Equivalent()
    {
        await StopApp();
        var process = await StartApp();
        try
        {
            //Arrange
            var nothingWebApiClient = GetNothingWebApiClient();
            var updateNothingModelWebDto = new UpdateNothingModelWebDto()
            {
                Id = 1,
                Name = "New name",
            };

            //Act
            var result = await nothingWebApiClient.Update(updateNothingModelWebDto);

            //Assert
            var expected = new NothingModelWebDto()
            {
                Id = updateNothingModelWebDto.Id,
                Name = updateNothingModelWebDto.Name,
            };
            Assert.Equivalent(expected, result, true);
            await StopApp(process);
        }
        finally
        {
            await StopApp(process);
        }
    }

    [Fact]
    public async Task Delete_Equivalent()
    {
        await StopApp();
        var process = await StartApp();
        try
        {
            //Arrange
            var nothingWebApiClient = GetNothingWebApiClient();
            var id = 1;

            //Act
            var result = await nothingWebApiClient.Delete(id);

            //Assert
            var expected = new NothingModelWebDto()
            {
                Id = id,
                Name = "Test",
            };
            Assert.Equivalent(expected, result, true);
            await StopApp(process);
        }
        finally
        {
            await StopApp(process);
        }
    }

    private static INothingWebApiClient GetNothingWebApiClient()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.NothingWebApiClient.Testing.json")
            .Build();
        var services = new ServiceCollection();
        services.AddAppConfigs(configuration);
        services.AddAppHttpClient(configuration);
        services.AddTransient<INothingWebApiClient, NothingWebApiClient>();
        var nothingWebApiClient = services
            .BuildServiceProvider()
            .GetRequiredService<INothingWebApiClient>();
        return nothingWebApiClient;
    }

    private static async Task<Process> StartApp()
    {
        var path = Path.GetFullPath("../../../../");
        var projectPath = Path.Combine(path, "NothingWebApi", "NothingWebApi.csproj");
        await Process.Start("dotnet", $"build {projectPath} --configuration Release --framework net8.0")
            .WaitForExitAsync();
        await Task.Delay(2000);
        var appPath = Path.Combine(path, "NothingWebApi", "bin", "Release", "net8.0", "NothingWebApi.dll");
        var argsBuilder = new StringBuilder();
        argsBuilder.Append($" \"{appPath}\"");
        argsBuilder.Append(" -e POSTGRES_HOST=localhost");
        argsBuilder.Append(" -e POSTGRES_PORT=5432");
        argsBuilder.Append(" -e POSTGRES_DB=nothing_web_api_db");
        argsBuilder.Append(" -e POSTGRES_USER=postgres");
        argsBuilder.Append(" -e POSTGRES_PASSWORD=postgres");
        argsBuilder.Append(" --urls http://localhost:9069");
        var args = argsBuilder.ToString();
        var process = Process.Start("dotnet", args);
        await Task.Delay(5000);
        return process;
    }

    private static async Task StopApp(Process? process = null)
    {
        if (process != null)
        {
            await Task.Delay(2000);
            process.Kill();
            await process.WaitForExitAsync();
        }

        var dbContext = GetDbContext();
        await dbContext.Database.EnsureDeletedAsync();
    }

    private static NothingWebApiDbContext GetDbContext()
    {
        var connectionString = "Host=localhost;Port=5432;Database=nothing_web_api_db;Username=postgres;Password=postgres";
        var optionsBuilder = new DbContextOptionsBuilder<NothingWebApiDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        var context = new NothingWebApiDbContext(optionsBuilder.Options);
        return context;
    }
}