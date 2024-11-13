using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Clients;
using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.Extensions;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.StrategiesTests;

public class NothingWebApiClientStrategyTests
{
    [Fact]
    public async Task GetNothingModelsAsync_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(nothingModels.ToArray());
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);

        //Act
        var result = await nothingWebApiClientStrategy.GetNothingModelsAsync();

        //Assert
        Assert.Equal(nothingModels.Single().Id, result.Single().Id);
        Assert.Equal(nothingModels.Single().Name, result.Single().Name);
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
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.GetNothingModelsAsync());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
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
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);
        var createNothingModelVM = Mock.Of<CreateNothingModelVM>(createNothingModelVM
            => createNothingModelVM.Name == name);

        //Act
        var result = await nothingWebApiClientStrategy.CreateNothingModelAsync(createNothingModelVM);

        //Assert
        Assert.Equal(1, result.Id);
        Assert.Equal(name, result.Name);
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
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);
        var createNothingModelVM = Mock.Of<CreateNothingModelVM>(createNothingModelVM
            => createNothingModelVM.Name == "Name");

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.CreateNothingModelAsync(createNothingModelVM));

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
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
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);
        var updateNothingModelVM = Mock.Of<UpdateNothingModelVM>(updateNothingModelVM
            => updateNothingModelVM.Id == id &&
               updateNothingModelVM.Name == name);

        //Act
        var result = await nothingWebApiClientStrategy.UpdateNothingModelAsync(updateNothingModelVM);

        //Assert
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
    }

    [Fact]
    public async Task UpdateNothingModelAsync_Throws_Exception()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.UpdateAsync(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);
        var updateNothingModelVM = Mock.Of<UpdateNothingModelVM>(updateNothingModelVM
            => updateNothingModelVM.Id == 1 &&
               updateNothingModelVM.Name == "New Name");

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.UpdateNothingModelAsync(updateNothingModelVM));

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
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
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);
        var nothingModelVM = Mock.Of<NothingModelVM>(nothingModelVM
            => nothingModelVM.Id == nothingModels.Single().Id);

        //Act
        var result = await nothingWebApiClientStrategy.DeleteNothingModelAsync(nothingModelVM);

        //Assert
        Assert.Equal(nothingModels.Single().Id, result.Id);
        Assert.Equal(nothingModels.Single().Name, result.Name);
    }

    [Fact]
    public async Task DeleteNothingModelAsync_Throws_Exception()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);
        var nothingModelVM = Mock.Of<NothingModelVM>(nothingModelVM => nothingModelVM.Id == 1);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.DeleteNothingModelAsync(nothingModelVM));

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
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

    private static Mock<INothingModelVMFactory> GetNothingModelVMFactoryMock()
    {
        var nothingModelVMFactoryMock = new Mock<INothingModelVMFactory>();
        nothingModelVMFactoryMock
            .Setup(factory => factory.Create(It.IsAny<NothingModelWebDto>()))
            .Returns<NothingModelWebDto>(nothingModelWebDto => new NothingModelVM()
            {
                Id = nothingModelWebDto.Id,
                Name = nothingModelWebDto.Name,
                DeleteButtonVM = Mock.Of<IButtonVM>(),
                UpdateButtonVM = Mock.Of<IButtonVM>(),
            });
        return nothingModelVMFactoryMock;
    }

    private static NothingWebApiClientStrategy GetNothingWebApiClientStrategy(
        INothingModelVMFactory factory,
        INothingWebApiClient client)
    {
        var nothingWebApiClientStrategy = new ServiceCollection()
            .AddTransient<NothingWebApiClientStrategy>()
            .AddTransient(_ => client)
            .AddTransient(_ => factory)
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<NothingWebApiClientStrategy>();
        return nothingWebApiClientStrategy;
    }
}