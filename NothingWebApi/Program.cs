using Microsoft.EntityFrameworkCore;
using NothingWebApi.DbContexts;
using NothingWebApi.Extensions;

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
    services.AddEndpointsApiExplorer();
    services.AddAppSwaggerGen();
});
hostBuilder.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.ConfigureServices((_, services) =>
    {
        services.AddAppControllers();
    });
    webBuilder.Configure((context, applicationBuilder) =>
    {
        applicationBuilder.UseAppPathBase(context.Configuration);
        applicationBuilder.UseRouting();
        applicationBuilder.UseEndpoints(endpointBuilder =>
        {
            endpointBuilder.MapControllers();
            endpointBuilder.MapSwagger();
            endpointBuilder.MapGet("/", () => "Welcome to Nothing Web API service!");
        });
        applicationBuilder.UseSwagger();
        applicationBuilder.UseAppSwaggerUI(context.Configuration);
    });
});
var host = hostBuilder.Build();
await using (var scope = host.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NothingWebApiDbContext>();
    if(dbContext.Database.IsNpgsql())
        await dbContext.Database.MigrateAsync();
    else if(dbContext.Database.IsInMemory())
        await dbContext.Database.EnsureCreatedAsync();
}
await host.RunAsync();