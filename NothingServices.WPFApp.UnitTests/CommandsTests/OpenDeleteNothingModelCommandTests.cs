using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class OpenDeleteNothingModelCommandTests
{
    public static IEnumerable<object?[]> CanExecuteData => new List<object?[]>
    {
        new object?[]
        {
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1),
            true,
        },
        new object?[]
        {
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 0),
            false,
        },
        new object?[] { null, false },
        new object?[] { new(), false },
    };

    [Theory]
    [MemberData(nameof(CanExecuteData))]
    public void CanExecute_Result_Equal(object? parameter, bool expected)
    {
        //Arrange
        var command = GetOpenDeleteNothingModelCommand(
            Mock.Of<IDeleteNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IDeleteNothingModelView>());

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Execute_Success()
    {
        //Arrange
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1);
        var deleteNothingModelVM = new DeleteNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);
        var deleteNothingModelView = Mock.Of<IDeleteNothingModelView>();
        var deleteNothingModelVMFactoryMock = new Mock<IDeleteNothingModelVMFactory>();
        deleteNothingModelVMFactoryMock
            .Setup(deleteNothingModelVMFactory => deleteNothingModelVMFactory
                .Create(It.Is<INothingModelVM>(model => model == nothingModelVM)))
            .Returns(deleteNothingModelVM);
        var dialogServiceMock = new Mock<IDialogService>();
        dialogServiceMock.Setup(dialogService => dialogService.OpenDialog(
                It.Is<DeleteNothingModelVM>(dialogContentVM => dialogContentVM == deleteNothingModelVM),
                It.Is<IDeleteNothingModelView>(dialogContentView => dialogContentView == deleteNothingModelView)));
        var command = GetOpenDeleteNothingModelCommand(
            deleteNothingModelVMFactoryMock.Object,
            dialogServiceMock.Object,
            Mock.Of<INotificationService>(),
            deleteNothingModelView);

        //Act
        command.Execute(nothingModelVM);

        //Assert
        deleteNothingModelVMFactoryMock.Verify(
            deleteNothingModelVMFactory => deleteNothingModelVMFactory
                .Create(It.Is<INothingModelVM>(model => model == nothingModelVM)),
            Times.Once);
        dialogServiceMock.Verify(
            dialogService => dialogService.OpenDialog(
                It.Is<DeleteNothingModelVM>(dialogContentVM => dialogContentVM == deleteNothingModelVM),
                It.Is<IDeleteNothingModelView>(dialogContentView => dialogContentView == deleteNothingModelView)),
            Times.Once);
    }

    public static IEnumerable<object?[]> ExecuteErrorData => new List<object?[]>
    {
        new object?[]
        {
            null,
            "Value cannot be null. (Parameter 'parameter')",
        },
        new object?[]
        {
            new (),
            "Некорректный тип параметра команды: Object",
        },
        new object?[]
        {
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 0),
            "Поле Идентификатор модели не может быть пустым",
        },
    };

    [Theory]
    [MemberData(nameof(ExecuteErrorData))]
    public void Execute_Error_Parameter_Error_Message_Equal(object? parameter, string errorMessage)
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenDeleteNothingModelCommand(
            Mock.Of<IDeleteNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            notificationServiceMock.Object,
            Mock.Of<IDeleteNothingModelView>());

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == errorMessage),
                It.IsAny<string>()),
            Times.Once);
    }

    private static OpenDeleteNothingModelCommand GetOpenDeleteNothingModelCommand(
        IDeleteNothingModelVMFactory deleteNothingModelVMFactory,
        IDialogService dialogService,
        INotificationService notificationService,
        IDeleteNothingModelView deleteNothingModelView)
    {
        var openUpdateNothingModelCommand = new ServiceCollection()
            .AddTransient<OpenDeleteNothingModelCommand>()
            .AddTransient(_ => deleteNothingModelVMFactory)
            .AddTransient(_ => dialogService)
            .AddTransient(_ => notificationService)
            .AddTransient(_ => deleteNothingModelView)
            .BuildServiceProvider()
            .GetRequiredService<OpenDeleteNothingModelCommand>();
        return openUpdateNothingModelCommand;
    }
}