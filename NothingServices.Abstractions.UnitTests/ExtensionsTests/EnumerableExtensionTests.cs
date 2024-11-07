namespace NothingServices.Abstractions.UnitTests.ExtensionsTests;

public class EnumerableExtensionTests
{
    [Fact]
    public void ForEach_Invoke_Action_Success()
    {
        //Arrange
        var collection = new List<int>(){1,2,3,4,5,6,7,8,9};
        
        //Act
        var result = 0;
        collection.ForEach(x => result += x);
        
        //Assert
        var assert = collection.Sum(x => x);
        Assert.Equal(assert, result);
    }
}