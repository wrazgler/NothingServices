using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NothingRpcApi.Dtos;
using NothingRpcApi.Extensions;
using NothingRpcApi.Models;

namespace NothingRpcApi.UnitTests.ExtensionsTests;

public class AutoMapperExtensionsTests
{
    [Fact]
    public void Map_NothingModel_To_NothingModelDto_Success()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var id = 1;
        var name = "Test";
        var nothingModel = new NothingModel()
        {
            Id = id,
            Name = name
        };

        //Act
        var result = mapper.Map<NothingModelDto>(nothingModel);

        //Assert
        var expected = new NothingModelDto()
        {
            Id = id,
            Name = name,
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void Map_NothingModelDto_To_NothingModel_Throws_AutoMapperMappingException()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var nothingModelDto = new NothingModelDto()
        {
            Name = "Test"
        };

        //Act
        var result = new Func<NothingModel>(() => mapper.Map<NothingModel>(nothingModelDto));

        //Assert
        Assert.Throws<AutoMapperMappingException>(result);
    }

    [Fact]
    public void Map_CreateNothingModelDto_To_NothingModel_Success()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var name = "Test";
        var createNothingModelDto = new CreateNothingModelDto()
        {
            Name = name
        };

        //Act
        var result = mapper.Map<NothingModel>(createNothingModelDto);

        //Assert
        var expected = new NothingModel()
        {
            Id = 0,
            Name = name,
        };
        Assert.Equivalent(expected, result, true);
    }

    [Fact]
    public void Map_NothingModel_To_CreateNothingModelDto_Throws_AutoMapperMappingException()
    {
        //Arrange
        var mapper = new ServiceCollection()
            .AddAppAutoMapper()
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();
        var nothingModel = new NothingModel()
        {
            Name = "Test"
        };

        //Act
        var result = new Func<CreateNothingModelDto>(() => mapper.Map<CreateNothingModelDto>(nothingModel));

        //Assert
        Assert.Throws<AutoMapperMappingException>(result);
    }
}