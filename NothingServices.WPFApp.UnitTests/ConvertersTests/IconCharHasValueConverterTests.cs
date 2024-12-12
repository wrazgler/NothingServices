using System.Globalization;
using FontAwesome.Sharp;
using NothingServices.WPFApp.Converters;

namespace NothingServices.WPFApp.UnitTests.ConvertersTests;

public class IconCharHasValueConverterTests
{
    public static IEnumerable<object?[]> ConvertData => new List<object?[]>
    {
        new object?[] { IconChar.Egg, true },
        new object?[] { IconChar.None, true },
        new object?[] { null, false },
        new object?[] { new(), false },
    };

    [Theory]
    [MemberData(nameof(ConvertData))]
    public void Convert_Result_Equal(object? value, bool expected)
    {
        //Arrange
        var converter = new IconCharHasValueConverter();

        //Act
        var result = converter.Convert(value, typeof(bool), null, CultureInfo.InvariantCulture);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertBack_Object_NotImplementedException()
    {
        //Arrange
        var converter = new IconCharHasValueConverter();

        //Act
        var result = new Func<object>(()
            => converter.ConvertBack(new object(), typeof(IconChar), null, CultureInfo.InvariantCulture));

        //Assert
        Assert.Throws<NotImplementedException>(result);
    }
}