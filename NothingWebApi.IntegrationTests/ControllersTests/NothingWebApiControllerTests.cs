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
    public async Task GetAsync_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.GetAsync(CancellationToken.None);

        //Assert
        var assert = new OkObjectResult(new List<NothingModelDto>(1)
        {
            new()
            {
                Id = 1,
                Name = "Test",
            }
        });
        Assert.Equivalent(assert, result, true);
    }
    
    [Fact]
    public async Task GetAsync_1_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.GetAsync(1, CancellationToken.None);

        //Assert
        var assert = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        });
        Assert.Equivalent(assert, result, true);
    }
    
    [Fact]
    public async Task GetAsync_0_BadRequestObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.GetAsync(0, CancellationToken.None);

        //Assert
        var assert = new BadRequestObjectResult("Sequence contains no elements");
        Assert.Equivalent(assert, result, true);
    }
    
    [Fact]
    public async Task CreateAsync_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = "Test",
        };

        //Act
        var result = await controller
            .CreateAsync(createNothingModelDto, CancellationToken.None);

        //Assert
        var assert = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        });
        Assert.Equivalent(assert, result, true);
    }
    
    [Fact]
    public async Task CreateAsync_EmptyName_BadRequestObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = string.Empty,
        };

        //Act
        var result = await controller
            .CreateAsync(createNothingModelDto, CancellationToken.None);

        //Assert
        var assert = new BadRequestObjectResult("Name cannot be null or empty. (Parameter 'Name')");
        Assert.Equivalent(assert, result, true);
    }
    
    [Fact]
    public async Task DeleteAsync_1_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.DeleteAsync(1, CancellationToken.None);

        //Assert
        var assert = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        });
        Assert.Equivalent(assert, result, true);
    }
    
    [Fact]
    public async Task DeleteAsync_0_BadRequestObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.DeleteAsync(0, CancellationToken.None);

        //Assert
        var assert = new BadRequestObjectResult("Sequence contains no elements");
        Assert.Equivalent(assert, result, true);
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