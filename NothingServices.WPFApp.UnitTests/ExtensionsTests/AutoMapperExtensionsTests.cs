using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.Extensions;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.UnitTests.ExtensionsTests;

public class AutoMapperExtensionsTests
{
    [Fact]
    public void Map_CreateNothingModelVM_To_CreateNothingModelDto_Success()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var name = "Test";
        var createNothingModelVM = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = name
        };

        //Act
        var result = mapper.Map<CreateNothingModelDto>(createNothingModelVM);

        //Assert
        var assert = new CreateNothingModelDto()
        {
            Name = name,
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public void Map_CreateNothingModelDto_To_CreateNothingModelVM_Throws_AutoMapperMappingException()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = "Test",
        };

        //Act
        var result = new Func<CreateNothingModelVM>(() => mapper.Map<CreateNothingModelVM>(createNothingModelDto));

        //Assert
        Assert.Throws<AutoMapperMappingException>(result);
    }

    [Fact]
    public void Map_CreateNothingModelVM_To_CreateNothingModelWebDto_Success()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var name = "Test";
        var createNothingModelVM = new CreateNothingModelVM(Mock.Of<IButtonVM>(), Mock.Of<IButtonVM>())
        {
            Name = name
        };

        //Act
        var result = mapper.Map<CreateNothingModelWebDto>(createNothingModelVM);

        //Assert
        var assert = new CreateNothingModelWebDto()
        {
            Name = name,
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public void Map_CreateNothingModelWebDto_To_CreateNothingModelVM_Throws_AutoMapperMappingException()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var createNothingModelWebDto = new CreateNothingModelWebDto()
        {
            Name = "Test",
        };

        //Act
        var result = new Func<CreateNothingModelVM>(() => mapper.Map<CreateNothingModelVM>(createNothingModelWebDto));

        //Assert
        Assert.Throws<AutoMapperMappingException>(result);
    }

    [Fact]
    public void Map_DeleteNothingModelVM_To_NothingModelIdDto_Success()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var id = 1;
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM => nothingModelVM.Id == id);
        var deleteNothingModelVM = new DeleteNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);

        //Act
        var result = mapper.Map<NothingModelIdDto>(deleteNothingModelVM);

        //Assert
        var assert = new NothingModelIdDto()
        {
            Id = id,
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public void Map_NothingModelIdDto_To_DeleteNothingModelVM_Throws_AutoMapperMappingException()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var nothingModelIdDto = new NothingModelIdDto()
        {
            Id = 1,
        };

        //Act
        var result = new Func<DeleteNothingModelVM>(() => mapper.Map<DeleteNothingModelVM>(nothingModelIdDto));

        //Assert
        Assert.Throws<AutoMapperMappingException>(result);
    }

    [Fact]
    public void Map_UpdateNothingModelVM_To_UpdateNothingModelDto_Success()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var id = 1;
        var name = "Test";
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == id && nothingModelVM.Name == name);
        var updateNothingModelVM = new UpdateNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);

        //Act
        var result = mapper.Map<UpdateNothingModelDto>(updateNothingModelVM);

        //Assert
        var assert = new UpdateNothingModelDto()
        {
            Id = id,
            Name = name,
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public void Map_UpdateNothingModelDto_To_UpdateNothingModelVM_Throws_AutoMapperMappingException()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var updateNothingModelDto = new UpdateNothingModelDto()
        {
            Id = 1,
            Name = "Test",
        };

        //Act
        var result = new Func<UpdateNothingModelVM>(() => mapper.Map<UpdateNothingModelVM>(updateNothingModelDto));

        //Assert
        Assert.Throws<AutoMapperMappingException>(result);
    }

    [Fact]
    public void Map_UpdateNothingModelVM_To_UpdateNothingModelWebDto_Success()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var id = 1;
        var name = "Test";
        var nothingModelVM = Mock.Of<INothingModelVM>(nothingModelVM
            => nothingModelVM.Id == id && nothingModelVM.Name == name);
        var updateNothingModelVM = new UpdateNothingModelVM(
            Mock.Of<IButtonVM>(),
            Mock.Of<IButtonVM>(),
            nothingModelVM);

        //Act
        var result = mapper.Map<UpdateNothingModelWebDto>(updateNothingModelVM);

        //Assert
        var assert = new UpdateNothingModelWebDto()
        {
            Id = id,
            Name = name,
        };
        Assert.Equivalent(assert, result, true);
    }

    [Fact]
    public void Map_UpdateNothingModelWebDto_To_UpdateNothingModelVM_Throws_AutoMapperMappingException()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var updateNothingModelWebDto = new UpdateNothingModelWebDto()
        {
            Id = 1,
            Name = "Test",
        };

        //Act
        var result = new Func<UpdateNothingModelVM>(() => mapper.Map<UpdateNothingModelVM>(updateNothingModelWebDto));

        //Assert
        Assert.Throws<AutoMapperMappingException>(result);
    }
}