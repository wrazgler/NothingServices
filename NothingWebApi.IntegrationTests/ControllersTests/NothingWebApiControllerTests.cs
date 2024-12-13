using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NothingWebApi.Controllers;
using NothingWebApi.DbContexts;
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
    public async Task Get_Id_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.Get(1);

        //Assert
        var expected = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        });
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Get_Id_0_BadRequestObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();
        var id = 0;

        //Act
        var result = await controller.Get(id);

        //Assert
        var expected = new BadRequestObjectResult($"Не удалось найти модель с идентификатором {id}.");
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
            .Create(createNothingModelDto);

        //Assert
        var expected = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        });
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Create_Db_Name_Equal()
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
        await controller.Create(createNothingModelDto);
        var nothingModel = await serviceProvider
            .GetRequiredService<NothingWebApiDbContext>().NothingModels
            .AsNoTracking()
            .SingleAsync();
        var result = nothingModel.Name;

        //Assert
        var expected = "Test";
        Assert.Equal(expected, result);
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
            .Create(createNothingModelDto);

        //Assert
        var expected = new BadRequestObjectResult("Имя не может быть пустым. (Parameter 'Name')");
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Update_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "New Name",
        };

        //Act
        var result = await controller.Update(updateNothingModelDto);

        //Assert
        var expected = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = "New Name",
        });
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Update_Db_Name_Equal()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "New Name",
        };

        //Act
        await controller.Update(updateNothingModelDto);
        var nothingModel = await serviceProvider
            .GetRequiredService<NothingWebApiDbContext>().NothingModels
            .AsNoTracking()
            .SingleAsync();
        var result = nothingModel.Name;

        //Assert
        var expected = "New Name";
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task Update_Id_0_BadRequestObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 0,
            Name = "Test",
        };

        //Act
        var result = await controller.Update(updateNothingModelDto);

        //Assert
        var expected = new BadRequestObjectResult($"Не удалось найти модель с идентификатором {updateNothingModelDto.Id}.");
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Update_EmptyName_BadRequestObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = string.Empty,
        };

        //Act
        var result = await controller
            .Update(updateNothingModelDto);

        //Assert
        var expected = new BadRequestObjectResult("Имя не может быть пустым. (Parameter 'Name')");
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Delete_OkObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        var result = await controller.Delete(1);

        //Assert
        var expected = new OkObjectResult(new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        });
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Delete_Db_Any_False()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();

        //Act
        await controller.Delete(1);
        var result = await serviceProvider
            .GetRequiredService<NothingWebApiDbContext>().NothingModels
            .AsNoTracking()
            .AnyAsync();

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Delete_Id_0_BadRequestObjectResult()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBase();
        await serviceProvider.AddNothingModel();
        var controller = serviceProvider.GetRequiredService<NothingWebApiController>();
        var id = 0;

        //Act
        var result = await controller.Delete(id);

        //Assert
        var expected = new BadRequestObjectResult($"Не удалось найти модель с идентификатором {id}.");
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