using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.ViewModels.Buttons;

namespace NothingServices.WPFApp.UnitTests.ViewModelsTests;

public class NothingModelsListVMTests
{

    [Fact]
    public void Next_Active_True()
    {
        //Arrange
        var mainWindowManager = new Mock<IMainWindowManager>();
        var nothingModelsListVM = new NothingModelsListVM(
            mainWindowManager.Object,
            Mock.Of<IBackButtonVM>(),
            Mock.Of<IOpenNothingModelsListCommand>());

        //Act
        mainWindowManager.Object.Next(MainWindowContentType.NothingModelsListVM);
        var result = nothingModelsListVM.Active;

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Next_Active_False()
    {
        //Arrange
        var mainWindowManager = new Mock<IMainWindowManager>();
        var nothingModelsListVM = new NothingModelsListVM(
            mainWindowManager.Object,
            Mock.Of<IBackButtonVM>(),
            Mock.Of<IOpenNothingModelsListCommand>());

        //Act
        mainWindowManager.Object.Next(MainWindowContentType.ApiSelectionVM);
        var result = nothingModelsListVM.Active;

        //Assert
        Assert.False(result);
    }
}