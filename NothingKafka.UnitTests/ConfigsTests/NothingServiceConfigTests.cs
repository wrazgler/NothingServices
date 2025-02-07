using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;

namespace NothingKafka.UnitTests.ConfigsTests;

public class NothingServiceConfigTests
{
    [Fact]
    public void NothingServiceConfig_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();

        //Act
        var result = configuration.GetConfig<NothingServiceConfig>();

        //Assert
        var expected = new NothingServiceConfig()
        {
            CreateTopic = "create",
            DeleteTopic = "delete",
            GetModelTopic = "get_model",
            GetModelsTopic = "get_models",
            UpdateTopic = "update",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void NothingServiceConfig_DependencyInjection_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();
        var services = new ServiceCollection()
            .Configure<NothingServiceConfig>(configuration)
            .BuildServiceProvider();

        //Act
        var result = services.GetRequiredService<IOptions<NothingServiceConfig>>().Value;

        //Assert
        var expected = new NothingServiceConfig()
        {
            CreateTopic = "create",
            DeleteTopic = "delete",
            GetModelTopic = "get_model",
            GetModelsTopic = "get_models",
            UpdateTopic = "update",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void NothingServiceConfig_Empty_Throws_ConfigurationNullException()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().Build();

        //Act
        var result = new Func<NothingServiceConfig>(() => configuration.GetConfig<NothingServiceConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<NothingServiceConfig>>(result);
    }

    [Fact]
    public void KafkaConfig_Not_Attribute_Format_Throws_ConfigurationNullException()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(5)
        {
            {"NothingServiceConfig", null!},
            {"NothingServiceConfig:CreateTopic", "create"},
            {"NothingServiceConfig:DeleteTopic", "delete"},
            {"NothingServiceConfig:GetModelTopic", "get_model"},
            {"NothingServiceConfig:GetModelsTopic", "get_models"},
            {"NothingServiceConfig:UpdateTopic", "update"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = new Func<NothingServiceConfig>(() => configuration.GetConfig<NothingServiceConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<NothingServiceConfig>>(result);
    }

    private IConfiguration GetConfiguration()
    {
        var dictionary = new Dictionary<string, string>(1)
        {
            {"NOTHING_SERVICE_CREATE_TOPIC", "create"},
            {"NOTHING_SERVICE_Delete_TOPIC", "delete"},
            {"NOTHING_SERVICE_GET_MODEL_TOPIC", "get_model"},
            {"NOTHING_SERVICE_GET_MODELS_TOPIC", "get_models"},
            {"NOTHING_SERVICE_UPDATE_TOPIC", "update"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();
        return configuration;
    }
}