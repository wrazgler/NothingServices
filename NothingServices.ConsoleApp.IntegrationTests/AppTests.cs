using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingServices.ConsoleApp.Dtos;
using NothingServices.ConsoleApp.Extensions;
using NothingServices.ConsoleApp.Services;

namespace NothingServices.ConsoleApp.IntegrationTests;

public class AppTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    [Fact]
    public async Task Run_1_1_e_e_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var stringBuilder = new StringBuilder();
            var host = GetHost();
            SetupConsole(stringBuilder, "1", "1", "e", "e");

            //Act
            await host.RunAsync();
            var result = stringBuilder.ToString();

            //Assert
            var nothingModels = new List<NothingModelDto>()
            {
                new()
                {
                    Id = 1,
                    Name = "Test",
                }
            };
            var expected = GetAssert(JsonSerializer.Serialize(nothingModels, _jsonSerializerOptions));
            Assert.Equal(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Run_1_2_1_e_e_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var stringBuilder = new StringBuilder();
            var host = GetHost();
            SetupConsole(stringBuilder, "1", "2", "1", "e", "e");

            //Act
            await host.RunAsync();
            var result = stringBuilder.ToString();

            //Assert
            var nothingModel = new NothingModelDto()
            {
                Id = 1,
                Name = "Test",
            };
            var expected = GetAssert(string.Concat(
                "Введите идентификатор",
                Environment.NewLine,
                JsonSerializer.Serialize(nothingModel, _jsonSerializerOptions)));
            Assert.Equal(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Run_1_3_1_e_e_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var stringBuilder = new StringBuilder();
            var host = GetHost();
            var newName = "New model";
            SetupConsole(stringBuilder, "1", "3", newName, "e", "e");

            //Act
            await host.RunAsync();
            var result = stringBuilder.ToString();

            //Assert
            var nothingModel = new NothingModelDto()
            {
                Id = 2,
                Name = newName,
            };
            var expected = GetAssert(string.Concat(
                "Введите имя модели",
                Environment.NewLine,
                JsonSerializer.Serialize(nothingModel, _jsonSerializerOptions)));
            Assert.Equal(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Run_1_4_1_e_e_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var stringBuilder = new StringBuilder();
            var host = GetHost();
            var id = 1;
            var newName = "New name";
            SetupConsole(stringBuilder, "1", "4", id.ToString(), newName, "e", "e");

            //Act
            await host.RunAsync();
            var result = stringBuilder.ToString();

            //Assert
            var nothingModel = new NothingModelDto()
            {
                Id = id,
                Name = newName,
            };
            var expected = GetAssert(string.Concat(
                "Введите идентификатор",
                Environment.NewLine,
                "Введите имя модели",
                Environment.NewLine,
                JsonSerializer.Serialize(nothingModel, _jsonSerializerOptions)));
            Assert.Equal(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Run_1_5_1_e_e_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var stringBuilder = new StringBuilder();
            var host = GetHost();
            SetupConsole(stringBuilder, "1", "5", "1", "e", "e");

            //Act
            await host.RunAsync();
            var result = stringBuilder.ToString();

            //Assert
            var nothingModel = new NothingModelDto()
            {
                Id = 1,
                Name = "Test",
            };
            var expected = GetAssert(string.Concat(
                "Введите идентификатор",
                Environment.NewLine,
                JsonSerializer.Serialize(nothingModel, _jsonSerializerOptions)));
            Assert.Equal(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Run_2_1_e_e_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var stringBuilder = new StringBuilder();
            var host = GetHost();
            SetupConsole(stringBuilder, "2", "1", "e", "e");

            //Act
            await host.RunAsync();
            var result = stringBuilder.ToString();

            //Assert
            var nothingModels = new List<NothingModelWebDto>()
            {
                new()
                {
                    Id = 1,
                    Name = "Test",
                }
            };
            var expected = GetAssert(JsonSerializer.Serialize(nothingModels, _jsonSerializerOptions));
            Assert.Equal(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Run_2_2_1_e_e_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var stringBuilder = new StringBuilder();
            var host = GetHost();
            SetupConsole(stringBuilder, "2", "2", "1", "e", "e");

            //Act
            await host.RunAsync();
            var result = stringBuilder.ToString();

            //Assert
            var nothingModel = new NothingModelWebDto()
            {
                Id = 1,
                Name = "Test",
            };
            var expected = GetAssert(string.Concat(
                "Введите идентификатор",
                Environment.NewLine,
                JsonSerializer.Serialize(nothingModel, _jsonSerializerOptions)));
            Assert.Equal(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Run_2_3_1_e_e_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var stringBuilder = new StringBuilder();
            var host = GetHost();
            var newName = "New model";
            SetupConsole(stringBuilder, "2", "3", newName, "e", "e");

            //Act
            await host.RunAsync();
            var result = stringBuilder.ToString();

            //Assert
            var nothingModel = new NothingModelWebDto()
            {
                Id = 2,
                Name = newName,
            };
            var expected = GetAssert(string.Concat(
                "Введите имя модели",
                Environment.NewLine,
                JsonSerializer.Serialize(nothingModel, _jsonSerializerOptions)));
            Assert.Equal(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Run_2_4_1_e_e_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var stringBuilder = new StringBuilder();
            var host = GetHost();
            var id = 1;
            var newName = "New name";
            SetupConsole(stringBuilder, "2", "4", id.ToString(), newName, "e", "e");

            //Act
            await host.RunAsync();
            var result = stringBuilder.ToString();

            //Assert
            var nothingModel = new NothingModelWebDto()
            {
                Id = id,
                Name = newName,
            };
            var expected = GetAssert(string.Concat(
                "Введите идентификатор",
                Environment.NewLine,
                "Введите имя модели",
                Environment.NewLine,
                JsonSerializer.Serialize(nothingModel, _jsonSerializerOptions)));
            Assert.Equal(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    [Fact]
    public async Task Run_2_5_1_e_e_Equal()
    {
        try
        {
            //Arrange
            await StopApp(0);
            await StartApp();
            var stringBuilder = new StringBuilder();
            var host = GetHost();
            SetupConsole(stringBuilder, "2", "5", "1", "e", "e");

            //Act
            await host.RunAsync();
            var result = stringBuilder.ToString();

            //Assert
            var nothingModel = new NothingModelWebDto()
            {
                Id = 1,
                Name = "Test",
            };
            var expected = GetAssert(string.Concat(
                "Введите идентификатор",
                Environment.NewLine,
                JsonSerializer.Serialize(nothingModel, _jsonSerializerOptions)));
            Assert.Equal(expected, result);
        }
        finally
        {
            await StopApp();
        }
    }

    private static string GetAssert(string methodResult)
    {
        return new StringBuilder()
            .AppendLine("Выберите клиент")
            .AppendLine("1: NothingRpcApi")
            .AppendLine("2: NothingWebApi")
            .AppendLine("e: Завершить работу")
            .AppendLine()
            .AppendLine("Выберите действие")
            .AppendLine("1: Получить список моделей")
            .AppendLine("2: Получить модель с указанным идентификатором")
            .AppendLine("3: Создать новую модель")
            .AppendLine("4: Обновить существующую модель")
            .AppendLine("5: Удалить модель с указанным идентификатором")
            .AppendLine("e: Вернуться назад")
            .AppendLine()
            .AppendLine(methodResult)
            .AppendLine("Выберите действие")
            .AppendLine("1: Получить список моделей")
            .AppendLine("2: Получить модель с указанным идентификатором")
            .AppendLine("3: Создать новую модель")
            .AppendLine("4: Обновить существующую модель")
            .AppendLine("5: Удалить модель с указанным идентификатором")
            .AppendLine("e: Вернуться назад")
            .AppendLine()
            .AppendLine("Выберите клиент")
            .AppendLine("1: NothingRpcApi")
            .AppendLine("2: NothingWebApi")
            .AppendLine("e: Завершить работу")
            .AppendLine()
            .AppendLine("Работа приложения завершена")
            .ToString();
    }

    private static IHost GetHost()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.App.Testing.json")
            .Build();
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureServices((_, services) =>
        {
            services.AddAppConfigs(configuration);
            services.AddAppHttpClient(configuration);
            services.AddAppClients(configuration);
            services.AddAppServices();
            services.AddHostedService<HostedService>();
        });
        var host = hostBuilder.Build();
        return host;
    }

    private static void SetupConsole(StringBuilder stringBuilder, params string[] input)
    {
        var enumerator = input.GetEnumerator();
        var textReaderMock = new Mock<TextReader>();
        textReaderMock
            .Setup(textReader => textReader.ReadLine())
            .Returns(() =>
            {
                var next = enumerator.MoveNext();
                return next ? enumerator.Current?.ToString() : string.Empty;
            });
        Console.SetIn(textReaderMock.Object);
        var textWriterMock = new Mock<TextWriter>();
        textWriterMock
            .Setup(textWriter => textWriter.WriteLine(It.IsAny<string>()))
            .Callback<string>(x =>
            {
                stringBuilder.AppendLine(x);
            });
        Console.SetOut(textWriterMock.Object);
    }

    private static async Task StartApp(int delay = 15000)
    {
        var projectPath = Path.GetFullPath("../../../");
        var dockerFilePath = Path.Combine(projectPath, "docker-compose.app-test.yml");
        await Process.Start("docker", $"compose -f {dockerFilePath} up -d").WaitForExitAsync();
        await Task.Delay(delay);
    }

    private static async Task StopApp(int beforeDelay = 20000, int afterDelay = 2000)
    {
        await Task.Delay(beforeDelay);
        await Process.Start("docker", "container remove -f -v console_app_test_postgres_nothing_grpc_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f -v console_app_test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f console_app_test_nothing_grpc_api")
            .WaitForExitAsync();
        await Process.Start("docker", "container remove -f console_app_test_nothing_web_api")
            .WaitForExitAsync();
        await Process.Start("docker", "image remove -f console_app_test_nothing_grpc_api")
            .WaitForExitAsync();
        await Process.Start("docker", "image remove -f console_app_test_nothing_web_api")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f console_app_test_console_app_test_postgres_nothing_grpc_api_db")
            .WaitForExitAsync();
        await Process.Start("docker", "volume remove -f console_app_test_console_app_test_postgres_nothing_web_api_db")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}