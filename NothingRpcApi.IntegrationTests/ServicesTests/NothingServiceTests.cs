using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NothingRpcApi.DbContexts;
using NothingRpcApi.Dtos;
using NothingRpcApi.Extensions;
using NothingRpcApi.Services;
using NothingWebApi.IntegrationTests.Extensions;

namespace NothingRpcApi.IntegrationTests.ServicesTests;

public class NothingServiceTests
{
    [Fact]
    public async Task GetStream_Equivalent()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var nothingService = serviceProvider.GetRequiredService<INothingService>();
        var responseStreamMock = new Mock<IServerStreamWriter<NothingModelDto>>();
        var nothingModelDtos = new List<NothingModelDto>(1);
        responseStreamMock
            .Setup(x => x.WriteAsync(It.IsAny<NothingModelDto>(), It.IsAny<CancellationToken>()))
            .Returns<NothingModelDto, CancellationToken>((x , c) => Task.Run(() => nothingModelDtos.Add(x), c));

        //Act
        await nothingService.GetStream(
            new Empty(),
            responseStreamMock.Object,
            Mock.Of<ServerCallContext>());
        var result = nothingModelDtos;

        //Assert
        var assert = new NothingModelDto[]
        {
            new()
            {
                Id = 1,
            Name = "Test",
            }
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public async Task Get_Equivalent()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var nothingService = serviceProvider.GetRequiredService<INothingService>();
        var nothingModelIdDto = new NothingModelIdDto()
        {
            Id = 1,
        };

        //Act
        var result = await nothingService
            .Get(nothingModelIdDto, Mock.Of<ServerCallContext>());

        //Assert
        var assert = new NothingModelDto()
        {
            Id = nothingModelIdDto.Id,
            Name = "Test",
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public async Task Create_Dto_Name_Equal()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var nothingService = serviceProvider.GetRequiredService<INothingService>();
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = "Test",
        };

        //Act
        var nothingModel = await nothingService
            .Create(createNothingModelDto, Mock.Of<ServerCallContext>());
        var result = nothingModel.Name;

        //Assert
        var assert = "Test";
        Assert.Equal(assert, result);
    }

    [Fact]
    public async Task Create_Db_Name_Equal()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        var nothingService = serviceProvider.GetRequiredService<INothingService>();
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = "Test",
        };

        //Act
        await nothingService.Create(createNothingModelDto, Mock.Of<ServerCallContext>());
        var nothingModel = await serviceProvider
            .GetRequiredService<NothingRpcApiDbContext>().NothingModels
            .AsNoTracking()
            .SingleAsync();
        var result = nothingModel.Name;

        //Assert
    var assert = "Test";
        Assert.Equal(assert, result);
    }

    [Fact]
    public async Task Create_EmptyName_Throw_ArgumentNullException()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        var nothingService = serviceProvider.GetRequiredService<INothingService>();
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = string.Empty,
        };

        //Act
        var result = new Func<Task<NothingModelDto>>(()
            => nothingService.Create(createNothingModelDto, Mock.Of<ServerCallContext>()));

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(result);
    }

    [Fact]
    public async Task Update_Dto_Name_Equal()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var nothingService = serviceProvider.GetRequiredService<INothingService>();
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "New Name",
        };

        //Act
        var nothingModel = await nothingService
            .Update(updateNothingModelDto, Mock.Of<ServerCallContext>());
        var result = nothingModel.Name;

        //Assert
        var assert = "New Name";
        Assert.Equal(assert, result);
    }

    [Fact]
    public async Task Update_Db_Name_Equal()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var nothingService = serviceProvider.GetRequiredService<INothingService>();
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "New Name",
        };

        //Act
        await nothingService.Update(updateNothingModelDto, Mock.Of<ServerCallContext>());
        var nothingModel = await serviceProvider
            .GetRequiredService<NothingRpcApiDbContext>().NothingModels
            .AsNoTracking()
            .SingleAsync();
        var result = nothingModel.Name;

        //Assert
        var assert = "New Name";
        Assert.Equal(assert, result);
    }

    [Fact]
    public async Task Update_EmptyName_Throw_ArgumentNullException()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var nothingService = serviceProvider.GetRequiredService<INothingService>();
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = string.Empty,
        };

        //Act
        var result = new Func<Task<NothingModelDto>>(()
            => nothingService.Update(updateNothingModelDto, Mock.Of<ServerCallContext>()));

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(result);
    }

    [Fact]
    public async Task Delete_Dto_Equivalent()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var nothingService = serviceProvider.GetRequiredService<INothingService>();
        var nothingModelIdDto = new NothingModelIdDto()
        {
            Id = 1,
        };

        //Act
        var result = await nothingService
            .Delete(nothingModelIdDto, Mock.Of<ServerCallContext>());

        //Assert
        var assert = new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public async Task Delete_Db_Any_False()
    {
        //Arrange
        var serviceProvider = GetServiceProvider();
        await serviceProvider.CreateNewDataBaseAsync();
        await serviceProvider.AddNothingModelAsync();
        var nothingService = serviceProvider.GetRequiredService<INothingService>();
        var nothingModelIdDto = new NothingModelIdDto()
        {
            Id = 1,
        };

        //Act
        await nothingService.Delete(nothingModelIdDto, Mock.Of<ServerCallContext>());
        var result = await serviceProvider
            .GetRequiredService<NothingRpcApiDbContext>().NothingModels
            .AsNoTracking()
            .AnyAsync();

        //Assert
        Assert.False(result);
    }

    private static ServiceProvider GetServiceProvider()
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<INothingService, NothingService>()
            .AddInMemoryDatabase(nameof(NothingServiceTests))
            .AddAppAutoMapper()
            .AddLogging()
            .BuildServiceProvider();
        return serviceProvider;
    }
}