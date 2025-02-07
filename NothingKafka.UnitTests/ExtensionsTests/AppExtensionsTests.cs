using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingKafka.Extensions;

namespace NothingKafka.UnitTests.ExtensionsTests;

public class AppExtensionsTests
{
    [Fact]
    public void AddAppConfigs_Services_Count_Equal()
    {
        //Act
        var result = new ServiceCollection()
            .AddAppConfigs(Mock.Of<IConfiguration>())
            .Select(x => x.ServiceType.ToString())
            .Where(x => x.Contains("Microsoft.Extensions.Options.IConfigureOptions"))
            .ToList();

        //Assert
        var expected = new[]
        {
            "Microsoft.Extensions.Options.IConfigureOptions`1[NothingKafka.Configs.KafkaConfig]",
            "Microsoft.Extensions.Options.IConfigureOptions`1[NothingKafka.Configs.NothingServiceConfig]"
        };
        Assert.Equivalent(expected, result, true);
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
        var expected = new[]
        {
            "NothingKafka.Services.IConsumerService",
            "NothingKafka.Services.IKafkaService",
            "NothingKafka.Services.INothingService",
            "NothingKafka.Services.IProducerService",
        };
        Assert.Equivalent(expected, result, true);
    }
}