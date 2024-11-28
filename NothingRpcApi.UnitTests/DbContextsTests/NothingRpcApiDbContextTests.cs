using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NothingRpcApi.DbContexts;
using NothingRpcApi.Extensions;
using NothingRpcApi.Models;

namespace NothingRpcApi.UnitTests.DbContextsTests;

public class NothingRpcApiDbContextTests
{
    [Fact]
    public async Task Create_Db_Empty()
    {
        //Arrange
        var dbContext = await GetDbContext();
        await dbContext.Database.EnsureCreatedAsync();

        //Act
        var result = await dbContext.NothingModels
            .AsNoTracking()
            .ToListAsync();

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Add_Db_Equivalent()
    {
        //Arrange
        var dbContext = await GetDbContext();
        await dbContext.Database.EnsureCreatedAsync();
        dbContext.ChangeTracker.Clear();
        var nothingModel = new NothingModel()
        {
            Name = "Test",
        };

        //Act
        await dbContext.NothingModels.AddAsync(nothingModel);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();
        var result = await dbContext.NothingModels
            .AsNoTracking()
            .SingleAsync();

        //Assert
        var expected = new NothingModel()
        {
            Id = 1,
            Name = "Test",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public async Task Remove_Db_Equivalent()
    {
        //Arrange
        var dbContext = await GetDbContext();
        await dbContext.Database.EnsureCreatedAsync();
        var nothingModel = new NothingModel()
        {
            Name = "Test",
        };
        await dbContext.NothingModels.AddAsync(nothingModel);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        //Act
        dbContext.NothingModels.Remove(nothingModel);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();
        var result = await dbContext.NothingModels
            .AsNoTracking()
            .ToListAsync();

        //Assert
        Assert.Empty(result);
    }

    private static async Task<NothingRpcApiDbContext> GetDbContext()
    {
        var dbContext = new ServiceCollection()
            .AddInMemoryDatabase(nameof(NothingRpcApiDbContextTests))
            .BuildServiceProvider()
            .GetRequiredService<NothingRpcApiDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        return dbContext;
    }
}