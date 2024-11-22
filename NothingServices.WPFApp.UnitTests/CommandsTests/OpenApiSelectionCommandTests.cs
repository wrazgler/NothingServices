using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class OpenApiSelectionCommandTests
{
    [Fact]
    public void CanExecute_True()
    {
        //Arrange
        var command = GetOpenApiSelectionCommand(
            Mock.Of<IMainWindowManager>(),
            Mock.Of<INotificationService>());

        //Act
        var result = command.CanExecute(null);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Execute_Success()
    {
        //Arrange
        var mainWindowManagerMock = new Mock<IMainWindowManager>();
        var command = GetOpenApiSelectionCommand(
            mainWindowManagerMock.Object,
            Mock.Of<INotificationService>());

        //Act
        command.Execute(null);

        //Assert
        mainWindowManagerMock.VerifySet(mainWindowManager => mainWindowManager.Strategy = null, Times.Once);
        mainWindowManagerMock.Verify(
            mainWindowManager => mainWindowManager.Next(It.Is<MainWindowContentType>(type
                => type == MainWindowContentType.ApiSelectionVM)),
            Times.Once);
    }

    [Fact]
    public void Execute_Notify_Exception()
    {
        //Arrange
        var mainWindowManagerMock = new Mock<IMainWindowManager>();
        mainWindowManagerMock.Setup(mainWindowManager => mainWindowManager.Next(It.IsAny<MainWindowContentType>()))
            .Throws(new Exception("Fake exception"));
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenApiSelectionCommand(
            mainWindowManagerMock.Object,
            notificationServiceMock.Object);

        //Act
        command.Execute(null);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(It.Is<string>(message
                => message == "Fake exception")),
            Times.Once);
    }

    private static OpenApiSelectionCommand GetOpenApiSelectionCommand(
        IMainWindowManager mainWindowManager,
        INotificationService notificationService)
    {
        var closeDialogCommand = new ServiceCollection()
            .AddTransient<OpenApiSelectionCommand>()
            .AddTransient(_ => mainWindowManager)
            .AddTransient(_ => notificationService)
            .BuildServiceProvider()
            .GetRequiredService<OpenApiSelectionCommand>();
        return closeDialogCommand;
    }
}