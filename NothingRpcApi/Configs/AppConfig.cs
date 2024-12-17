namespace NothingRpcApi.Configs;

/// <summary>
/// Конфигурация приложения
/// </summary>
public sealed class AppConfig
{
    /// <summary>
    /// Базовый адрес Url приложения
    /// </summary>
    [ConfigurationKeyName("PATH_BASE")]
    public string? PathBase { get; init; }
}