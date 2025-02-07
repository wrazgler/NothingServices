using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;

namespace NothingKafka.UnitTests.ConfigsTests;

public class KafkaConfigTests
{
    [Fact]
    public void KafkaConfig_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();

        //Act
        var result = configuration.GetConfig<KafkaConfig>();

        //Assert
        var expected = new KafkaConfig()
        {
            BootstrapServers = "host",
            ConsumerGroup = "nothing_services",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void KafkaConfig_DependencyInjection_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();
        var services = new ServiceCollection()
            .Configure<KafkaConfig>(configuration)
            .BuildServiceProvider();

        //Act
        var result = services.GetRequiredService<IOptions<KafkaConfig>>().Value;

        //Assert
        var expected = new KafkaConfig()
        {
            BootstrapServers = "host",
            ConsumerGroup = "nothing_services",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void KafkaConfig_Empty_Throws_ConfigurationNullException()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().Build();

        //Act
        var result = new Func<KafkaConfig>(() => configuration.GetConfig<KafkaConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<KafkaConfig>>(result);
    }

    [Fact]
    public void KafkaConfig_Not_Attribute_Format_Throws_ConfigurationNullException()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(5)
        {
            {"KafkaConfig", null!},
            {"KafkaConfig:BootstrapServers", "host"},
            {"KafkaConfig:ConsumerGroup", "nothing_services"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = new Func<KafkaConfig>(() => configuration.GetConfig<KafkaConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<KafkaConfig>>(result);
    }

    private IConfiguration GetConfiguration()
    {
        var dictionary = new Dictionary<string, string>(1)
        {
            {"KAFKA_BOOTSTRAP_SERVERS", "host"},
            {"KAFKA_CONSUMER_GROUP", "nothing_services"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();
        return configuration;
    }
}