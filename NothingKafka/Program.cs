using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NothingKafka.DbContexts;
using NothingKafka.Extensions;
using NothingKafka.Services;

var hostBuilder = Host.CreateDefaultBuilder(args);
hostBuilder.ConfigureAppConfiguration((_, configurationBuilder) =>
{
    configurationBuilder.AddEnvironmentVariables();
});
hostBuilder.ConfigureServices((context, services) =>
{
    services.AddAppAutoMapper();
    services.AddAppConfigs(context.Configuration);
    services.AddAppServices();
    services.AddPostgreSQL(context);
    services.AddHostedService<SubscriberService>();
});
var host = hostBuilder.Build();
await using (var scope = host.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NothingKafkaDbContext>();
    if(dbContext.Database.IsNpgsql())
        await dbContext.Database.MigrateAsync();
    else if(dbContext.Database.IsInMemory())
        await dbContext.Database.EnsureCreatedAsync();
}
await host.RunAsync();