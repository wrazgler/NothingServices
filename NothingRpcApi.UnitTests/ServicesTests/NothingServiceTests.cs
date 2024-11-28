using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq.EntityFrameworkCore;
using NothingRpcApi.DbContexts;
using NothingRpcApi.Dtos;
using NothingRpcApi.Extensions;
using NothingRpcApi.Models;
using NothingRpcApi.Services;

namespace NothingRpcApi.UnitTests.ServicesTests;

// ReSharper disable EntityFramework.UnsupportedServerSideFunctionCall
public class NothingServiceTests
{
    [Fact]
    public async Task GetStream_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);
        var responseStreamMock = new Mock<IServerStreamWriter<NothingModelDto>>();
        var nothingModelDtos = new List<NothingModelDto>(nothingModels.Count);
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
        var expected = nothingModels;
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task GetStream_ThrowsAsync_Exception()
    {
        //Arrange
        var dbContextOptions = new DbContextOptions<NothingRpcApiDbContext>();
        var dbContextMock = new Mock<NothingRpcApiDbContext>(dbContextOptions);
        dbContextMock
            .SetupGet(dbContext => dbContext.NothingModels)
            .Throws(new Exception("Fake exception"));
        var nothingService = GetNothingService(dbContextMock.Object);
        var responseStream = Mock.Of<IServerStreamWriter<NothingModelDto>>();

        //Act
        var result = new Func<Task>(()
            => nothingService.GetStream(new Empty(), responseStream, Mock.Of<ServerCallContext>()));

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task Get_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);
        var nothingModelIdDto = new NothingModelIdDto()
        {
            Id = 1,
        };

        //Act
        var result = await nothingService
            .Get(nothingModelIdDto, Mock.Of<ServerCallContext>());

        //Assert
        var expected = new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task GetAsync_Not_Exist_Id_Throws_ArgumentException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);
        var nothingModelIdDto = new NothingModelIdDto()
        {
            Id = 1,
        };

        //Act
        var result = new Func<Task<NothingModelDto>>(()
            => nothingService.Get(nothingModelIdDto, Mock.Of<ServerCallContext>()));

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
    }

    [Fact]
    public async Task Create_Dto_Name_Equal()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = "Test",
        };

        //Act
        var nothingModel = await nothingService
            .Create(createNothingModelDto, Mock.Of<ServerCallContext>());
        var result = nothingModel.Name;

        //Assert
        var expected = "Test";
        Assert.Equal(expected, result);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(), It.IsAny<CancellationToken>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_Db_Name_Equal()
    {
        //Arrange
        var nothingModels = new List<NothingModel>();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = "Test",
        };

        //Act
        await nothingService.Create(createNothingModelDto, Mock.Of<ServerCallContext>());
        var result = nothingModels.Single().Name;

        //Assert
        var expected = "Test";
        Assert.Equal(expected, result);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(), It.IsAny<CancellationToken>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_EmptyName_Throw_ArgumentNullException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = string.Empty,
        };

        //Act
        var result = new Func<Task<NothingModelDto>>(()
            => nothingService.Create(createNothingModelDto, Mock.Of<ServerCallContext>()));

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(result);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(), It.IsAny<CancellationToken>()), Times.Never);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_Dto_Name_Equal()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);
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
        var expected = "New Name";
        Assert.Equal(expected, result);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_Db_Name_Equal()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "New Name",
        };

        //Act
        await nothingService.Update(updateNothingModelDto, Mock.Of<ServerCallContext>());
        var result = nothingModels.Single().Name;

        //Assert
        var expected = "New Name";
        Assert.Equal(expected, result);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_EmptyName_Throw_ArgumentNullException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);
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
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_Not_Exist_Id_Throws_ArgumentException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "Test",
        };

        //Act
        var result = new Func<Task<NothingModelDto>>(()
            => nothingService.Update(updateNothingModelDto, Mock.Of<ServerCallContext>()));

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
    }

    [Fact]
    public async Task Delete_Dto_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);
        var nothingModelIdDto = new NothingModelIdDto()
        {
            Id = 1,
        };

        //Act
        var result = await nothingService
            .Delete(nothingModelIdDto, Mock.Of<ServerCallContext>());

        //Assert
        var expected = new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        };
        Assert.Equivalent(expected, result, true);
        dbContextMock.Verify(db => db.NothingModels.Remove(It.IsAny<NothingModel>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Db_Any_False()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);
        var nothingModelIdDto = new NothingModelIdDto()
        {
            Id = 1,
        };

        //Act
        await nothingService.Delete(nothingModelIdDto, Mock.Of<ServerCallContext>());

        //Assert
        Assert.Empty(nothingModels);
        dbContextMock.Verify(db => db.NothingModels.Remove(It.IsAny<NothingModel>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Not_Exist_Id_Throws_ArgumentException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);
        var nothingModelIdDto = new NothingModelIdDto()
        {
            Id = 1,
        };

        //Act
        var result = new Func<Task<NothingModelDto>>(()
            => nothingService.Delete(nothingModelIdDto, Mock.Of<ServerCallContext>()));

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
    }

    private static List<NothingModel> GetNothingModels()
    {
        var nothingModels = new List<NothingModel>()
        {
            new()
            {
                Id = 1,
                Name = "Test",
            }
        };
        return nothingModels;
    }

    private static INothingService GetNothingService(NothingRpcApiDbContext dbContext)
    {
        var nothingService = new ServiceCollection()
            .AddScoped(_ => dbContext)
            .AddTransient<INothingService, NothingService>()
            .AddTransient(_ => Mock.Of<ILogger<NothingService>>())
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<INothingService>();
        return nothingService;
    }

    private static Mock<NothingRpcApiDbContext> GetDbContextMock(List<NothingModel> nothingModels)
    {
        var dbContextOptions = new DbContextOptions<NothingRpcApiDbContext>();
        var nothingWebApiDbContextMock = new Mock<NothingRpcApiDbContext>(dbContextOptions);
        nothingWebApiDbContextMock
            .SetupGet(dbContext => dbContext.NothingModels)
            .ReturnsDbSet(nothingModels);
        nothingWebApiDbContextMock
            .Setup(dbContext => dbContext.NothingModels.AddAsync(It.IsAny<NothingModel>(), It.IsAny<CancellationToken>()))
            .Callback<NothingModel, CancellationToken>((nothingModel, _) => nothingModels.Add(nothingModel));
        nothingWebApiDbContextMock
            .Setup(dbContext => dbContext.NothingModels.Remove(It.IsAny<NothingModel>()))
            .Callback<NothingModel>(nothingModel => nothingModels.Remove(nothingModel));
        return nothingWebApiDbContextMock;
    }
}