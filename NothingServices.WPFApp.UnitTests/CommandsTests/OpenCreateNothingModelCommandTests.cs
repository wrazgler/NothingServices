using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;
using NothingServices.WPFApp.Views;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class OpenCreateNothingModelCommandTests
{
    [Fact]
    public void CanExecute_True()
    {
        //Arrange
        var command = GetOpenCreateNothingModelCommand(
            Mock.Of<ICreateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<CreateNothingModelView>());

        //Act
        var result = command.CanExecute(null);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Execute_Success()
    {
        //Arrange
        var createNothingModelVM = new CreateNothingModelVM(Mock.Of<IButtonVM>(),Mock.Of<IButtonVM>());
        var createNothingModelView = Mock.Of<CreateNothingModelView>();
        var createNothingModelVMFactoryMock = new Mock<ICreateNothingModelVMFactory>();
        createNothingModelVMFactoryMock.Setup(createNothingModelVMFactory => createNothingModelVMFactory.Create())
            .Returns(createNothingModelVM);
        var dialogServiceMock = new Mock<IDialogService>();
        dialogServiceMock.Setup(dialogService => dialogService.OpenDialog(
                It.Is<CreateNothingModelVM>(dialogContentVM => dialogContentVM == createNothingModelVM),
                It.Is<CreateNothingModelView>(dialogContentView => dialogContentView == createNothingModelView)))
            .Throws(new Exception("Fake exception"));
        var command = GetOpenCreateNothingModelCommand(
            createNothingModelVMFactoryMock.Object,
            dialogServiceMock.Object,
            Mock.Of<INotificationService>(),
            Mock.Of<CreateNothingModelView>());

        //Act
        command.Execute(null);

        //Assert
        createNothingModelVMFactoryMock.Verify(createNothingModelVMFactory => createNothingModelVMFactory.Create(), Times.Once);
        dialogServiceMock.Verify(
            dialogService => dialogService.OpenDialog(
                It.Is<CreateNothingModelVM>(dialogContentVM => dialogContentVM == createNothingModelVM),
                It.Is<CreateNothingModelView>(dialogContentView => dialogContentView == createNothingModelView)),
            Times.Once);
    }

    [Fact]
    public void Execute_Notify_Exception()
    {
        //Arrange
        var createNothingModelVMFactoryMock = new Mock<ICreateNothingModelVMFactory>();
        createNothingModelVMFactoryMock.Setup(createNothingModelVMFactory => createNothingModelVMFactory.Create())
            .Throws(new Exception("Fake exception"));
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenCreateNothingModelCommand(
            createNothingModelVMFactoryMock.Object,
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<CreateNothingModelView>());

        //Act
        command.Execute(null);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(It.Is<string>(message
                => message == "Fake exception")),
            Times.Once);
    }

    private static OpenCreateNothingModelCommand GetOpenCreateNothingModelCommand(
        ICreateNothingModelVMFactory createNothingModelVMFactory,
        IDialogService dialogService,
        INotificationService notificationService,
        CreateNothingModelView createNothingModelView)
    {
        var openCreateNothingModelCommand = new ServiceCollection()
            .AddTransient<OpenCreateNothingModelCommand>()
            .AddTransient(_ => createNothingModelVMFactory)
            .AddTransient(_ => dialogService)
            .AddTransient(_ => notificationService)
            .AddTransient(_ => createNothingModelView)
            .BuildServiceProvider()
            .GetRequiredService<OpenCreateNothingModelCommand>();
        return openCreateNothingModelCommand;
    }
}