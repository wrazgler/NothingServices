using System.Globalization;
using FontAwesome.Sharp;
using NothingServices.WPFApp.Converters;

namespace NothingServices.WPFApp.UnitTests.ConvertersTests;

public class IconCharHasValueConverterTests
{
    [Fact]
    public void Convert_IconChar_Egg_Return_True()
    {
        //Arrange
        var converter = new IconCharHasValueConverter();

        //Act
        var result = converter.Convert(IconChar.Egg, typeof(bool), null, CultureInfo.InvariantCulture);

        //Assert
        var expected = true;
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_IconChar_None_Return_False()
    {
        //Arrange
        var converter = new IconCharHasValueConverter();

        //Act
        var result = converter.Convert(IconChar.None, typeof(bool), null, CultureInfo.InvariantCulture);

        //Assert
        var expected = false;
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_Null_Return_False()
    {
        //Arrange
        var converter = new IconCharHasValueConverter();

        //Act
        var result = converter.Convert(null, typeof(bool), null, CultureInfo.InvariantCulture);

        //Assert
        var expected = false;
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