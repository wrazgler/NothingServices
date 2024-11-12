using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.UnitTests.ServicesTests;

public class AppVersionProviderTests
{
    [Fact]
    public void AppVersionProvider_GetVersion_Equal()
    {
        //Arrange
        var appVersionProvider = new AppVersionProvider();

        //Act
        var result = appVersionProvider.GetVersion();

        //Assert
        var assert = new Version("1.0.0.0");
        Assert.Equivalent(assert, result);
    }
}