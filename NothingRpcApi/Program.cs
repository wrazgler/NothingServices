using Microsoft.EntityFrameworkCore;
using NothingRpcApi.DbContexts;
using NothingRpcApi.Extensions;
using NothingRpcApi.Services;

var hostBuilder = Host.CreateDefaultBuilder(args);
hostBuilder.ConfigureAppConfiguration((_, configurationBuilder) =>
{
    configurationBuilder.AddEnvironmentVariables();
});
hostBuilder.ConfigureServices((context, services) =>
{
    services.AddAppAutoMapper();
    services.AddAppConfigs(context.Configuration);
    services.AddPostgreSQL(context);
    services.AddGrpcSwagger();
    services.AddAppSwaggerGen();
});
hostBuilder.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.ConfigureServices((_, services) =>
    {
        services.AddGrpc();
    });
    webBuilder.Configure((context, applicationBuilder) =>
    {
        applicationBuilder.UseAppPathBase(context.Configuration);
        applicationBuilder.UseRouting();
        applicationBuilder.UseEndpoints(endpointBuilder =>
        {
            endpointBuilder.MapGrpcService<NothingService>();
            endpointBuilder.MapGet("/", () => "Welcome to Nothing gRPC API service!");
        });
        applicationBuilder.UseSwagger();
        applicationBuilder.UseAppSwaggerUI(context.Configuration);
    });
    webBuilder.ConfigureKestrel();
});
var host = hostBuilder.Build();
await using (var scope = host.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NothingRpcApiDbContext>();
    if(dbContext.Database.IsNpgsql())
        await dbContext.Database.MigrateAsync();
    else if(dbContext.Database.IsInMemory())
        await dbContext.Database.EnsureCreatedAsync();
}
await host.RunAsync();