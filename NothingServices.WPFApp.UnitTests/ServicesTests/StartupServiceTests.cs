using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.Views;

namespace NothingServices.WPFApp.UnitTests.ServicesTests;

public class StartupServiceTests
{
    [Fact]
    public void StartupService_Start_Success()
    {
        //Arrange
        var mainWindowMock = new Mock<MainWindow>();
        var mainWindowVMMock = new Mock<MainWindowVM>();
        var mainWindow = mainWindowMock.Object;
        var mainWindowVM = mainWindowVMMock.Object;
        var startupService = new StartupService(mainWindow, mainWindowVM);

        //Act
        startupService.Start();
        var result = mainWindow.DataContext;

        //Assert
        var assert = mainWindowVM;
        Assert.Equal(assert, result);
        mainWindowMock.Verify(window => window.Show(), Times.Once);
    }
}