using Microsoft.Extensions.Configuration;
 using Microsoft.Extensions.DependencyInjection;
 using Microsoft.Extensions.Options;
 using NothingRpcApi.Configs;
 using NothingServices.Abstractions.Exceptions;
 using NothingServices.Abstractions.Extensions;

 namespace NothingRpcApi.UnitTests.ConfigsTests;

 public class AppConfigTests
 {
     [Fact]
     public void AppConfig_Equivalent()
     {
         //Arrange
         var configuration = GetConfiguration();

         //Act
         var result = configuration.GetConfig<AppConfig>();

         //Assert
         var expected = new AppConfig()
         {
             PathBase = "/nothing-grpc-api",
         };
         Assert.Equivalent(expected, result, true);
     }

     [Fact]
     public void AppConfig_DependencyInjection_Equivalent()
     {
         //Arrange
         var configuration = GetConfiguration();
         var services = new ServiceCollection()
             .Configure<AppConfig>(configuration)
             .BuildServiceProvider();

         //Act
         var result = services.GetRequiredService<IOptions<AppConfig>>().Value;

         //Assert
         var expected = new AppConfig()
         {
             PathBase = "/nothing-grpc-api",
         };
         Assert.Equivalent(expected, result, true);
     }

     [Fact]
     public void AppConfig_Empty_Throws_ConfigurationNullException()
     {
         //Arrange
         var configuration = new ConfigurationBuilder().Build();

         //Act
         var result = new Func<AppConfig>(() => configuration.GetConfig<AppConfig>());

         //Assert
         Assert.Throws<ConfigurationNullException<AppConfig>>(result);
     }

     [Fact]
     public void AppConfig_Not_Attribute_Format_PathBase_Null()
     {
         //Arrange
         var dictionary = new Dictionary<string, string>(5)
         {
             {"AppConfig", null!},
             {"AppConfig:PathBase", "/nothing-grpc-api"},
         };
         var configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(dictionary!)
             .Build();

         //Act
         var result = configuration.GetConfig<AppConfig>().PathBase;

         //Assert
         Assert.Null(result);
     }

     private IConfiguration GetConfiguration()
     {
         var dictionary = new Dictionary<string, string>(1)
         {
             {"PATH_BASE", "/nothing-grpc-api"},
         };
         var configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(dictionary!)
             .Build();
         return configuration;
     }
 }