using System.Diagnostics;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingKafka.Dtos;
using NothingKafka.Serializers;
using NothingKafka.Services;

namespace NothingKafka.UnitTests.ServicesTests;

public class ConsumerServiceTests
{
    private const string ContainerName = "test_kafka";
    private const string Host = "localhost";
    private const string Port = "9092";
    private const string Topic = "test_topic";

    [Fact]
    public async Task SubscribeTopic()
    {
        try
        {
            //Arrange
            var serviceProvider = GetServiceProvider();
            var consumerService = serviceProvider.GetRequiredService<IConsumerService>();
            var kafkaService = serviceProvider.GetRequiredService<IKafkaService>();
            var producerService = serviceProvider.GetRequiredService<IProducerService>();
            var kafkaConfig = serviceProvider.GetRequiredService<IOptions<KafkaConfig>>().Value;
            await StartApp();
            await kafkaService.CreateTopic(Topic);
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = kafkaConfig.BootstrapServers,
                GroupId = kafkaConfig.ConsumerGroup,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            var nothingModelDto = new NothingModelDto()
            {
                Id = 1,
                Name = "Name",
            };
            var consumerBuilder = new ConsumerBuilder<Null, NothingModelDto>(consumerConfig);
            consumerBuilder.SetValueDeserializer(new KafkaSerializer<NothingModelDto>()!);

            //Act
            using var  consumer = consumerBuilder.Build();
            consumer.Subscribe(Topic);
            consumer.Assign(new List<TopicPartitionOffset> { new (Topic, 0, 0) });
            await producerService.SendMessage(nothingModelDto, Topic);
            var consumeResult = consumer.Consume(new CancellationTokenSource(2000).Token);
            var result = consumeResult.Message.Value;

            //Assert
            var expected = new NothingModelDto()
            {
                Id = 1,
                Name = "Name",
            };
            Assert.Equivalent(expected, result, true);
        }
        finally
        {
            await StopApp();
        }
    }

    private static IServiceProvider GetServiceProvider()
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<IConsumerService, ConsumerService>()
            .AddTransient<IKafkaService, KafkaService>()
            .AddTransient<IProducerService, ProducerService>()
            .AddTransient(_ => Mock.Of<ILogger<ConsumerService>>())
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