using System.Globalization;
using System.Windows.Data;
using NothingServices.WPFApp.Converters;

namespace NothingServices.WPFApp.UnitTests.ConvertersTests;

public class NotificatorHeightConverterTests
{
    [Fact]
    public void Convert_Two_Values_Success()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert([2.0, 3.0], typeof(bool), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = 6.0;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_First_String_Number_Values_Success()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert(["2,0", 3.0], typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = 6.0;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_Second_String_Number_Values_Success()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert([2.0, "3,0"], typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = 6.0;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_Both_String_Point_Number_Values_Success()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert(["2.0", "3.0"], typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = 6.0;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_Both_String_Corner_Number_Values_Success()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert(["2,0", "3,0"], typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = 6.0;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_Null_Return_Binding_DoNothing()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert(null, typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = Binding.DoNothing;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_One_Value_Return_Binding_DoNothing()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert([1.0], typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = Binding.DoNothing;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_Three_Values_Return_Binding_DoNothing()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert([1.0, 2.0, 3.0], typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = Binding.DoNothing;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_First_Null_Return_Binding_DoNothing()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert([null, 2.0], typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = Binding.DoNothing;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_Second_Null_Return_Binding_DoNothing()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert([1.0 , null], typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = Binding.DoNothing;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_First_Not_Number_Return_Binding_DoNothing()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert(["test", 2.0], typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = Binding.DoNothing;
        Assert.Equal(assert, result);
    }

    [Fact]
    public void Convert_Second_Not_Number_Return_Binding_DoNothing()
    {
        //Arrange
        var converter = new NotificatorHeightConverter();

        //Act
        var result = converter.Convert([1.0 , "test"], typeof(double), null, CultureInfo.InvariantCulture);

        //Assert
        var assert = Binding.DoNothing;
        Assert.Equal(assert, result);
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