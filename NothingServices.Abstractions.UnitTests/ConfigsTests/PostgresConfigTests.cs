using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NothingServices.Abstractions.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;

namespace NothingServices.Abstractions.UnitTests.ConfigsTests;

public class PostgresConfigTests
{
    [Fact]
    public void PostgresConfig_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();

        //Act
        var result =  configuration.GetConfig<PostgresConfig>();

        //Assert
        var expected = new PostgresConfig()
        {
            Host = "localhost",
            Port = "5432",
            Database = "nothing_api_db",
            User = "nothing_api",
            Password = "nothing_api"
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void PostgresConfig_DependencyInjection_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();
        var services = new ServiceCollection()
            .Configure<PostgresConfig>(configuration)
            .BuildServiceProvider();

        //Act
        var result = services.GetRequiredService<IOptions<PostgresConfig>>().Value;

        //Assert
        var expected = new PostgresConfig()
        {
            Host = "localhost",
            Port = "5432",
            Database = "nothing_api_db",
            User = "nothing_api",
            Password = "nothing_api"
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void PostgresConfig_ConnectionString_Equal()
    {
        //Arrange
        var configuration = GetConfiguration();

        //Act
        var result =  configuration.GetConfig<PostgresConfig>().ConnectionString;

        //Assert
        var expected = "Host=localhost;Port=5432;Database=nothing_api_db;Username=nothing_api;Password=nothing_api";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PostgresConfig_Empty_Throws_ConfigurationNullException()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().Build();

        //Act
        var result = new Func<PostgresConfig>(() => configuration.GetConfig<PostgresConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<PostgresConfig>>(result);
    }

    [Fact]
    public void PostgresConfig_Not_Attribute_Format_Throws_ConfigurationNullException()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(5)
        {
            {"PostgresConfig", default!},
            {"PostgresConfig:Host", "localhost"},
            {"PostgresConfig:Port", "5432"},
            {"PostgresConfig:Database", "nothing_api_db"},
            {"PostgresConfig:User", "nothing_api"},
            {"PostgresConfig:Password", "nothing_api"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = new Func<PostgresConfig>(() => configuration.GetConfig<PostgresConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<PostgresConfig>>(result);
    }

    private IConfiguration GetConfiguration()
    {
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
        return configuration;
    }
}