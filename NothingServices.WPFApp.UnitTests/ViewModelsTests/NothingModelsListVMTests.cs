using System.Collections.ObjectModel;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.ViewModelsTests;

public class NothingModelsListVMTests
{
    [Fact]
    public void Next_Active_True()
    {
        //Arrange
        var mainWindowManager = new MainWindowManager();
        var nothingModelsListVM = new NothingModelsListVM(
            mainWindowManager,
            Mock.Of<IBackButtonVM>(),
            Mock.Of<IOpenNothingModelsListCommand>());

        //Act
        mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
        var result = nothingModelsListVM.Active;

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Next_Active_False()
    {
        //Arrange
        var mainWindowManager = new MainWindowManager();
        var nothingModelsListVM = new NothingModelsListVM(
            mainWindowManager,
            Mock.Of<IBackButtonVM>(),
            Mock.Of<IOpenNothingModelsListCommand>());

        //Act
        mainWindowManager.Next(MainWindowContentType.ApiSelectionVM);
        var result = nothingModelsListVM.Active;

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void NothingModels_Equivalent()
    {
        //Arrange
        var mainWindowManager = new MainWindowManager();
        var nothingModelsListVM = new NothingModelsListVM(
            mainWindowManager,
            Mock.Of<IBackButtonVM>(),
            Mock.Of<IOpenNothingModelsListCommand>());
        var nothingModels = new ObservableCollection<INothingModelVM>()
        {
            Mock.Of<INothingModelVM>(nothingModelVM
                => nothingModelVM.Id == 1 && nothingModelVM.Name == "Test 1"),
            Mock.Of<INothingModelVM>(nothingModelVM
                => nothingModelVM.Id == 2 && nothingModelVM.Name == "Test 2"),
        };
        var strategyMock = new Mock<INothingApiClientStrategy>();
        strategyMock
            .Setup(strategy => strategy.GetNothingModels(It.IsAny<CancellationToken>()))
            .ReturnsAsync(nothingModels);

        //Act
        mainWindowManager.Strategy = strategyMock.Object;
        mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
        var result = nothingModelsListVM.NothingModels;

        //Assert
        var expected = nothingModels;
        Assert.Equivalent(expected, result);
    }
}