namespace NothingServices.Abstractions.Exceptions;

/// <summary>
/// Ошибка получения конфигурации
/// </summary>
public sealed class ConfigurationNullException<TConfig> : Exception
    where TConfig : class
{
    private const string MessageFormat = "Конфигурация {0} не обнаружена";

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public override string Message { get; } = string.Format(MessageFormat, typeof(TConfig).Name);
}