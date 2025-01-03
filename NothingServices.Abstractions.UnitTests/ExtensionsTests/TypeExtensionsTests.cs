using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NothingServices.Abstractions.Extensions;
using NpgsqlTypes;

namespace NothingServices.Abstractions.UnitTests.ExtensionsTests;

public class TypeExtensionsTests
{
    [Fact]
    public void GetDefaultValue_Equal()
    {
        //Arrange
        var type = typeof(TestClass);
        var propertyName = nameof(TestClass.TestProperty);

        //Act
        var result = type.GetDefaultValue(propertyName);

        //Assert
        var expected = "defaultValue";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetDefaultValue_Generic_Equal()
    {
        //Arrange
        var type = typeof(TestClass);
        var propertyName = nameof(TestClass.TestProperty);

        //Act
        var result = type.GetDefaultValue<string>(propertyName);

        //Assert
        var expected = "defaultValue";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetDescription_Equal()
    {
        //Arrange
        var type = typeof(TestClass);
        var propertyName = nameof(TestClass.TestProperty);

        //Act
        var result = type.GetDescription(propertyName);

        //Assert
        var expected = "description";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetJsonPropertyName_Equal()
    {
        //Arrange
        var type = typeof(TestClass);
        var propertyName = nameof(TestClass.TestProperty);

        //Act
        var result = type.GetJsonPropertyName(propertyName);

        //Assert
        var expected = "jsonName";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetTableName_Equal()
    {
        //Arrange
        var type = typeof(TestClass);

        //Act
        var result = type.GetTableName();

        //Assert
        var expected = "test_class_table";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetPgName_Equal()
    {
        //Arrange
        var type = typeof(TestClass);

        //Act
        var result = type.GetPgName();

        //Assert
        var expected = "pg_name";
        Assert.Equal(expected, result);
    }

    [PgName("pg_name")]
    [Table("test_class_table")]
    private class TestClass
    {
        [DefaultValue("defaultValue")]
        [Description("description")]
        [JsonPropertyName("jsonName")]
        public required string TestProperty { get; set; }
    }
}