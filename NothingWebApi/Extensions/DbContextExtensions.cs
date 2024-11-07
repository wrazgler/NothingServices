using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NothingServices.Abstractions.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;
using NothingWebApi.DbContexts;

namespace NothingWebApi.Extensions;

/// <summary>
/// Методы расширений для работы с EntityFramework
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Добавить базу данных PostgreSQL в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="context">Контекст приложения</param>
    /// <returns>Коллекция сервисов с добавленной базой данных PostgreSQL</returns>
    /// <exception cref="ConfigurationNullException{TConfig}">
    /// Ошибка, возникшая при получении конфигурации.
    /// </exception>
    internal static IServiceCollection AddPostgreSQL(
        this IServiceCollection services,
        HostBuilderContext context)
    {
        var config = context.Configuration.GetConfig<PostgresConfig>();
        services.AddDbContextFactory<NothingWebApiDbContext>((_, optionsBuilder) =>
        {
            optionsBuilder.ConfigureNpgsql(config.ConnectionString);
            if (context.HostingEnvironment.IsDevelopment())
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }
        });
        return services;
    }

    /// <summary>
    /// Сконфигурировать базу данных PostgreSQL
    /// </summary>
    /// <param name="optionsBuilder">Конфигуратор базы данных</param>
    /// <param name="connectionString">Строка подключения</param>
    internal static void ConfigureNpgsql(
        this DbContextOptionsBuilder optionsBuilder,
        string connectionString)
    {
        var dataSource = NothingWebApiDbContext.CreateNpgsqlDataSource(connectionString);
        optionsBuilder.UseNpgsql(dataSource, dbContextOptionsBuilder =>
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            dbContextOptionsBuilder.MigrationsAssembly(assemblyName);
            dbContextOptionsBuilder.MigrationsHistoryTable("__ef_migrations");
        });
    }

    /// <summary>
    /// Добавить базу данных в оперативной памяти в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="context">Контекст приложения</param>
    /// <returns>Коллекция сервисов с добавленной базой данных  в оперативной памяти</returns>
    /// <exception cref="ConfigurationNullException{TConfig}">
    /// Ошибка, возникшая при получении конфигурации.
    /// </exception>
    public static IServiceCollection AddInMemoryDatabase(
        this IServiceCollection services,
        HostBuilderContext context)
    {
        var config = context.Configuration.GetConfig<PostgresConfig>();
        services.AddInMemoryDatabase(config.Database);
        return services;
    }

    /// <summary>
    /// Добавить базу данных в оперативной памяти в коллекцию сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="dbName">Имя базы данных</param>
    /// <returns>Коллекция сервисов с добавленной базой данных  в оперативной памяти</returns>
    public static IServiceCollection AddInMemoryDatabase(
        this IServiceCollection services,
        string dbName)
    {
        services.AddDbContext<NothingWebApiDbContext>(builder =>
        {
            builder.UseInMemoryDatabase(dbName);
            builder.ConfigureWarnings(warnings =>
            {
                warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning);
            });
        });
        return services;
    }
}