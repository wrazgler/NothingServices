using System.Windows.Threading;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.UnitTests.ServicesTests;

public class NotificationServiceTests
{
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