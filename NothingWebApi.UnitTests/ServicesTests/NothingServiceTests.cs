using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NothingWebApi.DbContexts;
using NothingWebApi.Dtos;
using NothingWebApi.Extensions;
using NothingWebApi.Models;
using NothingWebApi.Services;
using NothingWebApi.UnitTests.DbContextMock;

namespace NothingWebApi.UnitTests.ServicesTests;

// ReSharper disable EntityFramework.UnsupportedServerSideFunctionCall
public class NothingServiceTests
{
    [Fact]
    public async Task Get_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);

        //Act
        var result = await nothingService.Get();

        //Assert
        var expected = nothingModels;
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Get_Throws_Exception()
    {
        //Arrange
        var dbContextOptions = new DbContextOptions<NothingWebApiDbContext>();
        var dbContextMock = new Mock<NothingWebApiDbContext>(dbContextOptions);
        dbContextMock
            .SetupGet(dbContext => dbContext.NothingModels)
            .Throws(new Exception("Fake exception"));
        var nothingService = GetNothingService(dbContextMock.Object);

        //Act
        var result = new Func<Task<NothingModelDto[]>>(() => nothingService.Get());

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task Get_Id_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);

        //Act
        var result = await nothingService.Get(1);

        //Assert
        var expected = new NothingModelDto()
        {
            Id = 1,
            Name = "Test",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Get_Not_Exist_Id_Throws_ArgumentException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);

        //Act
        var result = new Func<Task<NothingModelDto>>(() => nothingService.Get(1));

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
        var nothingModel = await nothingService.Create(createNothingModelDto);
        var result = nothingModel.Name;

        //Assert
        var expected = "Test";
        Assert.Equal(expected, result);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(),It.IsAny<CancellationToken>()), Times.Once);
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
        await nothingService.Create(createNothingModelDto);
        var result = nothingModels.Single().Name;

        //Assert
        var expected = "Test";
        Assert.Equal(expected, result);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(),It.IsAny<CancellationToken>()), Times.Once);
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
        var result = new Func<Task<NothingModelDto>>(() => nothingService.Create(createNothingModelDto));

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(result);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(),It.IsAny<CancellationToken>()), Times.Never);
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
        var nothingModel = await nothingService.Update(updateNothingModelDto);
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
        await nothingService.Update(updateNothingModelDto);
        var result = nothingModels.Single().Name;

        //Assert
        var expected = "New Name";
        Assert.Equal(expected, result);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("New Name ", "New Name")]
    [InlineData(" New Name", "New Name")]
    [InlineData(" New Name ", "New Name")]
    public async Task Update_Trim_Db_Name_Equal(string name, string expected)
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = name,
        };

        //Act
        await nothingService.Update(updateNothingModelDto);
        var result = nothingModels.Single().Name;

        //Assert
        Assert.Equal(expected, result);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateNot_Exist_Id_Throws_ArgumentException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "New Name",
        };

        //Act
        var result = new Func<Task<NothingModelDto>>(() => nothingService.Update(updateNothingModelDto));

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
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
        var result = new Func<Task<NothingModelDto>>(() => nothingService.Update(updateNothingModelDto));

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(result);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Delete_Dto_Equivalent()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var nothingService = GetNothingService(dbContextMock.Object);

        //Act
        var result = await nothingService.Delete(1);

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

        //Act
        await nothingService.Delete(1);

        //Assert
        Assert.Empty(nothingModels);
        dbContextMock.Verify(db => db.NothingModels.Remove(It.IsAny<NothingModel>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Not_Exist_Id_Throws_ArgumentException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var nothingService = GetNothingService(dbContextMock.Object);

        //Act
        var result = new Func<Task<NothingModelDto>>(() => nothingService.Delete(1));

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
    }

    private static List<NothingModel> GetNothingModels()
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
            .AddTransient(_ => Mock.Of<ILogger<NothingService>>())
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<INothingService>();
        return nothingService;
    }

    private static Mock<NothingWebApiDbContext> GetDbContextMock(List<NothingModel> nothingModels)
    {
        var dbContextBuilder = new MockDbContextBuilder<NothingWebApiDbContext>();
        dbContextBuilder.AddDbSet(x => x.NothingModels, nothingModels);
        return  dbContextBuilder.Build();
    }
}