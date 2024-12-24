using NothingServices.Abstractions.Extensions;

namespace NothingServices.Abstractions.Exceptions;

/// <summary>
/// Ошибка отсутствия значения у требуемого поля
/// </summary>
/// <param name="propertyName">Имя поля</param>
public sealed class PropertyRequiredException<T>(string propertyName)
    : Exception where T : class
{
    private const string MessageFormat = "Поле {0} не может быть пустым";

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public override string Message => string.Format(MessageFormat, typeof(T).GetDescription(propertyName));
}