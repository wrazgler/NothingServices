using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NothingServices.Abstractions.Configs;
using NothingServices.Abstractions.Extensions;
using NothingWebApi.Extensions;

namespace NothingWebApi.DbContexts;

/// <summary>
/// Фабрика создания контекста базы данных
/// </summary>
internal sealed class NothingWebApiDesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<NothingWebApiDbContext>
{
    /// <summary>
    /// Создает контекст базы данных
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    /// <returns>Контекст базы данных</returns>
    public NothingWebApiDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build()
            .GetConfig<PostgresConfig>();
        var optionsBuilder = new DbContextOptionsBuilder<NothingWebApiDbContext>();
        optionsBuilder.ConfigureNpgsql(config.ConnectionString);
        return new NothingWebApiDbContext(optionsBuilder.Options);
    }
}