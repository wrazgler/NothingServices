using System.Windows.Threading;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.UnitTests.ServicesTests;

public class NotificationServiceTests
{
    [Fact]
    public void Pair_Success()
    {
        //Arrange
        var notificator = Mock.Of<INotificator>();
        var notificationService = new NotificationService(Dispatcher.CurrentDispatcher);

        //Act
        notificationService.Pair(notificator);
        var result = notificationService.Notificator;

        //Assert
        Assert.Equal(notificator, result);
    }

    [Fact]
    public void Pair_Return_Action_Invoce_Success()
    {
        //Arrange
        var notificator = Mock.Of<INotificator>();
        var notificationService = new NotificationService(Dispatcher.CurrentDispatcher);

        //Act
        notificationService.Pair(notificator).Invoke();
        var result = notificationService.Notificator;

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void ValidateDispatcher_Return_True()
    {
        //Arrange
        var notificationService = new NotificationService(Dispatcher.CurrentDispatcher);

        //Act
        var result = notificationService.ValidateDispatcher(Dispatcher.CurrentDispatcher);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateDispatcher_Return_False()
    {
        //Arrange
        var notificationService = new NotificationService(Dispatcher.CurrentDispatcher);
        var thread = new Thread(_ => { });
        var dispatcher = Dispatcher.FromThread(thread)!;

        //Act
        var result = notificationService.ValidateDispatcher(dispatcher);

        //Assert
        Assert.False(result);
    }
}