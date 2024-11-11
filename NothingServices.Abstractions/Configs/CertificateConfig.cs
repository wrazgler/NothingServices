using Microsoft.Extensions.Configuration;

namespace NothingServices.Abstractions.Configs;

/// <summary>
/// Конфигурация сертификата https
/// </summary>
public class CertificateConfig
{
    /// <summary>
    /// Путь к сертификату https
    /// </summary>
    [ConfigurationKeyName("NOTHING_CERTIFICATE_FILE_NAME")]
    public required string FileName { get; init; }

    /// <summary>
    /// Пароль сертификата https
    /// </summary>
    [ConfigurationKeyName("NOTHING_CERTIFICATE_PASSWORD")]
    public required string Password { get; init; }
}