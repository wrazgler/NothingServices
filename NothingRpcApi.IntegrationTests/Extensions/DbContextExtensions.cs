using Microsoft.Extensions.DependencyInjection;
using NothingRpcApi.DbContexts;
using NothingRpcApi.Models;

namespace NothingWebApi.IntegrationTests.Extensions;

internal static class DbContextExtensions
{
    internal static async Task<NothingRpcApiDbContext> CreateNewDataBaseAsync(this IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<NothingRpcApiDbContext>();
        await dbContext.CreateNewDataBaseAsync();
        return dbContext;
    }
    
    internal static async Task<NothingRpcApiDbContext> CreateNewDataBaseAsync(this NothingRpcApiDbContext dbContext)
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        dbContext.ChangeTracker.Clear();
        return dbContext;
    }
    
    internal static async Task<NothingRpcApiDbContext> AddNothingModelAsync(
        this IServiceProvider serviceProvider,
        string name = "Test")
    {
        var dbContext = serviceProvider.GetRequiredService<NothingRpcApiDbContext>();
        await dbContext.AddNothingModelAsync(name);
        return dbContext;
    }

    private static async Task<NothingRpcApiDbContext> AddNothingModelAsync(
        this NothingRpcApiDbContext dbContext,
        string name = "Test")
    {
        var nothingModel = new NothingModel()
        {
            Name = name,
        };
        await dbContext.NothingModels.AddAsync(nothingModel);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();
        return dbContext;
    }
}