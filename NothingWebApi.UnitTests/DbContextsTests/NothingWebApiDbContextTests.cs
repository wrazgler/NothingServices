using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NothingWebApi.DbContexts;
using NothingWebApi.Extensions;
using NothingWebApi.Models;

namespace NothingWebApi.UnitTests.DbContextsTests;

public class NothingWebApiDbContextTests
{
    [Fact]
    public async Task CreateAsync_Db_Empty()
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
    public async Task AddAsync_Db_Equivalent()
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
        var assert = new NothingModel()
        {
            Id = 1,
            Name = "Test",
        };
        Assert.Equivalent(assert, result, true);
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
    
    private static async Task<NothingWebApiDbContext> GetDbContext()
    {
        var dbContext = new ServiceCollection()
            .AddInMemoryDatabase(nameof(NothingWebApiDbContextTests))
            .BuildServiceProvider()
            .GetRequiredService<NothingWebApiDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        return dbContext;
    }
}