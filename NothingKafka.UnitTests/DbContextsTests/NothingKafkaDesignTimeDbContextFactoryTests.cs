using Microsoft.EntityFrameworkCore;
using NothingKafka.DbContexts;

namespace NothingKafka.UnitTests.DbContextsTests;

public class NothingKafkaDesignTimeDbContextFactoryTests
{
    [Fact]
    public void CreateDbContext_IsNpgsql()
    {
        //Arrange
        var designTimeDbContextFactory = new NothingKafkaDesignTimeDbContextFactory();

        //Act
        var dbContext = designTimeDbContextFactory.CreateDbContext([]);
        var result = dbContext.Database.IsNpgsql();

        //Assert
        Assert.True(result);
    }
}