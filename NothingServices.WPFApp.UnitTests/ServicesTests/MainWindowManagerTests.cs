using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.UnitTests.ServicesTests;

public class MainWindowManagerTests
{
    [Fact]
    public void Next_Success()
    {
        //Arrange
        var mainWindowManager = new MainWindowManager();
        var invoke = false;
        var onNext = new Action<MainWindowContentType>(_ => invoke = true);

        //Act
        mainWindowManager.OnNext += onNext;
        mainWindowManager.Next(MainWindowContentType.ApiSelectionVM);

        //Assert
        Assert.True(invoke);
    }
}