using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;
using NothingServices.ConsoleApp.Configs;

namespace NothingServices.ConsoleApp.UnitTests.ConfigsTests;

public class NothingRpcApiClientConfigTests
{
    [Fact]
    public void NothingRpcApiClientConfig_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();

        //Act
        var result =  configuration.GetConfig<NothingRpcApiClientConfig>();

        //Assert
        var assert = new NothingRpcApiClientConfig()
        {
            BaseUrl = "https://localhost:8400/nothing-grpc-api",
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public void NothingRpcApiClientConfig_DependencyInjection_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();
        var services = new ServiceCollection()
            .Configure<NothingRpcApiClientConfig>(configuration)
            .BuildServiceProvider();

        //Act
        var result = services.GetRequiredService<IOptions<NothingRpcApiClientConfig>>().Value;

        //Assert
        var assert = new NothingRpcApiClientConfig()
        {
            BaseUrl = "https://localhost:8400/nothing-grpc-api",
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public void NothingRpcApiClientConfig_Empty_Throws_ConfigurationNullException()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().Build();

        //Act
        var result = new Func<NothingRpcApiClientConfig>(() => configuration.GetConfig<NothingRpcApiClientConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<NothingRpcApiClientConfig>>(result);
    }

    [Fact]
    public void NothingRpcApiClientConfig_Not_Attribute_Format_PathBase_Null()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(5)
        {
            {"NothingRpcApiClientConfig", default!},
            {"NothingRpcApiClientConfig:BaseUrl", "https://localhost:8400/nothing-grpc-api"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = new Func<NothingRpcApiClientConfig>(() => configuration.GetConfig<NothingRpcApiClientConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<NothingRpcApiClientConfig>>(result);
    }

    private static IConfiguration GetConfiguration()
    {
        var dictionary = new Dictionary<string, string>(1)
        {
            {"NOTHING_GRPC_API_URL", "https://localhost:8400/nothing-grpc-api"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();
        return configuration;
    }
}