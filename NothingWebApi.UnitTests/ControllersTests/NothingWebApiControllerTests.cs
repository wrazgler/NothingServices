using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NothingWebApi.Controllers;
using NothingWebApi.Dtos;
using NothingWebApi.Services;

namespace NothingWebApi.UnitTests.ControllersTests;

public class NothingWebApiControllerTests
{
    [Fact]
    public async Task GetAsync_OkObjectResult()
    {
        //Arrange
        var nothingModels = new []
        {
            GetNothingModelDto()
        };
        var nothingServiceMock = new Mock<INothingService>();
        nothingServiceMock
            .Setup(nothingService => nothingService.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(nothingModels);
        var controller = GetNothingWebApiController(nothingServiceMock.Object);

        //Act
        var result = await controller.GetAsync(CancellationToken.None);

        //Assert
        var expected = new OkObjectResult(nothingModels);
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task GetAsync_Throws_BadRequestObjectResult()
    {
        //Arrange
        var nothingServiceMock = new Mock<INothingService>();
        nothingServiceMock
            .Setup(nothingService => nothingService.GetAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Fake exception"));
        var controller = GetNothingWebApiController(nothingServiceMock.Object);

        //Act
        var result = await controller.GetAsync(CancellationToken.None);

        //Assert
        var expected = new BadRequestObjectResult("Fake exception");
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task GetAsync_Id_OkObjectResult()
    {
        //Arrange
        var nothingModel = GetNothingModelDto();
        var nothingServiceMock = new Mock<INothingService>();
        nothingServiceMock
            .Setup(nothingService => nothingService.GetAsync(It.Is<int>(id => id == nothingModel.Id),It.IsAny<CancellationToken>()))
            .ReturnsAsync(nothingModel);
        var controller = GetNothingWebApiController(nothingServiceMock.Object);

        //Act
        var result = await controller.GetAsync(nothingModel.Id, CancellationToken.None);

        //Assert
        var expected = new OkObjectResult(nothingModel);
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task GetAsync_Id_BadRequestObjectResult()
    {
        //Arrange
        var nothingServiceMock = new Mock<INothingService>();
        nothingServiceMock
            .Setup(nothingService => nothingService.GetAsync(It.IsAny<int>(),It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Fake exception"));
        var controller = GetNothingWebApiController(nothingServiceMock.Object);

        //Act
        var result = await controller.GetAsync(1, CancellationToken.None);

        //Assert
        var expected = new BadRequestObjectResult("Fake exception");
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task CreateAsync_OkObjectResult()
    {
        //Arrange
        var nothingServiceMock = new Mock<INothingService>();
        nothingServiceMock
            .Setup(nothingService => nothingService.CreateAsync(
                It.IsAny<CreateNothingModelDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreateNothingModelDto x, CancellationToken _) => new NothingModelDto()
            {
                Id = 1,
                Name = x.Name,
            });
        var controller = GetNothingWebApiController(nothingServiceMock.Object);
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = "Test",
        };

        //Act
        var result = await controller
            .CreateAsync(createNothingModelDto, CancellationToken.None);

        //Assert
        var expected = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = createNothingModelDto.Name,
        });
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task CreateAsync_BadRequestObjectResult()
    {
        //Arrange
        var nothingServiceMock = new Mock<INothingService>();
        nothingServiceMock
            .Setup(nothingService => nothingService.CreateAsync(
                It.IsAny<CreateNothingModelDto>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Fake exception"));
        var controller = GetNothingWebApiController(nothingServiceMock.Object);
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = string.Empty,
        };

        //Act
        var result = await controller
            .CreateAsync(createNothingModelDto, CancellationToken.None);

        //Assert
        var expected = new BadRequestObjectResult("Fake exception");
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task UpdateAsync_OkObjectResult()
    {
        //Arrange
        var nothingServiceMock = new Mock<INothingService>();
        nothingServiceMock
            .Setup(nothingService => nothingService.UpdateAsync(
                It.IsAny<UpdateNothingModelDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((UpdateNothingModelDto x, CancellationToken _) => new NothingModelDto()
            {
                Id = x.Id,
                Name = x.Name,
            });
        var controller = GetNothingWebApiController(nothingServiceMock.Object);
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "New Name",
        };

        //Act
        var result = await controller
            .UpdateAsync(updateNothingModelDto, CancellationToken.None);

        //Assert
        var expected = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = updateNothingModelDto.Name,
        });
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task UpdateAsync_BadRequestObjectResult()
    {
        //Arrange
        var nothingServiceMock = new Mock<INothingService>();
        nothingServiceMock
            .Setup(nothingService => nothingService.UpdateAsync(
                It.IsAny<UpdateNothingModelDto>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Fake exception"));
        var controller = GetNothingWebApiController(nothingServiceMock.Object);
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = string.Empty,
        };

        //Act
        var result = await controller
            .UpdateAsync(updateNothingModelDto, CancellationToken.None);

        //Assert
        var expected = new BadRequestObjectResult("Fake exception");
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task DeleteAsync_OkObjectResult()
    {
        //Arrange
        var nothingModel = GetNothingModelDto();
        var nothingServiceMock = new Mock<INothingService>();
        nothingServiceMock
            .Setup(nothingService => nothingService.DeleteAsync(
                It.Is<int>(id => id == nothingModel.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(nothingModel);
        var controller = GetNothingWebApiController(nothingServiceMock.Object);

        //Act
        var result = await controller.DeleteAsync(1, CancellationToken.None);

        //Assert
        var expected = new OkObjectResult(nothingModel);
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task DeleteAsync_BadRequestObjectResult()
    {
        //Arrange
        var nothingServiceMock = new Mock<INothingService>();
        nothingServiceMock
            .Setup(nothingService => nothingService.DeleteAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Fake exception"));
        var controller = GetNothingWebApiController(nothingServiceMock.Object);

        //Act
        var result = await controller.DeleteAsync(0, CancellationToken.None);

        //Assert
        var expected = new BadRequestObjectResult("Fake exception");
        Assert.Equivalent(expected, result, true);
    }

    private static NothingModelDto GetNothingModelDto()
    {
        var nothingModel = new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        };
        return nothingModel;
    }

    private static NothingWebApiController GetNothingWebApiController(INothingService nothingService)
    {
        var nothingWebApiController = new ServiceCollection()
            .AddTransient<NothingWebApiController>()
            .AddTransient(_ => nothingService)
            .BuildServiceProvider()
            .GetRequiredService<NothingWebApiController>();
        return nothingWebApiController;
    }
}