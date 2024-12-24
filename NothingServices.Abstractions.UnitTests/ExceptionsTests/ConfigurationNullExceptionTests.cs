using NothingServices.Abstractions.Exceptions;

namespace NothingServices.Abstractions.UnitTests.ExceptionsTests;

public class ConfigurationNullExceptionTests
{
    [Fact]
    public void Message_Equal()
    {
        //Act
        var result = new ConfigurationNullException<ConfigurationNullExceptionTests>().Message;

        //Assert
        var expected = $"Конфигурация {nameof(ConfigurationNullExceptionTests)} не обнаружена";
        Assert.Equal(expected, result);
    }
}