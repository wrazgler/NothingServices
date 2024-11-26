using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.FactoriesTests;

public class NothingModelVMFactoryTests
{
    [Fact]
    public void Create_Success()
    {
        //Arrange
        var openDeleteNothingModelCommand = Mock.Of<IOpenDeleteNothingModelCommand>();
        var openUpdateNothingModelCommand = Mock.Of<IOpenUpdateNothingModelCommand>();
        var nothingModelDto = Mock.Of<NothingModelDto>(model => model.Id == 1 && model.Name == "test");
        var factory = GetNothingModelVMFactory(openDeleteNothingModelCommand, openUpdateNothingModelCommand);

        //Act
        var result = factory.Create(nothingModelDto);

        //Assert
        var assert = new NothingModelVM()
        {
            Id = nothingModelDto.Id,
            Name = nothingModelDto.Name,
            DeleteButtonVM = new DeleteButtonVM(openDeleteNothingModelCommand),
            UpdateButtonVM = new UpdateButtonVM(openUpdateNothingModelCommand),
        };
        Assert.Equivalent(assert, result, true);
    }

    private static INothingModelVMFactory GetNothingModelVMFactory(
        IOpenDeleteNothingModelCommand openDeleteNothingModelCommand,
        IOpenUpdateNothingModelCommand openUpdateNothingModelCommand)
    {
        var nothingModelVMFactory = new ServiceCollection()
            .AddTransient<INothingModelVMFactory, NothingModelVMFactory>()
            .AddTransient(_ => openDeleteNothingModelCommand)
            .AddTransient(_ => openUpdateNothingModelCommand)
            .BuildServiceProvider()
            .GetRequiredService<INothingModelVMFactory>();
        return nothingModelVMFactory;
    }
}