using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class OpenUpdateNothingModelCommandTests
{
    [Fact]
    public void CanExecute_True()
    {
        //Arrange
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IUpdateNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 1 && nothingModelVM.Name == "test");

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void CanExecute_Id_0_False()
    {
        //Arrange
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IUpdateNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 0 && nothingModelVM.Name == "test");

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CanExecute_Name_Empty_False()
    {
        //Arrange
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IUpdateNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 1 && nothingModelVM.Name == string.Empty);

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CanExecute_Name_Trim_Empty_False()
    {
        //Arrange
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IUpdateNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 1 && nothingModelVM.Name == "    ");

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CanExecute_Parameter_Null_False()
    {
        //Arrange
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IUpdateNothingModelView>());

        //Act
        var result = command.CanExecute(null);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CanExecute_Parameter_Object_False()
    {
        //Arrange
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IUpdateNothingModelView>());

        //Act
        var result = command.CanExecute(new object());

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void Execute_Success()
    {
        //Arrange
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 1 && nothingModelVM.Name == "test");
        var updateNothingModelVM = new UpdateNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);
        var updateNothingModelView = Mock.Of<IUpdateNothingModelView>();
        var updateNothingModelVMFactoryMock = new Mock<IUpdateNothingModelVMFactory>();
        updateNothingModelVMFactoryMock
            .Setup(updateNothingModelVMFactory => updateNothingModelVMFactory
                .Create(It.Is<INothingModelVM>(model => model == nothingModelVM)))
            .Returns(updateNothingModelVM);
        var dialogServiceMock = new Mock<IDialogService>();
        dialogServiceMock.Setup(dialogService => dialogService.OpenDialog(
                It.Is<UpdateNothingModelVM>(dialogContentVM => dialogContentVM == updateNothingModelVM),
                It.Is<IUpdateNothingModelView>(dialogContentView => dialogContentView == updateNothingModelView)));
        var command = GetOpenUpdateNothingModelCommand(
            updateNothingModelVMFactoryMock.Object,
            dialogServiceMock.Object,
            Mock.Of<INotificationService>(),
            updateNothingModelView);

        //Act
        command.Execute(null);

        //Assert
        updateNothingModelVMFactoryMock.Verify(
            updateNothingModelVMFactory => updateNothingModelVMFactory
                .Create(It.Is<INothingModelVM>(model => model == nothingModelVM)),
            Times.Once);
        dialogServiceMock.Verify(
            dialogService => dialogService.OpenDialog(
                It.Is<UpdateNothingModelVM>(dialogContentVM => dialogContentVM == updateNothingModelVM),
                It.Is<IUpdateNothingModelView>(dialogContentView => dialogContentView == updateNothingModelView)),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Object_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            notificationServiceMock.Object,
            Mock.Of<IUpdateNothingModelView>());

        //Act
        command.Execute(new object());

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(It.Is<string>(message
                => message == "Некорректный тип параметра команды: Object")),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Id_0_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            notificationServiceMock.Object,
            Mock.Of<IUpdateNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 0 && nothingModelVM.Name == "test");

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(It.Is<string>(message
                => message == "Поле Идентификатор модели не может быть пустым")),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Name_Null_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            notificationServiceMock.Object,
            Mock.Of<IUpdateNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 1 && nothingModelVM.Name == null!);

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(It.Is<string>(message
                => message == "Поле Имя модели не может быть пустым")),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Name_Empty_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            notificationServiceMock.Object,
            Mock.Of<IUpdateNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 1 && nothingModelVM.Name == string.Empty);

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(It.Is<string>(message
                => message == "Поле Имя модели не может быть пустым")),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Name_Trim_Empty_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            notificationServiceMock.Object,
            Mock.Of<IUpdateNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 1 && nothingModelVM.Name == "   ");

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(It.Is<string>(message
                => message == "Поле Имя модели не может быть пустым")),
            Times.Once);
    }

    private static OpenUpdateNothingModelCommand GetOpenUpdateNothingModelCommand(
        IUpdateNothingModelVMFactory updateNothingModelVMFactory,
        IDialogService dialogService,
        INotificationService notificationService,
        IUpdateNothingModelView updateNothingModelView)
    {
        var openUpdateNothingModelCommand = new ServiceCollection()
            .AddTransient<OpenUpdateNothingModelCommand>()
            .AddTransient(_ => updateNothingModelVMFactory)
            .AddTransient(_ => dialogService)
            .AddTransient(_ => notificationService)
            .AddTransient(_ => updateNothingModelView)
            .BuildServiceProvider()
            .GetRequiredService<OpenUpdateNothingModelCommand>();
        return openUpdateNothingModelCommand;
    }
}