using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingServices.Abstractions.Exceptions;
using NothingServices.WPFApp.Configs;
using NothingServices.WPFApp.Extensions;

namespace NothingServices.WPFApp.UnitTests.ExtensionsTests;

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
        var assert = new string[]
        {
            "NothingServices.WPFApp.Clients.NothingRpcService+NothingRpcServiceClient",
            "NothingServices.WPFApp.Clients.INothingWebApiClient",
        };
        Assert.Equivalent(assert, result, true);
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
        var assert = "Microsoft.Extensions.Options.IConfigureOptions`1[NothingServices.WPFApp.Configs.NothingWebApiClientConfig]";
        Assert.Contains(assert, result);
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
        var assert = new string[]
        {
            "NothingServices.WPFApp.Services.IAppVersionProvider",
            "NothingServices.WPFApp.Services.INotificator",
            "NothingServices.WPFApp.Services.StartupService",
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public void AddAppViews_Services_Equivalent()
    {
        //Act
        var result = new ServiceCollection()
            .AddAppViews()
            .Select(x => x.ServiceType.ToString())
            .ToArray();

        //Assert
        var assert = new string[]
        {
            "NothingServices.WPFApp.Views.MainWindow",
            "NothingServices.WPFApp.ViewModels.ApiSelectionVM",
            "NothingServices.WPFApp.ViewModels.MainWindowVM",
            "NothingServices.WPFApp.iewModels.Buttons.GRpcApiButtonVM",
            "NothingServices.WPFApp.iewModels.Buttons.RestApiButtonVM",
        };
        Assert.Equivalent(assert, result, true);
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
}