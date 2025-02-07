using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingKafka.Services;

namespace NothingKafka.UnitTests.ServicesTests;

public class ConsumerServiceTests
{
    [Fact]
    public async Task SubscribeTopic()
    {
        //Arrange
        var consumerService = GetConsumerService();
        var result = false;

        //Act
        await consumerService.SubscribeTopic<object>("test", (message, token) => result = true, CancellationToken.None);

        //Assert
        Assert.True(result);
    }

    private static IConsumerService GetConsumerService()
    {
        var consumerService = new ServiceCollection()
            .AddTransient<IConsumerService, ConsumerService>()
            .AddTransient(_ => Mock.Of<ILogger<ConsumerService>>())
            .AddTransient(_ => Mock.Of<IKafkaService>())
            .AddTransient(_ => Mock.Of<IOptions<KafkaConfig>>(config
                => config.Value == new KafkaConfig()
                {
                    BootstrapServers = "host",
                    ConsumerGroup = "nothing_services"
                }))
            .BuildServiceProvider()
            .GetRequiredService<IConsumerService>();
        return consumerService;
    }
}