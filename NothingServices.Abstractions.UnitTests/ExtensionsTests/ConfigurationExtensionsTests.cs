using Microsoft.Extensions.Configuration;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;

namespace NothingServices.Abstractions.UnitTests.ExtensionsTests;

public class ConfigurationExtensionsTests
{
    [Fact]
    public void Config_Equivalent()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(5)
        {
            {"CONFIG_NAME", "Test"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = configuration.GetConfig<TestConfig>();

        //Assert
        var expected = new TestConfig()
        {
            Name = "Test"
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void PostgresConfig_Empty_Throws_ConfigurationNullException()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().Build();

        //Act
        var result = new Func<TestConfig>(() => configuration.GetConfig<TestConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<TestConfig>>(result);
    }


    [Fact]
    public void PostgresConfig_Not_Attribute_Format_Throws_ConfigurationNullException()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(5)
        {
            {"TestConfig", default!},
            {"TestConfig:Name", "Test"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = new Func<TestConfig>(() => configuration.GetConfig<TestConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<TestConfig>>(result);
    }

    private class TestConfig
    {
        [ConfigurationKeyName("CONFIG_NAME")]
        public required string Name { get; init; }
    }
}