using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class OpenNothingModelsListCommandTests
{
    public static IEnumerable<object?[]> CanExecuteData => new List<object?[]>
    {
        new object?[]
        {
            Mock.Of<INothingApiClientStrategy>(),
            true,
        },
        new object?[] { null, false },
        new object?[] { new(), false },
    };

    [Theory]
    [MemberData(nameof(CanExecuteData))]
    public void CanExecute_Result_Equal(object? parameter, bool expected)
    {
        //Arrange
        var command = GetOpenNothingModelsListCommand(
            Mock.Of<IMainWindowManager>(),
            Mock.Of<INotificationService>());

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.Equal(expected, result);
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

    public static IEnumerable<object?[]> ExecuteErrorData => new List<object?[]>
    {
        new object?[]
        {
            null,
            "Value cannot be null. (Parameter 'parameter')",
        },
        new object?[]
        {
            new (),
            "Некорректный тип параметра команды: Object",
        },
    };

    [Theory]
    [MemberData(nameof(ExecuteErrorData))]
    public void Execute_Error_Parameter_Error_Message_Equal(object? parameter, string errorMessage)
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenNothingModelsListCommand(
            Mock.Of<IMainWindowManager>(),
            notificationServiceMock.Object);

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == errorMessage),
                It.IsAny<string>()),
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