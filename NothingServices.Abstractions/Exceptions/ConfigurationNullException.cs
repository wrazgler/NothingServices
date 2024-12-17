namespace NothingServices.Abstractions.Exceptions;

/// <summary>
/// Ошибка получения конфигурации
/// </summary>
public sealed class ConfigurationNullException<TConfig> : ArgumentNullException
    where TConfig : class
{
    private const string MessageFormat = "Конфигурация {0} не обнаружена";

    /// <summary>
    /// Создать исключение отсутствия конфигурации
    /// </summary>
    public ConfigurationNullException()
        : base(string.Format(MessageFormat, typeof(TConfig).Name))
    {
    }
}