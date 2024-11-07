using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingWebApi.DbContexts;
using NothingWebApi.Extensions;

namespace NothingWebApi.UnitTests.ExtensionsTests;

public class DbContextExtensionsTests
{
    [Fact]
    public void AddInMemoryDatabase_Context_Success()
    {
        //Arrange
        var serviceCollection = new ServiceCollection();
        var dictionary = new Dictionary<string, string>(5)
        {
            {"POSTGRES_HOST", "localhost"},
            {"POSTGRES_PORT", "5432"},
            {"POSTGRES_DB", "nothing_api_db"},
            {"POSTGRES_USER", "nothing_api"},
            {"POSTGRES_PASSWORD", "nothing_api"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();
        var context = new HostBuilderContext(new Dictionary<object, object>())
        {
            Configuration = configuration
        };

        //Act
        var result = serviceCollection
            .AddInMemoryDatabase(context)
            .BuildServiceProvider()
            .GetRequiredService<NothingWebApiDbContext>();

        //Assert
        Assert.IsType<NothingWebApiDbContext>(result);
    }

    [Fact]
    public void AddInMemoryDatabase_Name_Success()
    {
        //Arrange
        var serviceCollection = new ServiceCollection();

        //Act
        var result = serviceCollection
            .AddInMemoryDatabase(nameof(DbContextExtensionsTests))
            .BuildServiceProvider()
            .GetRequiredService<NothingWebApiDbContext>();

        //Assert
        Assert.IsType<NothingWebApiDbContext>(result);
    }
}