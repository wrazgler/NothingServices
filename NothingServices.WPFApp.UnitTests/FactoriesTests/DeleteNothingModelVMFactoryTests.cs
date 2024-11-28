using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.FactoriesTests;

public class DeleteNothingModelVMFactoryTests
{
    [Fact]
    public void Create_Success()
    {
        //Arrange
        var closeDialogCommand = Mock.Of<ICloseDialogCommand>();
        var deleteCommand = Mock.Of<IDeleteCommand>();
        var nothingModelVM = Mock.Of<INothingModelVM>();
        var factory = GetDeleteNothingModelVMFactory(closeDialogCommand, deleteCommand);

        //Act
        var result = factory.Create(nothingModelVM);

        //Assert
        var expected = new DeleteNothingModelVM(
            new CancelButtonVM(closeDialogCommand),
            new DeleteButtonVM(deleteCommand),
            nothingModelVM);
        Assert.Equivalent(expected, result, true);
    }

    private static IDeleteNothingModelVMFactory GetDeleteNothingModelVMFactory(
        ICloseDialogCommand closeDialogCommand,
        IDeleteCommand deleteCommand)
    {
        var deleteNothingModelVMFactory = new ServiceCollection()
            .AddTransient<IDeleteNothingModelVMFactory, DeleteNothingModelVMFactory>()
            .AddTransient(_ => closeDialogCommand)
            .AddTransient(_ => deleteCommand)
            .BuildServiceProvider()
            .GetRequiredService<IDeleteNothingModelVMFactory>();
        return deleteNothingModelVMFactory;
    }
}