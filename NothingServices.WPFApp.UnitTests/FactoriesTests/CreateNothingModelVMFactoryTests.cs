using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.FactoriesTests;

public class CreateNothingModelVMFactoryTests
{
    [Fact]
    public void Create_Success()
    {
        //Arrange
        var closeDialogCommand = Mock.Of<ICloseDialogCommand>();
        var createCommand = Mock.Of<ICreateCommand>();
        var factory = GetCreateNothingModelVMFactory(closeDialogCommand, createCommand);

        //Act
        var result = factory.Create();

        //Assert
        var expected = new CreateNothingModelVM(
            new CancelButtonVM(closeDialogCommand),
            new CreateButtonVM(createCommand));
        Assert.Equivalent(expected, result, true);
    }

    private static ICreateNothingModelVMFactory GetCreateNothingModelVMFactory(
        ICloseDialogCommand closeDialogCommand,
        ICreateCommand createCommand)
    {
        var createNothingModelVMFactory = new ServiceCollection()
            .AddTransient<ICreateNothingModelVMFactory, CreateNothingModelVMFactory>()
            .AddTransient(_ => closeDialogCommand)
            .AddTransient(_ => createCommand)
            .BuildServiceProvider()
            .GetRequiredService<ICreateNothingModelVMFactory>();
        return createNothingModelVMFactory;
    }
}