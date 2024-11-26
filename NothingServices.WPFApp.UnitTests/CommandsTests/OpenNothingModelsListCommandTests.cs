using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class OpenNothingModelsListCommandTests
{
    [Fact]
    public void CanExecute_True()
    {
        //Arrange
        var command = GetOpenNothingModelsListCommand(
            Mock.Of<IMainWindowManager>(),
            Mock.Of<INotificationService>());
        var parameter = Mock.Of<INothingApiClientStrategy>();

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void CanExecute_Parameter_Null_False()
    {
        //Arrange
        var command = GetOpenNothingModelsListCommand(
            Mock.Of<IMainWindowManager>(),
            Mock.Of<INotificationService>());

        //Act
        var result = command.CanExecute(null);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CanExecute_Parameter_Object_False()
    {
        //Arrange
        var command = GetOpenNothingModelsListCommand(
            Mock.Of<IMainWindowManager>(),
            Mock.Of<INotificationService>());

        //Act
        var result = command.CanExecute(new object());

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void Execute_Success()
    {
        //Arrange
        var mainWindowManagerMock = new Mock<IMainWindowManager>();
        var command = GetOpenNothingModelsListCommand(
            mainWindowManagerMock.Object,
            Mock.Of<INotificationService>());
        var parameter = Mock.Of<INothingApiClientStrategy>();

        //Act
        command.Execute(parameter);

        //Assert
        mainWindowManagerMock.VerifySet(mainWindowManager => mainWindowManager.Strategy = parameter, Times.Once);
        mainWindowManagerMock.Verify(
            mainWindowManager => mainWindowManager.Next(It.Is<MainWindowContentType>(type
                => type == MainWindowContentType.NothingModelsListVM)),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Null_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenNothingModelsListCommand(
            Mock.Of<IMainWindowManager>(),
            notificationServiceMock.Object);

        //Act
        command.Execute(null);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(It.Is<string>(message
                => message == "Value cannot be null. (Parameter 'parameter')")),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Object_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenNothingModelsListCommand(
            Mock.Of<IMainWindowManager>(),
            notificationServiceMock.Object);

        //Act
        command.Execute(new object());

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(It.Is<string>(message
                => message == "Некорректный тип параметра команды: Object")),
            Times.Once);
    }

    private static OpenNothingModelsListCommand GetOpenNothingModelsListCommand(
        IMainWindowManager mainWindowManager,
        INotificationService notificationService)
    {
        var closeDialogCommand = new ServiceCollection()
            .AddTransient<OpenNothingModelsListCommand>()
            .AddTransient(_ => mainWindowManager)
            .AddTransient(_ => notificationService)
            .BuildServiceProvider()
            .GetRequiredService<OpenNothingModelsListCommand>();
        return closeDialogCommand;
    }
}