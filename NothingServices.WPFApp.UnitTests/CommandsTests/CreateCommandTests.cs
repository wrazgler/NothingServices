using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class CreateCommandTests
{
    [Theory]
    [InlineData("test", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void CanExecute_Test(string name, bool expected)
    {
        //Arrange
        var command = GetCreateCommand(
            Mock.Of<IDialogService>(),
            Mock.Of<IMainWindowManager>(),
            Mock.Of<INotificationService>());
        var parameter = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = name,
        };

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CanExecute_Parameter_Null_False()
    {
        //Arrange
        var command = GetCreateCommand(
            Mock.Of<IDialogService>(),
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
        var command = GetCreateCommand(
            Mock.Of<IDialogService>(),
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
        var dialogServiceMock = new Mock<IDialogService>();
        var mainWindowManagerMock = new Mock<IMainWindowManager>();
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 1 && nothingModelVM.Name == "test");
        var parameter = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = nothingModelVM.Name,
        };
        var strategyMock = new Mock<INothingApiClientStrategy>();
        strategyMock.Setup(strategy => strategy.CreateNothingModel(
            It.Is<CreateNothingModelVM>(createNothingModelVM => createNothingModelVM == parameter),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(nothingModelVM);
        mainWindowManagerMock.SetupGet(mainWindowManager => mainWindowManager.Strategy)
            .Returns(strategyMock.Object);
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetCreateCommand(
            dialogServiceMock.Object,
            mainWindowManagerMock.Object,
            notificationServiceMock.Object);

        //Act
        command.Execute(parameter);

        //Assert
        mainWindowManagerMock.VerifyGet(mainWindowManager => mainWindowManager.Strategy, Times.Once);
        strategyMock.Verify(
            strategy => strategy.CreateNothingModel(
                It.Is<CreateNothingModelVM>(createNothingModelVM => createNothingModelVM == parameter),
                It.IsAny<CancellationToken>()),
            Times.Once);
        mainWindowManagerMock.Verify(
            mainWindowManager => mainWindowManager.Next(It.Is<MainWindowContentType>(type
                => type == MainWindowContentType.NothingModelsListVM)),
            Times.Once);
        dialogServiceMock.Verify(dialogService => dialogService.CloseDialog(), Times.Once);
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == $"Создано \"{nothingModelVM.Name}\""),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Null_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetCreateCommand(
            Mock.Of<IDialogService>(),
            Mock.Of<IMainWindowManager>(),
            notificationServiceMock.Object);

        //Act
        command.Execute(null);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == "Value cannot be null. (Parameter 'parameter')"),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Object_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetCreateCommand(
            Mock.Of<IDialogService>(),
            Mock.Of<IMainWindowManager>(),
            notificationServiceMock.Object);

        //Act
        command.Execute(new object());

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == "Некорректный тип параметра команды: Object"),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Name_Null_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetCreateCommand(
            Mock.Of<IDialogService>(),
            Mock.Of<IMainWindowManager>(),
            notificationServiceMock.Object);
        var parameter = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = null!,
        };

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == "Поле Имя модели не может быть пустым"),
                It.IsAny<string>()),
            Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Execute_Parameter_Name_Trim_Empty_Notify_Error(string name)
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetCreateCommand(
            Mock.Of<IDialogService>(),
            Mock.Of<IMainWindowManager>(),
            notificationServiceMock.Object);
        var parameter = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = name,
        };

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == "Поле Имя модели не может быть пустым"),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void Execute_Strategy_Null_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetCreateCommand(
            Mock.Of<IDialogService>(),
            Mock.Of<IMainWindowManager>(),
            notificationServiceMock.Object);
        var parameter = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = "test",
        };

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == "Стратегия работы приложения не задана"),
                It.IsAny<string>()),
            Times.Once);
    }

    private static CreateCommand GetCreateCommand(
        IDialogService dialogService,
        IMainWindowManager mainWindowManager,
        INotificationService notificationService)
    {
        var createCommand = new ServiceCollection()
            .AddTransient<CreateCommand>()
            .AddTransient(_ => dialogService)
            .AddTransient(_ => mainWindowManager)
            .AddTransient(_ => notificationService)
            .BuildServiceProvider()
            .GetRequiredService<CreateCommand>();
        return createCommand;
    }
}