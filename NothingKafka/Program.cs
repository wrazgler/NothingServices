using Microsoft.EntityFrameworkCore;
using NothingKafka.DbContexts;
using NothingKafka.Extensions;

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