using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NothingKafka.Models;
using Npgsql;

namespace NothingKafka.DbContexts;

/// <summary>
/// Контекст базы данных
/// </summary>
/// <param name="options">Конфигурация контекста базы данных</param>
public class NothingKafkaDbContext(DbContextOptions<NothingKafkaDbContext> options)
    : DbContext(options)
{
    /// <summary>
    /// Коллекция моделей
    /// </summary>
    public virtual DbSet<NothingModel> NothingModels { get; set; }

    /// <summary>
    /// Конфигурация моделей базы данных
    /// </summary>
    /// <param name="modelBuilder"> Конфигуратор моделей базы данных</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");
        base.OnModelCreating(modelBuilder);
        var assembly = Assembly.GetExecutingAssembly();
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }

    /// <summary>
    /// Создает экземпляр <see href="NpgsqlDataSource"/> источника данных
    /// </summary>
    /// <param name="connectionString">Строка подключения</param>
    /// <returns>Экземпляр <see href="NpgsqlDataSource"/> источника данных</returns>
    internal static NpgsqlDataSource CreateNpgsqlDataSource(string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        return dataSourceBuilder.Build();
    }
}