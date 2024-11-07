using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;
using NothingServices.ConsoleApp.Configs;

namespace NothingServices.ConsoleApp.UnitTests.ConfigsTests;

public class NothingWebApiClientConfigTests
{
        [Fact]
    public void NothingWebApiClientConfig_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();

        //Act
        var result =  configuration.GetConfig<NothingWebApiClientConfig>();

        //Assert
        var assert = new NothingWebApiClientConfig()
        {
            BaseUrl = "https://localhost:8200/nothing-web-api",
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public void NothingWebApiClientConfig_DependencyInjection_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();
        var services = new ServiceCollection()
            .Configure<NothingWebApiClientConfig>(configuration)
            .BuildServiceProvider();

        //Act
        var result = services.GetRequiredService<IOptions<NothingWebApiClientConfig>>().Value;

        //Assert
        var assert = new NothingWebApiClientConfig()
        {
            BaseUrl = "https://localhost:8200/nothing-web-api",
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public void NothingWebApiClientConfig_Empty_Throws_ConfigurationNullException()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().Build();

        //Act
        var result = new Func<NothingWebApiClientConfig>(() => configuration.GetConfig<NothingWebApiClientConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<NothingWebApiClientConfig>>(result);
    }

    [Fact]
    public void NothingWebApiClientConfig_Not_Attribute_Format_PathBase_Null()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(5)
        {
            {"NothingWebApiClientConfig", default!},
            {"NothingWebApiClientConfig:BaseUrl", "https://localhost:8200/nothing-web-api"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = new Func<NothingWebApiClientConfig>(() => configuration.GetConfig<NothingWebApiClientConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<NothingWebApiClientConfig>>(result);
    }

    private static IConfiguration GetConfiguration()
    {
        var dictionary = new Dictionary<string, string>(1)
        {
            {"NOTHING_WEB_API_URL", "https://localhost:8200/nothing-web-api"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();
        return configuration;
    }
}