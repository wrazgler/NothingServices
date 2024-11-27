using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.ServicesTests;

public class DialogServiceTests
{
    [Fact]
    public void CloseDialog_Success()
    {
        //Arrange
        var dialogVM = Mock.Of<IDialogVM>();
        var dialogService = new DialogService(dialogVM);

        //Act
        dialogService.CloseDialog();
        var result = dialogVM.Open;

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void OpenDialog_Success()
    {
        //Arrange
        var dialogVMMock= new Mock<IDialogVM>();
        var dialogService = new DialogService(dialogVMMock.Object);
        var dialogContentVM = Mock.Of<IDialogContentVM>();
        var dialogContentViewMock  = new Mock<IDialogControl>();

        //Act
        dialogService.OpenDialog(dialogContentVM, dialogContentViewMock.Object);

        //Assert
        dialogContentViewMock.VerifySet(dialogControl => dialogControl.DataContext = dialogContentVM, Times.Once);
        dialogVMMock.VerifySet(dialogVM => dialogVM.Content = dialogContentViewMock.Object, Times.Once);
        dialogVMMock.VerifySet(dialogVM => dialogVM.Open = true, Times.Once);
    }
}