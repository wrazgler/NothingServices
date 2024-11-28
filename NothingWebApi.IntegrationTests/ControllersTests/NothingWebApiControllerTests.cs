using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NothingWebApi.Controllers;
using NothingWebApi.Dtos;
using NothingWebApi.Extensions;
using NothingWebApi.IntegrationTests.Extensions;
using NothingWebApi.Services;

namespace NothingWebApi.IntegrationTests.ControllersTests;

public class NothingWebApiControllerTests
{
    [Fact]
    public async Task Get_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.Get(CancellationToken.None);

        //Assert
        var expected = new OkObjectResult(new List<NothingModelDto>(1)
        {
            new()
            {
                Id = 1,
                Name = "Test",
            }
        });
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Get_1_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.Get(1, CancellationToken.None);

        //Assert
        var expected = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        });
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Get_0_BadRequestObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.Get(0, CancellationToken.None);

        //Assert
        var expected = new BadRequestObjectResult("Sequence contains no elements");
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Create_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = "Test",
        };

        //Act
        var result = await controller
            .Create(createNothingModelDto, CancellationToken.None);

        //Assert
        var expected = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        });
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Create_EmptyName_BadRequestObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = string.Empty,
        };

        //Act
        var result = await controller
            .Create(createNothingModelDto, CancellationToken.None);

        //Assert
        var expected = new BadRequestObjectResult("Name cannot be null or empty. (Parameter 'Name')");
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Delete_1_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.Delete(1, CancellationToken.None);

        //Assert
        var expected = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        });
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Delete_0_BadRequestObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.Delete(0, CancellationToken.None);

        //Assert
        var expected = new BadRequestObjectResult("Sequence contains no elements");
        Assert.Equivalent(expected, result, true);
    }

    private static ServiceProvider GetServiceProvider()
    {
        var serviceProvider = new ServiceCollection()
            .AddAppAutoMapper()
            .AddLogging()
            .AddInMemoryDatabase(nameof(NothingWebApiControllerTests))
            .AddTransient<INothingService, NothingService>()
            .AddTransient<NothingWebApiController>()
            .BuildServiceProvider();
        return serviceProvider;
    }
}