using System.Globalization;
using FontAwesome.Sharp;
using NothingServices.WPFApp.Converters;

namespace NothingServices.WPFApp.UnitTests.ConvertersTests;

public class IconCharHasValueConverterTests
{
    [Theory]
    [InlineData(IconChar.Egg, true)]
    [InlineData(IconChar.None, false)]
    [InlineData(null, false)]
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
    public void Convert_Object_Throws_ArgumentException()
    {
        //Arrange
        var converter = new IconCharHasValueConverter();

        //Act
        var result = new Func<object>(()
            => converter.Convert(new object(), typeof(bool), null, CultureInfo.InvariantCulture));

        //Assert
        Assert.Throws<ArgumentException>(result);
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