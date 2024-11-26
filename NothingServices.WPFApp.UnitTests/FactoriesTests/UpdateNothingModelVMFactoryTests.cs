using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.FactoriesTests;

public class UpdateNothingModelVMFactoryTests
{
    [Fact]
    public void Create_Success()
    {
        //Arrange
        var closeDialogCommand = Mock.Of<ICloseDialogCommand>();
        var updateCommand = Mock.Of<IUpdateCommand>();
        var nothingModelVM = Mock.Of<INothingModelVM>();
        var factory = GetUpdateNothingModelVMFactory(closeDialogCommand, updateCommand);

        //Act
        var result = factory.Create(nothingModelVM);

        //Assert
        var assert = new UpdateNothingModelVM(
            new CancelButtonVM(closeDialogCommand),
            new UpdateButtonVM(updateCommand),
            nothingModelVM);
        Assert.Equivalent(assert, result, true);
    }

    private static IUpdateNothingModelVMFactory GetUpdateNothingModelVMFactory(
        ICloseDialogCommand closeDialogCommand,
        IUpdateCommand updateCommand)
    {
        var updateNothingModelVMFactory = new ServiceCollection()
            .AddTransient<IUpdateNothingModelVMFactory, UpdateNothingModelVMFactory>()
            .AddTransient(_ => closeDialogCommand)
            .AddTransient(_ => updateCommand)
            .BuildServiceProvider()
            .GetRequiredService<IUpdateNothingModelVMFactory>();
        return updateNothingModelVMFactory;
    }
}