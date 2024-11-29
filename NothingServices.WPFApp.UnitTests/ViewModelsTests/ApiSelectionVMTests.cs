using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.ViewModels.Buttons;

namespace NothingServices.WPFApp.UnitTests.ViewModelsTests;

public class ApiSelectionVMTests
{
    [Fact]
    public void Next_Active_True()
    {
        //Arrange
        var mainWindowManager = new MainWindowManager();
        var apiSelectionVM = new ApiSelectionVM(
            mainWindowManager,
            Mock.Of<IGRpcApiButtonVM>(),
            Mock.Of<IRestApiButtonVM>());

        //Act
        mainWindowManager.Next(MainWindowContentType.ApiSelectionVM);
        var result = apiSelectionVM.Active;

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Next_Active_False()
    {
        //Arrange
        var mainWindowManager = new MainWindowManager();
        var apiSelectionVM = new ApiSelectionVM(
            mainWindowManager,
            Mock.Of<IGRpcApiButtonVM>(),
            Mock.Of<IRestApiButtonVM>());

        //Act
        mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
        var result = apiSelectionVM.Active;

        //Assert
        Assert.False(result);
    }
}