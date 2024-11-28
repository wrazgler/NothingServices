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
    public async Task GetNothingModels_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Get(It.IsAny<CancellationToken>()))
            .ReturnsAsync(nothingModels.ToArray());
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder);
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingWebApiClientStrategy.GetNothingModels();
        var result = stringBuilder.ToString();

        //Assert
        var expected = string.Concat(
            JsonSerializer.Serialize(nothingModels, _jsonSerializerOptions),
            Environment.NewLine);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetNothingModels_Throws_Exception()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Get(It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.GetNothingModels());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task GetNothingModel_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Get(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int id, CancellationToken _) => nothingModels.Single(nothingModel => nothingModel.Id == id));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingWebApiClientStrategy.GetNothingModel();
        var result = stringBuilder.ToString();

        //Assert
        var expected = string.Concat(
            "Введите идентификатор",
            Environment.NewLine,
            JsonSerializer.Serialize(nothingModels.Single(), _jsonSerializerOptions),
            Environment.NewLine);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetNothingModel_Throws_Exception()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Get(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.GetNothingModel());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task GetNothingModel_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.GetNothingModel());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task GetNothingModel_Token_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingWebApiClientStrategy.GetNothingModel(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task CreateNothingModel_Equivalent()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Create(
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
        await nothingWebApiClientStrategy.CreateNothingModel();
        var result = stringBuilder.ToString();

        //Assert
        var expected = string.Concat(
            "Введите имя модели",
            Environment.NewLine,
            JsonSerializer.Serialize(new NothingModelWebDto()
            {
                Id = 1,
                Name = name,
            }, _jsonSerializerOptions),
            Environment.NewLine);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task CreateNothingModel_Throws_Exception()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Create(
                It.IsAny<CreateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var name = "Name";
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, name);
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.CreateNothingModel());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task CreateNothingModel_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Create(
                It.IsAny<CreateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.CreateNothingModel());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task CreateNothingModel_Token_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Create(
                It.IsAny<CreateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingWebApiClientStrategy.CreateNothingModel(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task UpdateNothingModel_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Update(
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
        await nothingWebApiClientStrategy.UpdateNothingModel();
        var result = stringBuilder.ToString();

        //Assert
        var expected = string.Concat(
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
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task UpdateNothingModel_Throws_Exception()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Update(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var name = "New Name";
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString(), name);
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.UpdateNothingModel());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task UpdateNothingModel_Id_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Update(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var name = "New Name";
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e", name);
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.UpdateNothingModel());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task UpdateNothingModel_Name_Throws_TaskCanceledException()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Update(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var id = nothingModels.Single().Id;
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, id.ToString(), "e");
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.UpdateNothingModel());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task UpdateNothingModel_Token_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Update(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingWebApiClientStrategy.UpdateNothingModel(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task DeleteNothingModel_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int id, CancellationToken _) => nothingModels.Single(nothingModel => nothingModel.Id == id));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingWebApiClientStrategy.DeleteNothingModel();
        var result = stringBuilder.ToString();

        //Assert
        var expected = string.Concat(
            "Введите идентификатор",
            Environment.NewLine,
            JsonSerializer.Serialize(nothingModels.Single(), _jsonSerializerOptions),
            Environment.NewLine);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DeleteNothingModel_Throws_Exception()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.DeleteNothingModel());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task DeleteNothingModel_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.DeleteNothingModel());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task DeleteNothingModel_Token_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingWebApiClientStrategy.DeleteNothingModel(cancellationTokenSource.Token));

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