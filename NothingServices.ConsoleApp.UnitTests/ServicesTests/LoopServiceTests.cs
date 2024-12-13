using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NothingServices.ConsoleApp.Clients;
using NothingServices.ConsoleApp.Services;
using NothingServices.ConsoleApp.Strategies;

namespace NothingServices.ConsoleApp.UnitTests.ServicesTests;

public class LoopServiceTests
{
    [Fact]
    public async Task DoWork_1_1_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "1", "1", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = GetAssert("NothingRpcApiClientStrategy.GetNothingModels");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_1_2_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "1", "2", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = GetAssert("NothingRpcApiClientStrategy.GetNothingModel");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_1_3_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "1", "3", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = GetAssert("NothingRpcApiClientStrategy.CreateNothingModel");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_1_4_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "1", "4", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = GetAssert("NothingRpcApiClientStrategy.UpdateNothingModel");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_1_5_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "1", "5", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = GetAssert("NothingRpcApiClientStrategy.DeleteNothingModel");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_2_1_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "2", "1", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = GetAssert("NothingWebApiClientStrategy.GetNothingModels");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_2_2_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "2", "2", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = GetAssert("NothingWebApiClientStrategy.GetNothingModel");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_2_3_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "2", "3", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = GetAssert("NothingWebApiClientStrategy.CreateNothingModel");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_2_4_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "2", "4", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = GetAssert("NothingWebApiClientStrategy.UpdateNothingModel");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_2_5_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "2", "5", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = GetAssert("NothingWebApiClientStrategy.DeleteNothingModel");
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = new StringBuilder()
            .AppendLine("Выберите клиент")
            .AppendLine("1: NothingRpcApi")
            .AppendLine("2: NothingWebApi")
            .AppendLine("e: Завершить работу")
            .AppendLine()
            .AppendLine("Работа приложения завершена")
            .ToString();
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_Cancelled_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder);
        var loopService = GetLoopService(consoleServiceMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(0);

        //Act
        await loopService.DoWork(cancellationTokenSource.Token);
        var result = stringBuilder.ToString();

        //Assert
        var expected = new StringBuilder()
            .AppendLine("Работа приложения завершена")
            .ToString();
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_error_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "error", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = new StringBuilder()
            .AppendLine("Выберите клиент")
            .AppendLine("1: NothingRpcApi")
            .AppendLine("2: NothingWebApi")
            .AppendLine("e: Завершить работу")
            .AppendLine()
            .AppendLine("Отсутствует соответствие выбранной команде: error")
            .AppendLine("Выберите клиент")
            .AppendLine("1: NothingRpcApi")
            .AppendLine("2: NothingWebApi")
            .AppendLine("e: Завершить работу")
            .AppendLine()
            .AppendLine("Работа приложения завершена")
            .ToString();
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_1_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "1", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = new StringBuilder()
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
            .AppendLine("Выберите клиент")
            .AppendLine("1: NothingRpcApi")
            .AppendLine("2: NothingWebApi")
            .AppendLine("e: Завершить работу")
            .AppendLine()
            .AppendLine("Работа приложения завершена")
            .ToString();
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DoWork_1_error_e_e_Equal()
    {
        //Arrange
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "1", "error", "e", "e");
        var loopService = GetLoopService(consoleServiceMock.Object);

        //Act
        await loopService.DoWork();
        var result = stringBuilder.ToString();

        //Assert
        var expected = new StringBuilder()
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
            .AppendLine("Отсутствует соответствие выбранной команде: error")
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
        Assert.Equal(expected, result);
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

    private static Mock<IConsoleService> GetConsoleServiceMock(StringBuilder stringBuilder, params string[] input)
    {
        var enumerator = input.GetEnumerator();
        var consoleServiceMock = new Mock<IConsoleService>();
        consoleServiceMock
            .Setup(consoleService => consoleService.ReadLine())
            .Returns(() =>
            {
                var next = enumerator.MoveNext();
                return next ? enumerator.Current?.ToString() : string.Empty;
            });
        consoleServiceMock
            .Setup(consoleService => consoleService.WriteLine(It.IsAny<string>()))
            .Callback<string>(x =>
            {
                stringBuilder.AppendLine(x);
            });
        return consoleServiceMock;
    }

    private static ILoopService GetLoopService(IConsoleService consoleService)
    {
        var loopService = new ServiceCollection()
            .AddTransient<ILoopService, LoopService>()
            .AddTransient(_ => Mock.Of<IHostApplicationLifetime>())
            .AddTransient(_ => consoleService)
            .AddTransient(_ => GetNothingRpcApiClientStrategy(consoleService))
            .AddTransient(_ => GetNothingWebApiClientStrategy(consoleService))
            .BuildServiceProvider()
            .GetRequiredService<ILoopService>();
        return loopService;
    }

    private static NothingRpcApiClientStrategy GetNothingRpcApiClientStrategy(IConsoleService consoleService)
    {
        var nothingRpcApiClientStrategyMock = new Mock<NothingRpcApiClientStrategy>(
            consoleService,
            Mock.Of<ILogger<NothingRpcApiClientStrategy>>(),
            Mock.Of<NothingRpcService.NothingRpcServiceClient>());
        nothingRpcApiClientStrategyMock
            .Setup(strategy => strategy.GetNothingModels(It.IsAny<CancellationToken>()))
            .Callback(() => consoleService.WriteLine($"{nameof(NothingRpcApiClientStrategy)}.{nameof(NothingRpcApiClientStrategy.GetNothingModels)}"));
        nothingRpcApiClientStrategyMock
            .Setup(strategy => strategy.GetNothingModel(It.IsAny<CancellationToken>()))
            .Callback(() => consoleService.WriteLine($"{nameof(NothingRpcApiClientStrategy)}.{nameof(NothingRpcApiClientStrategy.GetNothingModel)}"));
        nothingRpcApiClientStrategyMock
            .Setup(strategy => strategy.CreateNothingModel(It.IsAny<CancellationToken>()))
            .Callback(() => consoleService.WriteLine($"{nameof(NothingRpcApiClientStrategy)}.{nameof(NothingRpcApiClientStrategy.CreateNothingModel)}"));
        nothingRpcApiClientStrategyMock
            .Setup(strategy => strategy.UpdateNothingModel(It.IsAny<CancellationToken>()))
            .Callback(() => consoleService.WriteLine($"{nameof(NothingRpcApiClientStrategy)}.{nameof(NothingRpcApiClientStrategy.UpdateNothingModel)}"));
        nothingRpcApiClientStrategyMock
            .Setup(strategy => strategy.DeleteNothingModel(It.IsAny<CancellationToken>()))
            .Callback(() => consoleService.WriteLine($"{nameof(NothingRpcApiClientStrategy)}.{nameof(NothingRpcApiClientStrategy.DeleteNothingModel)}"));
        return nothingRpcApiClientStrategyMock.Object;
    }

    private static NothingWebApiClientStrategy GetNothingWebApiClientStrategy(IConsoleService consoleService)
    {
        var nothingWebApiClientStrategyMock = new Mock<NothingWebApiClientStrategy>(
            consoleService,
            Mock.Of<ILogger<NothingWebApiClientStrategy>>(),
            Mock.Of<INothingWebApiClient>());
        nothingWebApiClientStrategyMock
            .Setup(strategy => strategy.GetNothingModels(It.IsAny<CancellationToken>()))
            .Callback(() => consoleService.WriteLine($"{nameof(NothingWebApiClientStrategy)}.{nameof(NothingWebApiClientStrategy.GetNothingModels)}"));
        nothingWebApiClientStrategyMock
            .Setup(strategy => strategy.GetNothingModel(It.IsAny<CancellationToken>()))
            .Callback(() => consoleService.WriteLine($"{nameof(NothingWebApiClientStrategy)}.{nameof(NothingWebApiClientStrategy.GetNothingModel)}"));
        nothingWebApiClientStrategyMock
            .Setup(strategy => strategy.CreateNothingModel(It.IsAny<CancellationToken>()))
            .Callback(() => consoleService.WriteLine($"{nameof(NothingWebApiClientStrategy)}.{nameof(NothingWebApiClientStrategy.CreateNothingModel)}"));
        nothingWebApiClientStrategyMock
            .Setup(strategy => strategy.UpdateNothingModel(It.IsAny<CancellationToken>()))
            .Callback(() => consoleService.WriteLine($"{nameof(NothingWebApiClientStrategy)}.{nameof(NothingWebApiClientStrategy.UpdateNothingModel)}"));
        nothingWebApiClientStrategyMock
            .Setup(strategy => strategy.DeleteNothingModel(It.IsAny<CancellationToken>()))
            .Callback(() => consoleService.WriteLine($"{nameof(NothingWebApiClientStrategy)}.{nameof(NothingWebApiClientStrategy.DeleteNothingModel)}"));
        return nothingWebApiClientStrategyMock.Object;
    }
}