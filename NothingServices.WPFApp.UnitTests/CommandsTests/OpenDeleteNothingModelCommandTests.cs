using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class OpenDeleteNothingModelCommandTests
{
    [Fact]
    public void CanExecute_True()
    {
        //Arrange
        var command = GetOpenDeleteNothingModelCommand(
            Mock.Of<IDeleteNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IDeleteNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1);

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void CanExecute_Id_0_False()
    {
        //Arrange
        var command = GetOpenDeleteNothingModelCommand(
            Mock.Of<IDeleteNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IDeleteNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 0);

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CanExecute_Parameter_Null_False()
    {
        //Arrange
        var command = GetOpenDeleteNothingModelCommand(
            Mock.Of<IDeleteNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IDeleteNothingModelView>());

        //Act
        var result = command.CanExecute(null);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CanExecute_Parameter_Object_False()
    {
        //Arrange
        var command = GetOpenDeleteNothingModelCommand(
            Mock.Of<IDeleteNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IDeleteNothingModelView>());

        //Act
        var result = command.CanExecute(new object());

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void Execute_Success()
    {
        //Arrange
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1);
        var deleteNothingModelVM = new DeleteNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);
        var deleteNothingModelView = Mock.Of<IDeleteNothingModelView>();
        var deleteNothingModelVMFactoryMock = new Mock<IDeleteNothingModelVMFactory>();
        deleteNothingModelVMFactoryMock
            .Setup(deleteNothingModelVMFactory => deleteNothingModelVMFactory
                .Create(It.Is<INothingModelVM>(model => model == nothingModelVM)))
            .Returns(deleteNothingModelVM);
        var dialogServiceMock = new Mock<IDialogService>();
        dialogServiceMock.Setup(dialogService => dialogService.OpenDialog(
                It.Is<DeleteNothingModelVM>(dialogContentVM => dialogContentVM == deleteNothingModelVM),
                It.Is<IDeleteNothingModelView>(dialogContentView => dialogContentView == deleteNothingModelView)));
        var command = GetOpenDeleteNothingModelCommand(
            deleteNothingModelVMFactoryMock.Object,
            dialogServiceMock.Object,
            Mock.Of<INotificationService>(),
            deleteNothingModelView);

        //Act
        command.Execute(nothingModelVM);

        //Assert
        deleteNothingModelVMFactoryMock.Verify(
            deleteNothingModelVMFactory => deleteNothingModelVMFactory
                .Create(It.Is<INothingModelVM>(model => model == nothingModelVM)),
            Times.Once);
        dialogServiceMock.Verify(
            dialogService => dialogService.OpenDialog(
                It.Is<DeleteNothingModelVM>(dialogContentVM => dialogContentVM == deleteNothingModelVM),
                It.Is<IDeleteNothingModelView>(dialogContentView => dialogContentView == deleteNothingModelView)),
            Times.Once);
    }

    [Fact]
    public void Execute_Parameter_Object_Notify_Error()
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenDeleteNothingModelCommand(
            Mock.Of<IDeleteNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            notificationServiceMock.Object,
            Mock.Of<IDeleteNothingModelView>());

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
        var command = GetOpenDeleteNothingModelCommand(
            Mock.Of<IDeleteNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            notificationServiceMock.Object,
            Mock.Of<IDeleteNothingModelView>());
        var parameter = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 0);

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(It.Is<string>(message
                => message == "Поле Идентификатор модели не может быть пустым")),
            Times.Once);
    }

    private static OpenDeleteNothingModelCommand GetOpenDeleteNothingModelCommand(
        IDeleteNothingModelVMFactory deleteNothingModelVMFactory,
        IDialogService dialogService,
        INotificationService notificationService,
        IDeleteNothingModelView deleteNothingModelView)
    {
        var openUpdateNothingModelCommand = new ServiceCollection()
            .AddTransient<OpenDeleteNothingModelCommand>()
            .AddTransient(_ => deleteNothingModelVMFactory)
            .AddTransient(_ => dialogService)
            .AddTransient(_ => notificationService)
            .AddTransient(_ => deleteNothingModelView)
            .BuildServiceProvider()
            .GetRequiredService<OpenDeleteNothingModelCommand>();
        return openUpdateNothingModelCommand;
    }
}