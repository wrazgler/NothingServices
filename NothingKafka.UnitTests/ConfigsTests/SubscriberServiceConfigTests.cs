using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;

namespace NothingKafka.UnitTests.ConfigsTests;

public class SubscriberServiceConfigTests
{
    [Fact]
    public void SubscriberServiceConfig_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();

        //Act
        var result = configuration.GetConfig<SubscriberServiceConfig>();

        //Assert
        var expected = new SubscriberServiceConfig()
        {
            IterationDelay = 10,
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void SubscriberServiceConfig_DependencyInjection_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();
        var services = new ServiceCollection()
            .Configure<SubscriberServiceConfig>(configuration)
            .BuildServiceProvider();

        //Act
        var result = services.GetRequiredService<IOptions<SubscriberServiceConfig>>().Value;

        //Assert
        var expected = new SubscriberServiceConfig()
        {
            IterationDelay = 10,
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void SubscriberServiceConfig_Empty_Throws_ConfigurationNullException()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().Build();

        //Act
        var result = new Func<SubscriberServiceConfig>(() => configuration.GetConfig<SubscriberServiceConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<SubscriberServiceConfig>>(result);
    }

    [Fact]
    public void SubscriberServiceConfig_Not_Attribute_Format_Return_Default_Value()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(5)
        {
            {"SubscriberServiceConfig", null!},
            {"SubscriberServiceConfig:IterationDelay", "10"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = configuration.GetConfig<SubscriberServiceConfig>();

        //Assert
        var expected = new SubscriberServiceConfig()
        {
            IterationDelay = 30,
        };
        Assert.Equivalent(expected, result, true);
    }

    private IConfiguration GetConfiguration()
    {
        var dictionary = new Dictionary<string, string>(1)
        {
            {"SUBSCRIBE_ITERATION_DELAY", "10"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();
        return configuration;
    }
}