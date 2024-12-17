using Microsoft.Extensions.Configuration;

namespace NothingServices.Abstractions.Configs;

/// <summary>
/// Конфигурация подключения к базе данных Postgres
/// </summary>
public sealed class PostgresConfig
{
    private const string ConnectingStringFormat = "Host={0};Port={1};Database={2};Username={3};Password={4}";

    /// <summary>
    /// Имя хоста
    /// </summary>
    [ConfigurationKeyName("POSTGRES_HOST")]
    public required string Host { get; init; }

    /// <summary>
    /// Номер порта
    /// </summary>
    [ConfigurationKeyName("POSTGRES_PORT")]
    public required string Port { get; init; }

    /// <summary>
    /// Имя базы данных
    /// </summary>
    [ConfigurationKeyName("POSTGRES_DB")]
    public required string Database { get; init; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    [ConfigurationKeyName("POSTGRES_USER")]
    public required string User { get; init; }

    /// <summary>
    /// Пароль
    /// </summary>
    [ConfigurationKeyName("POSTGRES_PASSWORD")]
    public required string Password { get; init; }

    /// <summary>
    /// Строка подключения
    /// </summary>
    public string ConnectionString => string.Format(ConnectingStringFormat, Host, Port, Database, User, Password);
}