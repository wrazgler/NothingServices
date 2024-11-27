using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels;

namespace NothingServices.WPFApp.UnitTests.ServicesTests;

public class StartupServiceTests
{
    [Fact]
    public void Start_Success()
    {
        //Arrange
        var mainWindowManagerMock = new Mock<IMainWindowManager>();
        var mainWindowMock = new Mock<IMainWindow>();
        var mainWindowVM = Mock.Of<IMainWindowVM>();
        var startupService = new StartupService(mainWindowManagerMock.Object, mainWindowMock.Object, mainWindowVM);

        //Act
        startupService.Start();

        //Assert
        mainWindowManagerMock.Verify(
            mainWindowManager => mainWindowManager
                .Next(It.Is<MainWindowContentType>(type => type == MainWindowContentType.ApiSelectionVM)),
            Times.Once);
        mainWindowMock.VerifySet(mainWindow => mainWindow.DataContext = mainWindowVM, Times.Once);
        mainWindowMock.Verify(mainWindow => mainWindow.Show(), Times.Once);
    }
}