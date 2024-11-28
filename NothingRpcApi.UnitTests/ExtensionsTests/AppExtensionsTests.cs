using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingRpcApi.Extensions;

namespace NothingRpcApi.UnitTests.ExtensionsTests;

public class AppExtensionsTests
{
    [Fact]
    public void AddAppConfigs_Services_Count_Equal()
    {
        //Act
        var result = new ServiceCollection()
            .AddAppConfigs(Mock.Of<IConfiguration>())
            .Count;

        //Assert
        var expected = 5;
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UseAppPathBase_Contains_UsePathBaseMiddleware()
    {
        //Arrange
        var dictionary = new Dictionary<string, string>(1)
        {
            {"PATH_BASE", "/nothing-web-api"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dictionary!)
            .Build();
        var serviceProvider = new ServiceCollection()
            .BuildServiceProvider();

        //Act
        var result = new ApplicationBuilder(serviceProvider)
            .UseAppPathBase(configuration)
            .Build()
            .Target;

        //Assert
        Assert.IsType<UsePathBaseMiddleware>(result);
    }
}