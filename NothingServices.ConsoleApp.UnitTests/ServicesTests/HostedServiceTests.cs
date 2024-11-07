using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingServices.ConsoleApp.Services;

namespace NothingServices.ConsoleApp.UnitTests.ServicesTests;

public class HostedServiceTests
{
    [Fact]
    public async Task StartAsync_Success()
    {
        //Arrange
        var loopServiceMock = new Mock<ILoopService>();
        var hostedService = GetHostedService(
            loopServiceMock.Object,
            Mock.Of<IHostApplicationLifetime>());

        //Act
        await hostedService.StartAsync(CancellationToken.None);

        //Assert
        loopServiceMock.Verify(
            consoleService => consoleService.DoWorkAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task StopAsync_Success()
    {
        //Arrange
        var hostApplicationLifetimeMock = new Mock<IHostApplicationLifetime>();
        var hostedService = GetHostedService(
            Mock.Of<ILoopService>(),
            hostApplicationLifetimeMock.Object);

        //Act
        await hostedService.StopAsync(CancellationToken.None);

        //Assert
        hostApplicationLifetimeMock.Verify(
            hostApplicationLifetime => hostApplicationLifetime.StopApplication(),
            Times.Once);
    }

    private static IHostedService GetHostedService(
        ILoopService loopService,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        var hostedService = new ServiceCollection()
            .AddTransient<IHostedService, HostedService>()
            .AddTransient(_ => loopService)
            .AddTransient(_ => hostApplicationLifetime)
            .BuildServiceProvider()
            .GetRequiredService<IHostedService>();
        return hostedService;
    }
}