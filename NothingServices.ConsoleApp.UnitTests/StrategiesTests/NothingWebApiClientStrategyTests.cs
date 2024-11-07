using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NothingServices.ConsoleApp.Clients;
using NothingServices.ConsoleApp.Dtos;
using NothingServices.ConsoleApp.Services;
using NothingServices.ConsoleApp.Strategies;

namespace NothingServices.ConsoleApp.UnitTests.StrategiesTests;

public class NothingWebApiClientStrategyTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new () { WriteIndented = true };

    [Fact]
    public async Task GetNothingModelsAsync_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(nothingModels.ToArray());
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder);
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingWebApiClientStrategy.GetNothingModelsAsync();
        var result = stringBuilder.ToString();

        //Assert
        var assert = string.Concat(
            JsonSerializer.Serialize(nothingModels, _jsonSerializerOptions),
            Environment.NewLine);
        Assert.Equal(assert, result);
    }

    [Fact]
    public async Task GetNothingModelsAsync_Throws_Exception()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.GetAsync(It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.GetNothingModelsAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task GetNothingModelAsync_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int id, CancellationToken _) => nothingModels.Single(nothingModel => nothingModel.Id == id));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingWebApiClientStrategy.GetNothingModelAsync();
        var result = stringBuilder.ToString();

        //Assert
        var assert = string.Concat(
            "Введите идентификатор",
            Environment.NewLine,
            JsonSerializer.Serialize(nothingModels.Single(), _jsonSerializerOptions),
            Environment.NewLine);
        Assert.Equal(assert, result);
    }

    [Fact]
    public async Task GetNothingModelAsync_Throws_Exception()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.GetNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task GetNothingModelAsync_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.GetNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task GetNothingModelAsync_Token_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingWebApiClientStrategy.GetNothingModelAsync(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task CreateNothingModelAsync_Equivalent()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.CreateAsync(
                It.IsAny<CreateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreateNothingModelWebDto createNothingModelWebDto, CancellationToken _) =>
            {
                var nothingModel = new NothingModelWebDto()
                {
                    Id = 1,
                    Name = createNothingModelWebDto.Name,
                };
                return nothingModel;
            });
        var name = "Name";
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, name);
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingWebApiClientStrategy.CreateNothingModelAsync();
        var result = stringBuilder.ToString();

        //Assert
        var assert = string.Concat(
            "Введите имя модели",
            Environment.NewLine,
            JsonSerializer.Serialize(new NothingModelWebDto()
            {
                Id = 1,
                Name = name,
            }, _jsonSerializerOptions),
            Environment.NewLine);
        Assert.Equal(assert, result);
    }

    [Fact]
    public async Task CreateNothingModelAsync_Throws_Exception()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.CreateAsync(
                It.IsAny<CreateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var name = "Name";
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, name);
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.CreateNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task CreateNothingModelAsync_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.CreateAsync(
                It.IsAny<CreateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.CreateNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task CreateNothingModelAsync_Token_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.CreateAsync(
                It.IsAny<CreateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingWebApiClientStrategy.CreateNothingModelAsync(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task UpdateNothingModelAsync_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((UpdateNothingModelWebDto updateNothingModelWebDto, CancellationToken _) =>
            {
                var nothingModel = new NothingModelWebDto()
                {
                    Id = updateNothingModelWebDto.Id,
                    Name = updateNothingModelWebDto.Name,
                };
                return nothingModel;
            });
        var id = nothingModels.Single().Id;
        var name = "New Name";
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, id.ToString(), name);
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingWebApiClientStrategy.UpdateNothingModelAsync();
        var result = stringBuilder.ToString();

        //Assert
        var assert = string.Concat(
            "Введите идентификатор",
            Environment.NewLine,
            "Введите имя модели",
            Environment.NewLine,
            JsonSerializer.Serialize(new NothingModelWebDto()
            {
                Id = id,
                Name = name,
            }, _jsonSerializerOptions),
            Environment.NewLine);
        Assert.Equal(assert, result);
    }

    [Fact]
    public async Task UpdateNothingModelAsync_Throws_Exception()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var name = "New Name";
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString(), name);
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.UpdateNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task UpdateNothingModelAsync_Id_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var name = "New Name";
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e", name);
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.UpdateNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task UpdateNothingModelAsync_Name_Throws_TaskCanceledException()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var id = nothingModels.Single().Id;
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, id.ToString(), "e");
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.UpdateNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task UpdateNothingModelAsync_Token_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingWebApiClientStrategy.UpdateNothingModelAsync(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task DeleteNothingModelAsync_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int id, CancellationToken _) => nothingModels.Single(nothingModel => nothingModel.Id == id));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingWebApiClientStrategy.DeleteNothingModelAsync();
        var result = stringBuilder.ToString();

        //Assert
        var assert = string.Concat(
            "Введите идентификатор",
            Environment.NewLine,
            JsonSerializer.Serialize(nothingModels.Single(), _jsonSerializerOptions),
            Environment.NewLine);
        Assert.Equal(assert, result);
    }

    [Fact]
    public async Task DeleteNothingModelAsync_Throws_Exception()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.DeleteNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task DeleteNothingModelAsync_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.DeleteNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task DeleteNothingModelAsync_Token_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingWebApiClientStrategy.DeleteNothingModelAsync(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    private static List<NothingModelWebDto> GetNothingModels()
    {
        var nothingModels = new List<NothingModelWebDto>(1)
        {
            new()
            {
                Id = 1,
                Name = "Name",
            }
        };
        return nothingModels;
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

    private static NothingWebApiClientStrategy GetNothingWebApiClientStrategy(
        IConsoleService consoleService,
        INothingWebApiClient client)
    {
        var nothingWebApiClientStrategy = new ServiceCollection()
            .AddTransient<NothingWebApiClientStrategy>()
            .AddTransient(_ => Mock.Of<ILogger<NothingWebApiClientStrategy>>())
            .AddTransient(_ => client)
            .AddTransient(_ => consoleService)
            .BuildServiceProvider()
            .GetRequiredService<NothingWebApiClientStrategy>();
        return nothingWebApiClientStrategy;
    }
}