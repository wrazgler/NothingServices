using System.Globalization;
using System.Windows.Data;
using NothingServices.WPFApp.Converters;

namespace NothingServices.WPFApp.UnitTests.ConvertersTests;

public class NotificatorHeightConverterTests
{
    public static IEnumerable<object?[]> ConvertData => new List<object?[]>
    {
        new object?[] { new object?[] { 2.0, 3.0 }, 6.0 },
        new object?[] { new object?[] { "2,0", 3.0 }, 6.0 },
        new object?[] { new object?[] { 2.0, "3,0" }, 6.0 },
        new object?[] { new object?[] { "2.0", "3.0" }, 6.0 },
        new object?[] { new object?[] { "2,0", "3,0" }, 6.0 },
        new [] { null, Binding.DoNothing },
        new [] { new object?[] { 1.0 }, Binding.DoNothing },
        new [] { new object?[] { 1.0, 2.0, 3.0 }, Binding.DoNothing },
        new [] { new object?[] { null, 2.0 }, Binding.DoNothing },
        new [] { new object?[] { 1.0 , null }, Binding.DoNothing },
        new [] { new object?[] { "test", 2.0 }, Binding.DoNothing },
        new [] { new object?[] { 1.0 , "test" }, Binding.DoNothing },
    };

    [Theory]
    [MemberData(nameof(ConvertData))]
    public void Convert_Result_Equal(object?[]? value, object expected)
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert(value, typeof(bool), null, CultureInfo.InvariantCulture);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertBack_Object_NotImplementedException()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = new Func<object>(() => converter.ConvertBack(
            new object(),
            [typeof(double), typeof(double)] ,
            null,
            CultureInfo.InvariantCulture));

        //Assert
        Assert.Throws<NotImplementedException>(result);
    }
}