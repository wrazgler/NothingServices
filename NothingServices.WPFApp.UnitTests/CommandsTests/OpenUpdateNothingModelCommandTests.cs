using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.CommandsTests;

public class OpenUpdateNothingModelCommandTests
{
    public static IEnumerable<object?[]> CanExecuteData => new List<object?[]>
    {
        new object?[]
        {
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1 && nothingModelVM.Name == "test"),
            true,
        },
        new object?[]
        {
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 0 && nothingModelVM.Name == "test"),
            false,
        },
        new object?[]
        {
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1 && nothingModelVM.Name == string.Empty),
            false,
        },
        new object?[]
        {
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1 && nothingModelVM.Name == "    "),
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
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            Mock.Of<INotificationService>(),
            Mock.Of<IUpdateNothingModelView>());

        //Act
        var result = command.CanExecute(parameter);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Execute_Success()
    {
        //Arrange
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == 1 && nothingModelVM.Name == "test");
        var updateNothingModelVM = new UpdateNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);
        var updateNothingModelView = Mock.Of<IUpdateNothingModelView>();
        var updateNothingModelVMFactoryMock = new Mock<IUpdateNothingModelVMFactory>();
        updateNothingModelVMFactoryMock
            .Setup(updateNothingModelVMFactory => updateNothingModelVMFactory
                .Create(It.Is<INothingModelVM>(model => model == nothingModelVM)))
            .Returns(updateNothingModelVM);
        var dialogServiceMock = new Mock<IDialogService>();
        dialogServiceMock.Setup(dialogService => dialogService.OpenDialog(
                It.Is<UpdateNothingModelVM>(dialogContentVM => dialogContentVM == updateNothingModelVM),
                It.Is<IUpdateNothingModelView>(dialogContentView => dialogContentView == updateNothingModelView)));
        var command = GetOpenUpdateNothingModelCommand(
            updateNothingModelVMFactoryMock.Object,
            dialogServiceMock.Object,
            Mock.Of<INotificationService>(),
            updateNothingModelView);

        //Act
        command.Execute(nothingModelVM);

        //Assert
        updateNothingModelVMFactoryMock.Verify(
            updateNothingModelVMFactory => updateNothingModelVMFactory
                .Create(It.Is<INothingModelVM>(model => model == nothingModelVM)),
            Times.Once);
        dialogServiceMock.Verify(
            dialogService => dialogService.OpenDialog(
                It.Is<UpdateNothingModelVM>(dialogContentVM => dialogContentVM == updateNothingModelVM),
                It.Is<IUpdateNothingModelView>(dialogContentView => dialogContentView == updateNothingModelView)),
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
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 0 && nothingModelVM.Name == "test"),
            "Поле Идентификатор модели не может быть пустым",
        },
        new object?[]
        {
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1 && nothingModelVM.Name == null!),
            "Поле Имя модели не может быть пустым",
        },
        new object?[]
        {
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1 && nothingModelVM.Name == string.Empty),
            "Поле Имя модели не может быть пустым",
        },
        new object?[]
        {
            Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == 1 && nothingModelVM.Name == "   "),
            "Поле Имя модели не может быть пустым",
        },
    };

    [Theory]
    [MemberData(nameof(ExecuteErrorData))]
    public void Execute_Error_Parameter_Error_Message_Equal(object? parameter, string errorMessage)
    {
        //Arrange
        var notificationServiceMock = new Mock<INotificationService>();
        var command = GetOpenUpdateNothingModelCommand(
            Mock.Of<IUpdateNothingModelVMFactory>(),
            Mock.Of<IDialogService>(),
            notificationServiceMock.Object,
            Mock.Of<IUpdateNothingModelView>());

        //Act
        command.Execute(parameter);

        //Assert
        notificationServiceMock.Verify(
            notificationService => notificationService.Notify(
                It.Is<string>(message => message == errorMessage),
                It.IsAny<string>()),
            Times.Once);
    }

    private static OpenUpdateNothingModelCommand GetOpenUpdateNothingModelCommand(
        IUpdateNothingModelVMFactory updateNothingModelVMFactory,
        IDialogService dialogService,
        INotificationService notificationService,
        IUpdateNothingModelView updateNothingModelView)
    {
        var openUpdateNothingModelCommand = new ServiceCollection()
            .AddTransient<OpenUpdateNothingModelCommand>()
            .AddTransient(_ => updateNothingModelVMFactory)
            .AddTransient(_ => dialogService)
            .AddTransient(_ => notificationService)
            .AddTransient(_ => updateNothingModelView)
            .BuildServiceProvider()
            .GetRequiredService<OpenUpdateNothingModelCommand>();
        return openUpdateNothingModelCommand;
    }
}