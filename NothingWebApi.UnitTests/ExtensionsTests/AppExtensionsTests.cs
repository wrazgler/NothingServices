using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NothingWebApi.Controllers;
using NothingWebApi.Extensions;
using NothingWebApi.Services;

namespace NothingWebApi.UnitTests.ExtensionsTests;

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
    public void AddAppControllers_Create_NothingWebApiController()
    {
        //Arrange
        var serviceCollection = new ServiceCollection()
            .AddTransient(_ => Mock.Of<INothingService>());

        //Act
        var serviceProvider = serviceCollection
            .AddAppControllers()
            .BuildServiceProvider();
        var context = new ControllerContext
        {
            ActionDescriptor = new ControllerActionDescriptor()
            {
                ControllerTypeInfo = typeof(NothingWebApiController).GetTypeInfo()
            },
            HttpContext = new DefaultHttpContext()
            {
                RequestServices = serviceProvider
            }
        };
        var result = serviceProvider
            .GetRequiredService<IControllerActivator>()
            .Create(context);

        //Assert
        Assert.IsType<NothingWebApiController>(result);
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