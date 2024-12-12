using System.Globalization;
using NothingServices.WPFApp.Converters;

namespace NothingServices.WPFApp.UnitTests.ConvertersTests;

public class StringHasValueConverterTests
{
    public static IEnumerable<object?[]> ConvertData => new List<object?[]>
    {
        new object?[] { "value", true },
        new object?[] { string.Empty, false },
        new object?[] { null, false },
        new object?[] { new(), false },
    };

    [Theory]
    [MemberData(nameof(ConvertData))]
    public void Convert_Result_Equal(object? value, bool expected)
    {
        //Arrange
        var converter = new StringHasValueConverter();

        //Act
        var result = converter.Convert(value, typeof(bool), null, CultureInfo.InvariantCulture);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertBack_Object_NotImplementedException()
    {
        //Arrange
        var converter = new StringHasValueConverter();

        //Act
        var result = new Func<object>(()
            => converter.ConvertBack(new object(), typeof(string), null, CultureInfo.InvariantCulture));

        //Assert
        Assert.Throws<NotImplementedException>(result);
    }
}