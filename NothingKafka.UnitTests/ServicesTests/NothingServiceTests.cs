using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingKafka.DbContexts;
using NothingKafka.Dtos;
using NothingKafka.Extensions;
using NothingKafka.Models;
using NothingKafka.Services;
using NothingKafka.UnitTests.DbContextMock;

namespace NothingKafka.UnitTests.ServicesTests;

public class NothingServiceTests
{
    private static readonly NothingServiceConfig NothingServiceConfig = new()
    {
        CreateTopic = "CreateTopic",
        DeleteTopic = "DeleteTopic",
        GetModelTopic = "GetModelTopic",
        GetModelsTopic = "GetModelsTopic",
        UpdateTopic = "UpdateTopic",
    };

    [Fact]
    public async Task Get_Success()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var getNothingModelsDto = new GetNothingModelsDto();

        //Act
        await nothingService.Get(getNothingModelsDto);

        //Assert
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.IsAny<NothingModelDto[]>(),
                It.Is<string>(x => x == NothingServiceConfig.GetModelsTopic),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Get_Throws_Exception()
    {
        //Arrange
        var dbContextOptions = new DbContextOptions<NothingKafkaDbContext>();
        var dbContextMock = new Mock<NothingKafkaDbContext>(dbContextOptions);
        dbContextMock
            .SetupGet(dbContext => dbContext.NothingModels)
            .Throws(new Exception("Fake exception"));
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var getNothingModelsDto = new GetNothingModelsDto();

        //Act
        var result = new Func<Task>(() => nothingService.Get(getNothingModelsDto));

        //Assert
        await Assert.ThrowsAsync<Exception>(result);
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.IsAny<NothingModelDto[]>(),
                It.Is<string>(x => x == NothingServiceConfig.GetModelsTopic),
                It.IsAny<CancellationToken>()),
            Times.Never());
    }

    [Fact]
    public async Task Get_Id_Success()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var getNothingModelDto = new GetNothingModelDto()
        {
            Id = 1,
        };

        //Act
        await nothingService.Get(getNothingModelDto);

        //Assert
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x => x.Id == getNothingModelDto.Id),
                It.Is<string>(x => x == NothingServiceConfig.GetModelTopic),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Get_Not_Exist_Id_Throws_ArgumentException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var getNothingModelDto = new GetNothingModelDto()
        {
            Id = 1,
        };

        //Act
        var result = new Func<Task>(() => nothingService.Get(getNothingModelDto));

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x => x.Id == getNothingModelDto.Id),
                It.Is<string>(x => x == NothingServiceConfig.CreateTopic),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Create_Success()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = "Test",
        };

        //Act
        await nothingService.Create(createNothingModelDto);

        //Assert
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x => x.Name == createNothingModelDto.Name),
                It.Is<string>(x => x == NothingServiceConfig.CreateTopic),
                It.IsAny<CancellationToken>()),
            Times.Once);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(),It.IsAny<CancellationToken>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_Db_Name_Equal()
    {
        //Arrange
        var nothingModels = new List<NothingModel>();
        var dbContextMock = GetDbContextMock(nothingModels);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
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
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x => x.Name == createNothingModelDto.Name),
                It.Is<string>(x => x == NothingServiceConfig.CreateTopic),
                It.IsAny<CancellationToken>()),
            Times.Once);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(),It.IsAny<CancellationToken>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_EmptyName_Throw_ArgumentNullException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = string.Empty,
        };

        //Act
        var result = new Func<Task>(() => nothingService.Create(createNothingModelDto));

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(result);
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x => x.Name == createNothingModelDto.Name),
                It.Is<string>(x => x == NothingServiceConfig.CreateTopic),
                It.IsAny<CancellationToken>()),
            Times.Never);
        dbContextMock.Verify(db => db.NothingModels.AddAsync(It.IsAny<NothingModel>(),It.IsAny<CancellationToken>()), Times.Never);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_Success()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "New Name",
        };

        //Act
        await nothingService.Update(updateNothingModelDto);

        //Assert
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x
                    => x.Id == updateNothingModelDto.Id && x.Name == updateNothingModelDto.Name),
                It.Is<string>(x => x == NothingServiceConfig.UpdateTopic),
                It.IsAny<CancellationToken>()),
            Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_Db_Name_Equal()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
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
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x
                    => x.Id == updateNothingModelDto.Id && x.Name == updateNothingModelDto.Name),
                It.Is<string>(x => x == NothingServiceConfig.UpdateTopic),
                It.IsAny<CancellationToken>()),
            Times.Once);
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
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
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
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x
                    => x.Id == updateNothingModelDto.Id && x.Name == expected),
                It.Is<string>(x => x == NothingServiceConfig.UpdateTopic),
                It.IsAny<CancellationToken>()),
            Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateNot_Exist_Id_Throws_ArgumentException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "New Name",
        };

        //Act
        var result = new Func<Task>(() => nothingService.Update(updateNothingModelDto));

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x
                    => x.Id == updateNothingModelDto.Id && x.Name == updateNothingModelDto.Name),
                It.Is<string>(x => x == NothingServiceConfig.UpdateTopic),
                It.IsAny<CancellationToken>()),
            Times.Never);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_EmptyName_Throw_ArgumentNullException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = string.Empty,
        };

        //Act
        var result = new Func<Task>(() => nothingService.Update(updateNothingModelDto));

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(result);
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x
                    => x.Id == updateNothingModelDto.Id && x.Name == updateNothingModelDto.Name),
                It.Is<string>(x => x == NothingServiceConfig.UpdateTopic),
                It.IsAny<CancellationToken>()),
            Times.Never);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Delete_Success()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var deleteNothingModelDto = new DeleteNothingModelDto()
        {
            Id = 1,
        };

        //Act
        await nothingService.Delete(deleteNothingModelDto);

        //Assert
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x=> x.Id == deleteNothingModelDto.Id),
                It.Is<string>(x => x == NothingServiceConfig.DeleteTopic),
                It.IsAny<CancellationToken>()),
            Times.Once);
        dbContextMock.Verify(db => db.NothingModels.Remove(It.IsAny<NothingModel>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Db_Any_False()
    {
        //Arrange
        var nothingModels = GetNothingModels();
        var dbContextMock = GetDbContextMock(nothingModels);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var deleteNothingModelDto = new DeleteNothingModelDto()
        {
            Id = 1,
        };

        //Act
        await nothingService.Delete(deleteNothingModelDto);

        //Assert
        Assert.Empty(nothingModels);
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x=> x.Id == deleteNothingModelDto.Id),
                It.Is<string>(x => x == NothingServiceConfig.DeleteTopic),
                It.IsAny<CancellationToken>()),
            Times.Once);
        dbContextMock.Verify(db => db.NothingModels.Remove(It.IsAny<NothingModel>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Not_Exist_Id_Throws_ArgumentException()
    {
        //Arrange
        var dbContextMock = GetDbContextMock([]);
        var producerServiceMock = new Mock<IProducerService>();
        var nothingService = GetNothingService(dbContextMock.Object, producerServiceMock.Object);
        var deleteNothingModelDto = new DeleteNothingModelDto()
        {
            Id = 1,
        };

        //Act
        var result = new Func<Task>(() => nothingService.Delete(deleteNothingModelDto));

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
        producerServiceMock.Verify(
            producerService => producerService.SendMessage(
                It.Is<NothingModelDto>(x=> x.Id == deleteNothingModelDto.Id),
                It.Is<string>(x => x == NothingServiceConfig.DeleteTopic),
                It.IsAny<CancellationToken>()),
            Times.Never);
        dbContextMock.Verify(db => db.NothingModels.Remove(It.IsAny<NothingModel>()), Times.Never);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
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

    private static INothingService GetNothingService(
        NothingKafkaDbContext dbContext,
        IProducerService producerService)
    {
        var nothingService = new ServiceCollection()
            .AddScoped(_ => dbContext)
            .AddTransient<INothingService, NothingService>()
            .AddTransient(_ => Mock.Of<ILogger<NothingService>>())
            .AddTransient(_ => Mock.Of<IOptions<NothingServiceConfig>>(nothingServiceConfig
                => nothingServiceConfig.Value == NothingServiceConfig))
            .AddTransient(_ => producerService)
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<INothingService>();
        return nothingService;
    }

    private static Mock<NothingKafkaDbContext> GetDbContextMock(List<NothingModel> nothingModels)
    {
        var dbContextBuilder = new MockDbContextBuilder<NothingKafkaDbContext>();
        dbContextBuilder.AddDbSet(x => x.NothingModels, nothingModels);
        return  dbContextBuilder.Build();
    }
}