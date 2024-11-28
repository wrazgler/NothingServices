using System.Collections.ObjectModel;
using NothingServices.WPFApp.Extensions;

namespace NothingServices.WPFApp.UnitTests.ExtensionsTests;

public class EnumerableExtensionTests
{
    [Fact]
    public void ToObservableCollection_Success()
    {
        //Arrange
        var items = new List<string> {"a", "b", "c"};

        //Act
        var result = items.ToObservableCollection();

        //Assert
        var expected = new ObservableCollection<string> {"a", "b", "c"};
        Assert.Equivalent(expected, result, true);
    }
}