using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq.EntityFrameworkCore;
using NothingWebApi.DbContexts;
using NothingWebApi.Dtos;
using NothingWebApi.Extensions;
using NothingWebApi.Models;
using NothingWebApi.Services;

namespace NothingWebApi.UnitTests.ServicesTests;

// ReSharper disable EntityFramework.UnsupportedServerSideFunctionCall
public class NothingServiceTests
{
    [Fact]
    public async Task GetAsync_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);

        //Act
        var result = await nothingService.GetAsync();

        //Assert
        var assert = nothingModels;
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public async Task GetAsync_Id_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);

        //Act
        var result = await nothingService.GetAsync(1);

        //Assert
        var assert = new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public async Task CreateAsync_Dto_Name_Equal()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = "Test",
        };

        //Act
        var nothingModel = await nothingService.CreateAsync(createNothingModelDto);
        var result = nothingModel.Name;

        //Assert
        var assert = "Test";
        Assert.Equal(assert, result);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(),It.IsAny<CancellationToken>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Db_Name_Equal()
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
        await nothingService.CreateAsync(createNothingModelDto);
        var result = nothingModels.Single().Name;

        //Assert
        var assert = "Test";
        Assert.Equal(assert, result);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(),It.IsAny<CancellationToken>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_EmptyName_Throw_ArgumentNullException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = string.Empty,
        };

        //Act
        var result = new Func<Task<NothingModelDto>>(() => nothingService.CreateAsync(createNothingModelDto));

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(result);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(),It.IsAny<CancellationToken>()), Times.Never);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Dto_Name_Equal()
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
        var nothingModel = await nothingService.UpdateAsync(updateNothingModelDto);
        var result = nothingModel.Name;

        //Assert
        var assert = "New Name";
        Assert.Equal(assert, result);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Db_Name_Equal()
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
        await nothingService.UpdateAsync(updateNothingModelDto);
        var result = nothingModels.Single().Name;

        //Assert
        var assert = "New Name";
        Assert.Equal(assert, result);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_EmptyName_Throw_ArgumentNullException()
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
        var result = new Func<Task<NothingModelDto>>(() => nothingService.UpdateAsync(updateNothingModelDto));

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(result);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Dto_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);

        //Act
        var result = await nothingService.DeleteAsync(1);

        //Assert
        var assert = new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        };
        Assert.Equivalent(assert, result, true);
        dbContextMock.Verify(db => db.NothingModels.Remove(It.IsAny<NothingModel>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Db_Any_False()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);

        //Act
        await nothingService.DeleteAsync(1);

        //Assert
        Assert.Empty(nothingModels);
        dbContextMock.Verify(db => db.NothingModels.Remove(It.IsAny<NothingModel>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static IList<NothingModel> GetNothingModels()
    {
        var nothingModels = new List<NothingModel>()
        {
            new NothingModel()
            {
                Id = 1,
                Name = "Test",
            }
        };
        return nothingModels;
    }

    private static INothingService GetNothingService(NothingWebApiDbContext dbContext)
    {
        var nothingService = new ServiceCollection()
            .AddScoped(_ => dbContext)
            .AddTransient<INothingService, NothingService>()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<INothingService>();
        return nothingService;
    }

    private static Mock<NothingWebApiDbContext> GetDbContextMock(IList<NothingModel> nothingModels)
    {
        var dbContextOptions = new DbContextOptions<NothingWebApiDbContext>();
        var nothingWebApiDbContextMock = new Mock<NothingWebApiDbContext>(dbContextOptions);
        nothingWebApiDbContextMock
            .SetupGet(dbContext => dbContext.NothingModels)
            .ReturnsDbSet(nothingModels);
        nothingWebApiDbContextMock
            .Setup(dbContext => dbContext.NothingModels.AddAsync(It.IsAny<NothingModel>(),  It.IsAny<CancellationToken>()))
            .Callback<NothingModel, CancellationToken>((nothingModel, _) => nothingModels.Add(nothingModel));
        nothingWebApiDbContextMock
            .Setup(dbContext => dbContext.NothingModels.Remove(It.IsAny<NothingModel>()))
            .Callback<NothingModel>(nothingModel => nothingModels.Remove(nothingModel));
        return nothingWebApiDbContextMock;
    }
}