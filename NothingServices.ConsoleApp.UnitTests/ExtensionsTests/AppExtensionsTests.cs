using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingServices.Abstractions.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.ConsoleApp.Configs;
using NothingServices.ConsoleApp.Extensions;

namespace NothingServices.ConsoleApp.UnitTests.ExtensionsTests;

public class AppExtensionsTests
{
    [Fact]
    public void AddAppClients_Services_Equivalent()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(1)
        {
            {"NOTHING_GRPC_API_URL", "https://localhost:8400/nothing-grpc-api"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = new ServiceCollection()
            .AddAppClients(configuration)
            .Select(x => x.ServiceType.ToString())
            .ToArray();

        //Assert
        var expected = new string[]
        {
            "NothingServices.ConsoleApp.Clients.NothingRpcService+NothingRpcServiceClient",
            "NothingServices.ConsoleApp.Clients.INothingWebApiClient",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void AddAppClients_Throws_ConfigurationNullException()
    {
        //Act
        var result = new Func<IServiceCollection>(()
            => new ServiceCollection().AddAppClients(Mock.Of<IConfiguration>()));

        //Assert
        Assert.Throws<ConfigurationNullException<NothingRpcApiClientConfig>>(result);
    }

    [Fact]
    public void AddAppConfigs_Contain_IConfigureOptions_NothingWebApiClientConfig()
    {
        //Act
        var result = new ServiceCollection()
            .AddAppConfigs(Mock.Of<IConfiguration>())
            .Select(x => x.ServiceType.ToString())
            .ToArray();

        //Assert
        var expected = "Microsoft.Extensions.Options.IConfigureOptions`1[NothingServices.ConsoleApp.Configs.NothingWebApiClientConfig]";
        Assert.Contains(expected, result);
    }

    [Fact]
    public void AddAppServices_Services_Equivalent()
    {
        //Act
        var result = new ServiceCollection()
            .AddAppServices()
            .Select(x => x.ServiceType.ToString())
            .ToArray();

        //Assert
        var expected = new string[]
        {
            "NothingServices.ConsoleApp.Services.IConsoleService",
            "NothingServices.ConsoleApp.Services.ILoopService",
            "NothingServices.ConsoleApp.Strategies.NothingRpcApiClientStrategy",
            "NothingServices.ConsoleApp.Strategies.NothingWebApiClientStrategy",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void AddAppHttpClient_Success()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(1)
        {
            {"NOTHING_CERTIFICATE_FILE_NAME", "localhost.crt"},
            {"NOTHING_CERTIFICATE_PASSWORD", "localhost"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = new ServiceCollection()
            .AddAppHttpClient(configuration)
            .BuildServiceProvider()
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient();

        //Assert
        Assert.IsType<HttpClient>(result);
    }

    [Fact]
    public void AddAppHttpClient_Throws_FileNotFoundException()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(1)
        {
            {"NOTHING_CERTIFICATE_FILE_NAME", "error.crt"},
            {"NOTHING_CERTIFICATE_PASSWORD", "localhost"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = new Func<IServiceCollection>(() => new ServiceCollection()
            .AddAppHttpClient(configuration));

        //Assert
        Assert.Throws<FileNotFoundException>(result);
    }

    [Fact]
    public void AddAppHttpClient_Throws_ConfigurationNullException()
    {
        //Act
        var result = new Func<IServiceCollection>(() => new ServiceCollection()
            .AddAppHttpClient(Mock.Of<IConfiguration>()));

        //Assert
        Assert.Throws<ConfigurationNullException<CertificateConfig>>(result);
    }
}