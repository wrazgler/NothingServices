using System.Collections.ObjectModel;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Clients;
using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.Extensions;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.StrategiesTests;

public class NothingRpcApiClientStrategyTests
{
    [Fact]
    public async Task GetNothingModels_Equivalent()
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
                    () => [],
                    () => { });
                return asyncServerStreamingCall;
            });
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);

        //Act
        var nothingModelVMs = await nothingRpcApiClientStrategy.GetNothingModels();
        var result = nothingModelVMs.Count;

        //Assert
        var expected = 1;
        Assert.Equal(expected, result);
        clientMock.Verify(
            client => client.GetStream(
                It.IsAny<Empty>(),
                null,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
        nothingModelVMFactoryMock.Verify(
            factory => factory.Create(
                It.Is<NothingModelDto>(model => model == nothingModels.Single())),
            Times.Once);
    }

    [Fact]
    public async Task GetNothingModels_Throws_Exception()
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
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.GetNothingModels());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task CreateNothingModel_Equivalent()
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
        var name = "Name";
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);
        var createNothingModelVM = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = name
        };

        //Act
        await nothingRpcApiClientStrategy.CreateNothingModel(createNothingModelVM);

        //Assert
        clientMock.Verify(client => client.CreateAsync(
                It.IsAny<CreateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateNothingModel_Throws_Exception()
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
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);
        var createNothingModelVM = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = "Name"
        };

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.CreateNothingModel(createNothingModelVM));

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task UpdateNothingModel_Equivalent()
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
        var id = nothingModels.Single().Id;
        var name = "New Name";
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == id);
        var updateNothingModelVM = new UpdateNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM)
        {
            Name = name,
        };

        //Act
        await nothingRpcApiClientStrategy.UpdateNothingModel(updateNothingModelVM);

        //Assert
        clientMock.Verify(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateNothingModel_Throws_Exception()
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
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1);
        var updateNothingModelVM = new UpdateNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM)
        {
            Name = "New Name",
        };

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.UpdateNothingModel(updateNothingModelVM));

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task DeleteNothingModel_Equal()
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
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingRpcApiClientStrategy = GetNothingRpcApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == nothingModels.Single().Id);
        var deleteNothingModelVM = new DeleteNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);

        //Act
        await nothingRpcApiClientStrategy.DeleteNothingModel(deleteNothingModelVM);

        //Assert
        clientMock.Verify(client => client.DeleteAsync(
                It.IsAny<NothingModelIdDto>(),
                null,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteNothingModel_Throws_Exception()
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
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1);
        var deleteNothingModelVM = new DeleteNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);

        //Act
        var result = new Func<Task>(() => nothingRpcApiClientStrategy.DeleteNothingModel(deleteNothingModelVM));

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
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

    private static Mock<INothingModelVMFactory> GetNothingModelVMFactoryMock()
    {
        var nothingModelVMFactoryMock = new Mock<INothingModelVMFactory>();
        nothingModelVMFactoryMock
            .Setup(factory => factory.Create(It.IsAny<NothingModelDto>()))
            .Returns<NothingModelDto>(nothingModelWebDto => new NothingModelVM()
            {
                Id = nothingModelWebDto.Id,
                Name = nothingModelWebDto.Name,
                DeleteButtonVM = Mock.Of<IButtonVM>(),
                UpdateButtonVM = Mock.Of<IButtonVM>(),
            });
        return nothingModelVMFactoryMock;
    }

    private static AsyncUnaryCall<TResponse> CreateAsyncUnaryCall<TResponse>(TResponse response)
    {
        return new AsyncUnaryCall<TResponse>(
            Task.FromResult(response),
            Task.FromResult(new Metadata()),
            () => Status.DefaultSuccess,
            () => [],
            () => { });
    }

    private static NothingRpcApiClientStrategy GetNothingRpcApiClientStrategy(
        INothingModelVMFactory factory,
        NothingRpcService.NothingRpcServiceClient client)
    {
        var nothingRpcApiClientStrategy = new ServiceCollection()
            .AddTransient<NothingRpcApiClientStrategy>()
            .AddTransient(_ => client)
            .AddTransient(_ => factory)
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<NothingRpcApiClientStrategy>();
        return nothingRpcApiClientStrategy;
    }
}