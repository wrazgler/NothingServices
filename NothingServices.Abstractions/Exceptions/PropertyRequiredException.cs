using System.ComponentModel.DataAnnotations;
using NothingServices.Abstractions.Extensions;

namespace NothingServices.Abstractions.Exceptions;

/// <summary>
/// Ошибка отсутствия значения у требуемого поля
/// </summary>
public sealed class PropertyRequiredException : ValidationException
{
    private const string MessageFormat = "Поле {0} не может быть пустым";

    /// <summary>
    /// Создать исключение отсутствия значения у требуемого поля
    /// </summary>
    /// <param name="type">Тип объекта</param>
    /// <param name="propertyName">Имя поля</param>
    public PropertyRequiredException(Type type, string propertyName)
        : base(string.Format(MessageFormat, type.GetDescription(propertyName)))
    {
    }
}