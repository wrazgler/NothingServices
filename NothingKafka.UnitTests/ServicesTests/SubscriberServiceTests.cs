using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingKafka.Dtos;
using NothingKafka.Services;

namespace NothingKafka.UnitTests.ServicesTests;

public class SubscriberServiceTests
{
    private static readonly KafkaConfig KafkaConfig = new()
    {
        BootstrapServers = "localhost:9092",
        ConsumerGroup = "test",
    };

    private static readonly NothingServiceConfig NothingServiceConfig = new()
    {
        CreateTopic = "CreateTopic",
        DeleteTopic = "DeleteTopic",
        GetModelTopic = "GetModelTopic",
        GetModelsTopic = "GetModelsTopic",
        UpdateTopic = "UpdateTopic",
    };

    private static readonly SubscriberServiceConfig SubscriberServiceConfig = new()
    {
        IterationDelay = 1,
    };

    [Fact]
    public async Task StartAsync_Kafka_Not_Available()
    {
        //Arrange
        var consumerServiceMock = new Mock<IConsumerService>();
        var kafkaServiceMock = new Mock<IKafkaService>();
        kafkaServiceMock
            .Setup(kafkaService => kafkaService.IsKafkaAvailable(It.IsAny<int>()))
            .Returns(false);
        var loggerMock = new Mock<ILogger<SubscriberService>>();
        var nothingServiceMock = new Mock<INothingService>();
        var subscriberService = GetSubscriberService(
            consumerServiceMock.Object,
            kafkaServiceMock.Object,
            nothingServiceMock.Object,
            loggerMock.Object);

        //Act
        await subscriberService.StartAsync(CancellationToken.None);

        //Assert
        kafkaServiceMock.Verify(kafkaService => kafkaService.IsKafkaAvailable(It.IsAny<int>()), Times.Once);
        loggerMock.Verify(
            logger => logger.Log(
                It.Is<LogLevel>(x => x == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.GetModelTopic),
                It.IsAny<Action<GetNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.GetModelsTopic),
                It.IsAny<Action<GetNothingModelsDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.CreateTopic),
                It.IsAny<Action<CreateNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.DeleteTopic),
                It.IsAny<Action<DeleteNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.UpdateTopic),
                It.IsAny<Action<UpdateNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task StartAsync_One_Iteration()
    {
        //Arrange
        var consumerServiceMock = new Mock<IConsumerService>();
        var kafkaServiceMock = new Mock<IKafkaService>();
        kafkaServiceMock
            .Setup(kafkaService => kafkaService.IsKafkaAvailable(It.IsAny<int>()))
            .Returns(true);
        var nothingServiceMock = new Mock<INothingService>();
        var subscriberService = GetSubscriberService(
            consumerServiceMock.Object,
            kafkaServiceMock.Object,
            nothingServiceMock.Object);

        //Act
        await subscriberService.StartAsync(CancellationToken.None);

        //Assert
        kafkaServiceMock.Verify(kafkaService => kafkaService.IsKafkaAvailable(It.IsAny<int>()), Times.Once);
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.GetModelTopic),
                It.IsAny<Action<GetNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.GetModelsTopic),
                It.IsAny<Action<GetNothingModelsDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.CreateTopic),
                It.IsAny<Action<CreateNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.DeleteTopic),
                It.IsAny<Action<DeleteNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.UpdateTopic),
                It.IsAny<Action<UpdateNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task StartAsync_Three_Iteration()
    {
        //Arrange
        var consumerServiceMock = new Mock<IConsumerService>();
        var kafkaServiceMock = new Mock<IKafkaService>();
        kafkaServiceMock
            .Setup(kafkaService => kafkaService.IsKafkaAvailable(It.IsAny<int>()))
            .Returns(true);
        var nothingServiceMock = new Mock<INothingService>();
        var subscriberService = GetSubscriberService(
            consumerServiceMock.Object,
            kafkaServiceMock.Object,
            nothingServiceMock.Object);

        //Act
        await subscriberService.StartAsync(CancellationToken.None);
        await Task.Delay(2500);

        //Assert
        kafkaServiceMock.Verify(kafkaService => kafkaService.IsKafkaAvailable(It.IsAny<int>()), Times.Exactly(3));
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.GetModelTopic),
                It.IsAny<Action<GetNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(3));
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.GetModelsTopic),
                It.IsAny<Action<GetNothingModelsDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(3));
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.CreateTopic),
                It.IsAny<Action<CreateNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(3));
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.DeleteTopic),
                It.IsAny<Action<DeleteNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(3));
        consumerServiceMock.Verify(
            consumerService => consumerService.SubscribeTopic(
                It.Is<string>(x => x == NothingServiceConfig.UpdateTopic),
                It.IsAny<Action<UpdateNothingModelDto, CancellationToken>>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(3));
    }

    private static SubscriberService GetSubscriberService(
        IConsumerService consumerService,
        IKafkaService kafkaService,
        INothingService nothingService,
        ILogger<SubscriberService>? logger = null)
    {
        var subscriberService = new ServiceCollection()
            .AddTransient<SubscriberService>()
            .AddTransient(_ => logger ?? Mock.Of<ILogger<SubscriberService>>())
            .AddTransient(_ => Mock.Of<IOptions<KafkaConfig>>(kafkaConfig
                => kafkaConfig.Value == KafkaConfig))
            .AddTransient(_ => Mock.Of<IOptions<NothingServiceConfig>>(nothingServiceConfig
                => nothingServiceConfig.Value == NothingServiceConfig))
            .AddTransient(_ => Mock.Of<IOptions<SubscriberServiceConfig>>(subscriberServiceConfig
                => subscriberServiceConfig.Value == SubscriberServiceConfig))
            .AddTransient(_ => consumerService)
            .AddTransient(_ => kafkaService)
            .AddTransient(_ => nothingService)
            .BuildServiceProvider()
            .GetRequiredService<SubscriberService>();
        return subscriberService;
    }
}