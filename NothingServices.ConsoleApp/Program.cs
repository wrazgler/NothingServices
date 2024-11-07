using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingServices.ConsoleApp.Extensions;
using NothingServices.ConsoleApp.Services;

var hostBuilder = Host.CreateDefaultBuilder(args);
hostBuilder.ConfigureAppConfiguration((_, configurationBuilder) =>
{
    configurationBuilder.AddEnvironmentVariables();
});
hostBuilder.ConfigureServices((context, services) =>
{
    services.AddAppConfigs(context.Configuration);
    services.AddAppHttpClient(context.Configuration);
    services.AddAppClients(context.Configuration);
    services.AddAppServices();
    services.AddHostedService<HostedService>();
});
var host = hostBuilder.Build();
await host.RunAsync();