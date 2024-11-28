using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NothingServices.Abstractions.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;

namespace NothingServices.Abstractions.UnitTests.ConfigsTests;

public class CertificateConfigTests
{

    [Fact]
    public void CertificateConfig_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();

        //Act
        var result =  configuration.GetConfig<CertificateConfig>();

        //Assert
        var expected = new CertificateConfig()
        {
            FileName = "localhost.crt",
            Password = "localhost",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void CertificateConfig_DependencyInjection_Equivalent()
    {
        //Arrange
        var configuration = GetConfiguration();
        var services = new ServiceCollection()
            .Configure<CertificateConfig>(configuration)
            .BuildServiceProvider();

        //Act
        var result = services.GetRequiredService<IOptions<CertificateConfig>>().Value;

        //Assert
        var expected = new CertificateConfig()
        {
            FileName = "localhost.crt",
            Password = "localhost",
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void CertificateConfig_Empty_Throws_ConfigurationNullException()
    {
        //Arrange
        var configuration = new ConfigurationBuilder().Build();

        //Act
        var result = new Func<CertificateConfig>(() => configuration.GetConfig<CertificateConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<CertificateConfig>>(result);
    }

    [Fact]
    public void CertificateConfig_Not_Attribute_Format_PathBase_Null()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(5)
        {
            {"CertificateConfig", default!},
            {"CertificateConfig:FileName", "localhost.crt"},
            {"CertificateConfig:Password", "localhost"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();

        //Act
        var result = new Func<CertificateConfig>(() => configuration.GetConfig<CertificateConfig>());

        //Assert
        Assert.Throws<ConfigurationNullException<CertificateConfig>>(result);
    }

    private static IConfiguration GetConfiguration()
    {
        var dictionary = new Dictionary<string, string>(1)
        {
            {"NOTHING_CERTIFICATE_FILE_NAME", "localhost.crt"},
            {"NOTHING_CERTIFICATE_PASSWORD", "localhost"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();
        return configuration;
    }
}