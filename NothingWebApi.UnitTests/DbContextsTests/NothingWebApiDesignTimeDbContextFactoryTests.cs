using Microsoft.EntityFrameworkCore;
using NothingWebApi.DbContexts;
namespace NothingWebApi.UnitTests.DbContextsTests;

public class NothingWebApiDesignTimeDbContextFactoryTests
{
    [Fact]
    public void CreateDbContext_IsNpgsql()
    {
        //Arrange
        var designTimeDbContextFactory = new NothingWebApiDesignTimeDbContextFactory();

        //Act
        var dbContext = designTimeDbContextFactory.CreateDbContext([]);
        var result = dbContext.Database.IsNpgsql();

        //Assert
        Assert.True(result);
    }
}