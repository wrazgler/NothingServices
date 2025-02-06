using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NothingKafka.DbContexts;
using NothingServices.Abstractions.Configs;
using NothingServices.Abstractions.Exceptions;
using NothingServices.Abstractions.Extensions;

namespace NothingKafka.Extensions;

/// <summary>
/// Методы расширений для работы с EntityFramework
/// </summary>
internal static class DbContextExtensions
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
        services.AddDbContextFactory<NothingKafkaDbContext>((_, optionsBuilder) =>
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
        var dataSource = NothingKafkaDbContext.CreateNpgsqlDataSource(connectionString);
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
    /// <returns>Коллекция сервисов с добавленной базой данных в оперативной памяти</returns>
    /// <exception cref="ConfigurationNullException{TConfig}">
    /// Ошибка, возникшая при получении конфигурации.
    /// </exception>
    internal static IServiceCollection AddInMemoryDatabase(
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
    /// <returns>Коллекция сервисов с добавленной базой данных в оперативной памяти</returns>
    internal static IServiceCollection AddInMemoryDatabase(
        this IServiceCollection services,
        string dbName)
    {
        services.AddDbContext<NothingKafkaDbContext>(builder =>
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