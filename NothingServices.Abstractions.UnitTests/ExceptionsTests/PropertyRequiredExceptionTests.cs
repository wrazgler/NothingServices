using System.ComponentModel;
using NothingServices.Abstractions.Exceptions;

namespace NothingServices.Abstractions.UnitTests.ExceptionsTests;

public class PropertyRequiredExceptionTests
{
    [Fact]
    public void Message_Equal()
    {
        //Act
        var result = new PropertyRequiredException(typeof(TestClass), nameof(TestClass.TestProperty)).Message;

        //Assert
        var assert = "Поле description не может быть пустым";
        Assert.Equal(assert, result);
    }

    private class TestClass
    {
        [Description("description")]
        public required string TestProperty { get; set; }
    }
}