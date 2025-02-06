using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NothingKafka.Extensions;
using NothingServices.Abstractions.Configs;
using NothingServices.Abstractions.Extensions;

namespace NothingKafka.DbContexts;

/// <summary>
/// Фабрика создания контекста базы данных
/// </summary>
internal sealed class NothingKafkaDesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<NothingKafkaDbContext>
{
    /// <summary>
    /// Создает контекст базы данных
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    /// <returns>Контекст базы данных</returns>
    public NothingKafkaDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build()
            .GetConfig<PostgresConfig>();
        var optionsBuilder = new DbContextOptionsBuilder<NothingKafkaDbContext>();
        optionsBuilder.ConfigureNpgsql(config.ConnectionString);
        return new NothingKafkaDbContext(optionsBuilder.Options);
    }
}