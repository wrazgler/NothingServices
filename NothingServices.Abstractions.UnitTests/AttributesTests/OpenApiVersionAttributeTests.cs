using NothingServices.Abstractions.Attributes;

namespace NothingServices.Abstractions.UnitTests.AttributesTests;

public class OpenApiVersionAttributeTests
{
    [Fact]
    public void OpenApiVersionAttribute_Version_Equal()
    {
        //Arrange
        var version = "1.0.0.0";

        //Act
        var result = new OpenApiVersionAttribute(version).Version;

        //Assert
        var assert = new Version(version);
        Assert.Equal(assert, result);
    } 
}