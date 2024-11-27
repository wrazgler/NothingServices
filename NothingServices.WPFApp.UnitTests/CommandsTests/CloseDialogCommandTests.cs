using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class CloseDialogCommandTests
{
    [Fact]
    public void CanExecute_True()
    {
        //Arrange
        var command = GetCloseDialogCommand(
            Mock.Of<IDialogService>(),
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
        var dialogServiceMock = new Mock<IDialogService>();
        var command = GetCloseDialogCommand(
            dialogServiceMock.Object,
            Mock.Of<INotificationService>());

        //Act
        command.Execute(null);

        //Assert
        dialogServiceMock.Verify(dialogService => dialogService.CloseDialog(), Times.Once);
    }

    [Fact]
    public void Execute_Notify_Exception()
    {
        //Arrange
        var dialogServiceMock = new Mock<IDialogService>();
        dialogServiceMock.Setup(dialogService => dialogService.CloseDialog())
            .Throws(new Exception("Fake exception"));
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetCloseDialogCommand(
            dialogServiceMock.Object,
            notificationServiceMock.Object);

        //Act
        command.Execute(null);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == "Fake exception"),
                It.IsAny<string>()),
            Times.Once);
    }

    private static CloseDialogCommand GetCloseDialogCommand(
        IDialogService dialogService,
        INotificationService notificationService)
    {
        var closeDialogCommand = new ServiceCollection()
            .AddTransient<CloseDialogCommand>()
            .AddTransient(_ => dialogService)
            .AddTransient(_ => notificationService)
            .BuildServiceProvider()
            .GetRequiredService<CloseDialogCommand>();
        return closeDialogCommand;
    }
}