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
    public async Task GetNothingModels_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Get(It.IsAny<CancellationToken>()))
            .ReturnsAsync(nothingModels.ToArray());
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);

        //Act
        var nothingModelVMs = await nothingWebApiClientStrategy.GetNothingModels();
        var result = nothingModelVMs.Count;

        //Assert
        var expected = 1;
        Assert.Equal(expected, result);
        clientMock.Verify(client => client.Get(It.IsAny<CancellationToken>()),Times.Once);
        nothingModelVMFactoryMock.Verify(
            factory => factory.Create(
                It.Is<NothingModelWebDto>(model => model == nothingModels.Single())),
            Times.Once);
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
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.GetNothingModels());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
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
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);
        var createNothingModelVM = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = name
        };

        //Act
        await nothingWebApiClientStrategy.CreateNothingModel(createNothingModelVM);

        //Assert
        clientMock.Verify(client => client.Create(
                It.IsAny<CreateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
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
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);
        var createNothingModelVM = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = "Name"
        };

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.CreateNothingModel(createNothingModelVM));

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
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
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
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
        await nothingWebApiClientStrategy.UpdateNothingModel(updateNothingModelVM);

        //Assert
        clientMock.Verify(client => client.Update(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateNothingModel_Throws_Exception()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Update(
                It.IsAny<UpdateNothingModelWebDto>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
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
        var result = new Func<Task>(() => nothingWebApiClientStrategy.UpdateNothingModel(updateNothingModelVM));

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
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
        var nothingModelVMFactoryMock = GetNothingModelVMFactoryMock();
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            nothingModelVMFactoryMock.Object,
            clientMock.Object);
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == nothingModels.Single().Id);
        var deleteNothingModelVM = new DeleteNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);

        //Act
        await nothingWebApiClientStrategy.DeleteNothingModel(deleteNothingModelVM);

        //Assert
        clientMock.Verify(
            client => client.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteNothingModel_Throws_Exception()
    {
        //Arrange
        var clientMock = new Mock<INothingWebApiClient>();
        clientMock
            .Setup(client => client.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Fake exception"));
        var nothingWebApiClientStrategy = GetNothingWebApiClientStrategy(
            Mock.Of<INothingModelVMFactory>(),
            clientMock.Object);
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1);
        var deleteNothingModelVM = new DeleteNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);

        //Act
        var result = new Func<Task>(() => nothingWebApiClientStrategy.DeleteNothingModel(deleteNothingModelVM));

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