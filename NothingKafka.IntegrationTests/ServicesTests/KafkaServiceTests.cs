using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingKafka.Dtos;
using NothingKafka.Services;

namespace NothingKafka.IntegrationTests.ServicesTests;

public class KafkaServiceTests
{
    private const string ContainerName = "test_kafka";
    private const string Host = "localhost";
    private const string Port = "9093";
    private const string Topic = "test_topic";

    [Fact]
    public async Task IsKafkaAvailable()
    {
        try
        {
            //Arrange
            var serviceProvider = GetServiceProvider();
            var kafkaService = serviceProvider.GetRequiredService<IKafkaService>();
            var producerService = serviceProvider.GetRequiredService<IProducerService>();
            await StartApp();
            var nothingModelDto = new NothingModelDto()
            {
                Id = 1,
                Name = "Name",
            };
            await kafkaService.CreateTopic(Topic);
            await producerService.SendMessage(nothingModelDto, Topic);

            //Act
            var result = kafkaService.IsKafkaAvailable();

            //Assert
            Assert.True(result);
        }
        finally
        {
            await StopApp();
        }
    }

    private static IServiceProvider GetServiceProvider()
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<IKafkaService, KafkaService>()
            .AddTransient<IProducerService, ProducerService>()
            .AddTransient(_ => Mock.Of<ILogger<KafkaService>>())
            .AddTransient(_ => Mock.Of<IOptions<KafkaConfig>>(config
                => config.Value == new KafkaConfig()
                {
                    BootstrapServers = $"{Host}:{Port}",
                    ConsumerGroup = "nothing_services"
                }))
            .BuildServiceProvider();
        return serviceProvider;
    }

    private static async Task StartApp(int delay = 10000)
    {
        await Process.Start("docker", $"run -d --name {ContainerName} -p {Port}:{Port}  apache/kafka:latest")
            .WaitForExitAsync();
        await Task.Delay(delay);
    }

    private static async Task StopApp(int beforeDelay = 10000, int afterDelay = 2000)
    {
        await Task.Delay(beforeDelay);
        await Process.Start("docker", $"container remove -f -v {ContainerName}")
            .WaitForExitAsync();
        await Task.Delay(afterDelay);
    }
}