using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NothingRpcApi.Extensions;
using NothingServices.Abstractions.Configs;
using NothingServices.Abstractions.Extensions;

namespace NothingRpcApi.DbContexts;

/// <summary>
/// Фабрика создания контекста базы данных
/// </summary>
public class NothingRpcApiDesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<NothingRpcApiDbContext>
{
    /// <summary>
    /// Создает контекст базы данных
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    /// <returns>Контекст базы данных</returns>
    public NothingRpcApiDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build()
            .GetConfig<PostgresConfig>();
        var optionsBuilder = new DbContextOptionsBuilder<NothingRpcApiDbContext>();
        optionsBuilder.ConfigureNpgsql(config.ConnectionString);
        return new NothingRpcApiDbContext(optionsBuilder.Options);
    } 
}