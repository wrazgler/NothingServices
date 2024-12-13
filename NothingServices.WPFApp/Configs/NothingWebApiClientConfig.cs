using Microsoft.Extensions.Configuration;

namespace NothingServices.WPFApp.Configs;

/// <summary>
/// Конфигурация подключения к сервису NothingWebApi
/// </summary>
public class NothingWebApiClientConfig
{
    /// <summary>
    /// Адрес сервиса NothingWebApi
    /// </summary>
    [ConfigurationKeyName("NOTHING_WEB_API_URL")]
    public required string BaseUrl { get; init; }

    /// <summary>
    /// Url контроллера NothingWebApi
    /// </summary>
    public string NothingWebApiUrl => $"{BaseUrl}/NothingWebApi";
}