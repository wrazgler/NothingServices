using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class DeleteCommandTests
{
    public static IEnumerable<object?[]> CanExecuteData => new List<object?[]>
    {
        new object?[]
        {
            new DeleteNothingModelVM(
                Mock.Of<IButtonVM>(),
                Mock.Of<IButtonVM>(),
                Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1)),
            true,
        },
        new object?[]
        {
            new DeleteNothingModelVM(
                Mock.Of<IButtonVM>(),
                Mock.Of<IButtonVM>(),
                Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 0)),
            false,
        },
        new object?[] { null, false },
        new object?[] { new(), false },
    };

    [Theory]
    [MemberData(nameof(CanExecuteData))]
    public void CanExecute_Result_Equal(object? parameter, bool expected)
    {
        //Arrange
        var command = GetDeleteCommand(
            Mock.Of<IDialogService>(),
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
        var dialogServiceMock = new Mock<IDialogService>();
        var mainWindowManagerMock = new Mock<IMainWindowManager>();
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 1 && nothingModelVM.Name == "test");
        var parameter = new DeleteNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);
        var strategyMock = new Mock<INothingApiClientStrategy>();
        strategyMock.Setup(strategy => strategy.DeleteNothingModel(
            It.Is<DeleteNothingModelVM>(deleteNothingModelVM => deleteNothingModelVM == parameter),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(nothingModelVM);
        mainWindowManagerMock.SetupGet(mainWindowManager => mainWindowManager.Strategy)
            .Returns(strategyMock.Object);
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetDeleteCommand(
            dialogServiceMock.Object,
            mainWindowManagerMock.Object,
            notificationServiceMock.Object);

        //Act
        command.Execute(parameter);

        //Assert
        mainWindowManagerMock.VerifyGet(mainWindowManager => mainWindowManager.Strategy, Times.Once);
        strategyMock.Verify(
            strategy => strategy.DeleteNothingModel(
                It.Is<DeleteNothingModelVM>(deleteNothingModelVM => deleteNothingModelVM == parameter),
                It.IsAny<CancellationToken>()),
            Times.Once);
        mainWindowManagerMock.Verify(
            mainWindowManager => mainWindowManager.Next(It.Is<MainWindowContentType>(type
                => type == MainWindowContentType.NothingModelsListVM)),
            Times.Once);
        dialogServiceMock.Verify(dialogService => dialogService.CloseDialog(), Times.Once);
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == $"Удалено \"{nothingModelVM.Name}\""),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void Execute_Strategy_Null_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetDeleteCommand(
            Mock.Of<IDialogService>(),
            Mock.Of<IMainWindowManager>(),
            notificationServiceMock.Object);
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1);
        var parameter = new DeleteNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == "Стратегия работы приложения не задана"),
                It.IsAny<string>()),
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
        new object?[]
        {
            new DeleteNothingModelVM(
                Mock.Of<IButtonVM>(),
                Mock.Of<IButtonVM>(),
                Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 0)),
            "Поле Идентификатор модели не может быть пустым",
        },
    };

    [Theory]
    [MemberData(nameof(ExecuteErrorData))]
    public void Execute_Error_Parameter_Error_Message_Equal(object? parameter, string errorMessage)
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetDeleteCommand(
            Mock.Of<IDialogService>(),
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

    private static DeleteCommand GetDeleteCommand(
        IDialogService dialogService,
        IMainWindowManager mainWindowManager,
        INotificationService notificationService)
    {
        var deleteCommand = new ServiceCollection()
            .AddTransient<DeleteCommand>()
            .AddTransient(_ => dialogService)
            .AddTransient(_ => mainWindowManager)
            .AddTransient(_ => notificationService)
            .BuildServiceProvider()
            .GetRequiredService<DeleteCommand>();
        return deleteCommand;
    }
}