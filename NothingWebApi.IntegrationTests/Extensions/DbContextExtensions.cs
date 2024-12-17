using Microsoft.Extensions.DependencyInjection;
using NothingWebApi.DbContexts;
using NothingWebApi.Models;

namespace NothingWebApi.IntegrationTests.Extensions;

internal static class DbContextExtensions
{
    internal static async Task<NothingWebApiDbContext> CreateNewDataBase(this IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<NothingWebApiDbContext>();
        await dbContext.CreateNewDataBase();
        return dbContext;
    }

    private static async Task<NothingWebApiDbContext> CreateNewDataBase(this NothingWebApiDbContext dbContext)
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        dbContext.ChangeTracker.Clear();
        return dbContext;
    }

    internal static async Task<NothingWebApiDbContext> AddNothingModel(
        this IServiceProvider serviceProvider,
        string name = "Test")
    {
        var dbContext = serviceProvider.GetRequiredService<NothingWebApiDbContext>();
        await dbContext.AddNothingModel(name);
        return dbContext;
    }

    private static async Task<NothingWebApiDbContext> AddNothingModel(
        this NothingWebApiDbContext dbContext,
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