using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

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
            Mock.Of<ICreateNothingModelView>());

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
        var createNothingModelView = Mock.Of<ICreateNothingModelView>();
        var createNothingModelVMFactoryMock = new Mock<ICreateNothingModelVMFactory>();
        createNothingModelVMFactoryMock
            .Setup(createNothingModelVMFactory => createNothingModelVMFactory.Create())
            .Returns(createNothingModelVM);
        var dialogServiceMock = new Mock<IDialogService>();
        dialogServiceMock.Setup(dialogService => dialogService.OpenDialog(
                It.Is<CreateNothingModelVM>(dialogContentVM => dialogContentVM == createNothingModelVM),
                It.Is<ICreateNothingModelView>(dialogContentView => dialogContentView == createNothingModelView)));
        var command = GetOpenCreateNothingModelCommand(
            createNothingModelVMFactoryMock.Object,
            dialogServiceMock.Object,
            Mock.Of<INotificationService>(),
            createNothingModelView);

        //Act
        command.Execute(null);

        //Assert
        createNothingModelVMFactoryMock.Verify(createNothingModelVMFactory => createNothingModelVMFactory.Create(), Times.Once);
        dialogServiceMock.Verify(
            dialogService => dialogService.OpenDialog(
                It.Is<CreateNothingModelVM>(dialogContentVM => dialogContentVM == createNothingModelVM),
                It.Is<ICreateNothingModelView>(dialogContentView => dialogContentView == createNothingModelView)),
            Times.Once);
    }

    [Fact]
    public void Execute_Notify_Exception()
    {
        //Arrange
        var createNothingModelVMFactoryMock = new Mock<ICreateNothingModelVMFactory>();
        createNothingModelVMFactoryMock
            .Setup(createNothingModelVMFactory => createNothingModelVMFactory.Create())
            .Throws(new Exception("Fake exception"));
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenCreateNothingModelCommand(
            createNothingModelVMFactoryMock.Object,
            Mock.Of<IDialogService>(),
            notificationServiceMock.Object,
            Mock.Of<ICreateNothingModelView>());

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
        ICreateNothingModelView createNothingModelView)
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