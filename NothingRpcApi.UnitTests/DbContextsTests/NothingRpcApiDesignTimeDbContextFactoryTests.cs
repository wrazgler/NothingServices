using Microsoft.EntityFrameworkCore;
using NothingRpcApi.DbContexts;

namespace NothingRpcApi.UnitTests.DbContextsTests;

public class NothingRpcApiDesignTimeDbContextFactoryTests
{
    [Fact]
    public void CreateDbContext_IsNpgsql()
    {
        //Arrange
        var designTimeDbContextFactory = new NothingRpcApiDesignTimeDbContextFactory();

        //Act
        var dbContext = designTimeDbContextFactory.CreateDbContext([]);
        var result = dbContext.Database.IsNpgsql();

        //Assert
        Assert.True(result);
    }
}