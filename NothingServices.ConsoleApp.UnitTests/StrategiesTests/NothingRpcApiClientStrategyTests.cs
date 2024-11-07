using System.Text;
using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NothingServices.ConsoleApp.Clients;
using NothingServices.ConsoleApp.Dtos;
using NothingServices.ConsoleApp.Services;
using NothingServices.ConsoleApp.Strategies;

namespace NothingServices.ConsoleApp.UnitTests.StrategiesTests;

public class NothingRpcApiClientStrategyTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new () { WriteIndented = true };

    [Fact]
    public async Task GetNothingModelsAsync_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.GetStream(
                It.IsAny<Empty>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Returns((Empty _, Metadata _, DateTime _, CancellationToken _) =>
            {
                var enumerator = nothingModels.GetEnumerator();
                var streamMock = new Mock<IAsyncStreamReader<NothingModelDto>>();
                streamMock.Setup(stream => stream.MoveNext(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => enumerator.MoveNext());
                streamMock.SetupGet(stream => stream.Current)
                    .Returns(() => enumerator.Current);
                var asyncServerStreamingCall = new AsyncServerStreamingCall<NothingModelDto>(
                    streamMock.Object,
                    Task.FromResult(new Metadata()),
                    () => Status.DefaultSuccess,
                    () => new Metadata(),
                    () => { });
                return asyncServerStreamingCall;
            });
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder);
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingRpcApiClientStrategy.GetNothingModelsAsync();
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
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.GetStream(
                It.IsAny<Empty>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.GetNothingModelsAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task GetNothingModelAsync_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.GetAsync(
                It.IsAny<NothingModelIdDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Returns((NothingModelIdDto request, Metadata _, DateTime _, CancellationToken _) =>
            {
                var nothingModel = nothingModels.Single(nothingModel => nothingModel.Id == request.Id);
                var asyncUnaryCall = CreateAsyncUnaryCall(nothingModel);
                return asyncUnaryCall;
            });
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingRpcApiClientStrategy.GetNothingModelAsync();
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
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.GetAsync(
                It.IsAny<NothingModelIdDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "1");
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.GetNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task GetNothingModelAsync_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.GetAsync(
                It.IsAny<NothingModelIdDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.GetNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task GetNothingModelAsync_Token_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.GetAsync(
                It.IsAny<NothingModelIdDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingRpcApiClientStrategy.GetNothingModelAsync(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task CreateNothingModelAsync_Equivalent()
    {
        //Arrange
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.CreateAsync(
                It.IsAny<CreateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Returns((CreateNothingModelDto request, Metadata _, DateTime _, CancellationToken _) =>
            {
                var nothingModel = new NothingModelDto()
                {
                    Id =  1,
                    Name = request.Name,
                };
                var asyncUnaryCall = CreateAsyncUnaryCall(nothingModel);
                return asyncUnaryCall;
            });
        var stringBuilder = new StringBuilder();
        var name = "Name";
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, name);
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingRpcApiClientStrategy.CreateNothingModelAsync();
        var result = stringBuilder.ToString();

        //Assert
        var assert = string.Concat(
            "Введите имя модели",
            Environment.NewLine,
            JsonSerializer.Serialize(new NothingModelDto()
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
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.CreateAsync(
                It.IsAny<CreateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var name = "Name";
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, name);
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.CreateNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task CreateNothingModelAsync_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.CreateAsync(
                It.IsAny<CreateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.CreateNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task CreateNothingModelAsync_Token_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.CreateAsync(
                It.IsAny<CreateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingRpcApiClientStrategy.CreateNothingModelAsync(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task UpdateNothingModelAsync_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Returns((UpdateNothingModelDto request, Metadata _, DateTime _, CancellationToken _) =>
            {
                var nothingModel = new NothingModelDto()
                {
                    Id = request.Id,
                    Name = request.Name,
                };
                var asyncUnaryCall = CreateAsyncUnaryCall(nothingModel);
                return asyncUnaryCall;
            });
        var stringBuilder = new StringBuilder();
        var id = nothingModels.Single().Id;
        var name = "New Name";
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, id.ToString(), name);
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingRpcApiClientStrategy.UpdateNothingModelAsync();
        var result = stringBuilder.ToString();

        //Assert
        var assert = string.Concat(
            "Введите идентификатор",
            Environment.NewLine,
            "Введите имя модели",
            Environment.NewLine,
            JsonSerializer.Serialize(new NothingModelDto()
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
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var name = "New Name";
        var consoleServiceMock = GetConsoleServiceMock(
            stringBuilder,
            nothingModels.Single().Id.ToString(),
            name);
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.UpdateNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task UpdateNothingModelAsync_Id_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var name = "New Name";
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e", name);
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.UpdateNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task UpdateNothingModelAsync_Name_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.UpdateNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task UpdateNothingModelAsync_Token_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingRpcApiClientStrategy.UpdateNothingModelAsync(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task DeleteNothingModelAsync_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.DeleteAsync(
                It.IsAny<NothingModelIdDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Returns((NothingModelIdDto request, Metadata _, DateTime _, CancellationToken _) =>
            {
                var nothingModel = nothingModels.Single(nothingModel => nothingModel.Id == request.Id);
                var asyncUnaryCall = CreateAsyncUnaryCall(nothingModel);
                return asyncUnaryCall;
            });
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        await nothingRpcApiClientStrategy.DeleteNothingModelAsync();
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
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.DeleteAsync(
                It.IsAny<NothingModelIdDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, nothingModels.Single().Id.ToString());
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.DeleteNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task DeleteNothingModelAsync_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.DeleteAsync(
                It.IsAny<NothingModelIdDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var stringBuilder = new StringBuilder();
        var consoleServiceMock = GetConsoleServiceMock(stringBuilder, "e");
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(consoleServiceMock.Object, clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.DeleteNothingModelAsync());

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    [Fact]
    public async Task DeleteNothingModelAsync_Token_Throws_TaskCanceledException()
    {
        //Arrange
        var clientMock = new Mock<NothingRpcService.NothingRpcServiceClient>();
        clientMock
            .Setup(client => client.DeleteAsync(
                It.IsAny<NothingModelIdDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(
            Mock.Of<IConsoleService>(),
            clientMock.Object);
        var cancellationTokenSource = new CancellationTokenSource(100);

        //Act
        var result = new Func<Task>(()
            => nothingRpcApiClientStrategy.DeleteNothingModelAsync(cancellationTokenSource.Token));

        //Assert
        await Assert.ThrowsAsync<TaskCanceledException>(result);
    }

    private static List<NothingModelDto> GetNothingModels()
    {
        var nothingModels = new List<NothingModelDto>(1)
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

    private static AsyncUnaryCall<TResponse> CreateAsyncUnaryCall<TResponse>(TResponse response)
    {
        return new AsyncUnaryCall<TResponse>(
            Task.FromResult(response),
            Task.FromResult(new Metadata()),
            () => Status.DefaultSuccess,
            () => new Metadata(),
            () => { });
    }

    private static NothingRpcApiClientStrategy GetNothingRpcApiClientStrategy(
        IConsoleService consoleService,
        NothingRpcService.NothingRpcServiceClient client)
    {
        var nothingRpcApiClientStrategy = new ServiceCollection()
            .AddTransient<NothingRpcApiClientStrategy>()
            .AddTransient(_ => Mock.Of<ILogger<NothingRpcApiClientStrategy>>())
            .AddTransient(_ => client)
            .AddTransient(_ => consoleService)
            .BuildServiceProvider()
            .GetRequiredService<NothingRpcApiClientStrategy>();
        return nothingRpcApiClientStrategy;
    }
}