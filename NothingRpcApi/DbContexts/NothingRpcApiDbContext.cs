using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NothingRpcApi.Models;
using Npgsql;

namespace NothingRpcApi.DbContexts;

/// <summary>
/// Контекст базы данных
/// </summary>
/// <param name="options">Конфигурация контекста базы данных</param>
public class NothingRpcApiDbContext(DbContextOptions<NothingRpcApiDbContext> options) 
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