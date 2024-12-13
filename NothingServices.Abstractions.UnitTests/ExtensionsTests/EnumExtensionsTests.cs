using System.ComponentModel;
using NothingServices.Abstractions.Extensions;
using NpgsqlTypes;

namespace NothingServices.Abstractions.UnitTests.ExtensionsTests;

public class EnumExtensionsTests
{
    [Fact]
    public void GetDescription_Equal()
    {
        //Arrange
        var enumValue = TestEnum.Test;

        //Act
        var result = enumValue.GetDescription();

        //Assert
        var expected = "test";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetPgName_Equal()
    {
        //Arrange
        var enumValue = TestEnum.Test;

        //Act
        var result = enumValue.GetPgName();

        //Assert
        var expected = "test";
        Assert.Equal(expected, result);
    }

    private enum TestEnum
    {
        [PgName("test")]
        [Description("test")]
        Test,
    }
}