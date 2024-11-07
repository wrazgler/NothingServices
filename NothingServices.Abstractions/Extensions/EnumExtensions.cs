using System.ComponentModel;
using System.Reflection;
using NpgsqlTypes;

namespace NothingServices.Abstractions.Extensions;

/// <summary>
/// Методы расширений для <see cref="Enum"/>
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Получить описание из аттрибута <see cref="DescriptionAttribute"/> у значения <see cref="Enum"/>
    /// </summary>
    /// <param name="value">Значение <see cref="Enum"/></param>
    /// <returns>Описание свойства</returns>
    public static string GetDescription(this Enum value)
    {
        var stringValue = value.ToString();
        var fieldInfo = value.GetType().GetField(stringValue!);
        var attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>(true);
        return attribute?.Description ?? stringValue;
    }

    /// <summary>
    /// Получить имя для PostgreSQL аттрибута <see cref="PgNameAttribute"/> у значения <see cref="Enum"/>
    /// </summary>
    /// <param name="value">Значение <see cref="Enum"/></param>
    /// <returns>Имя для PostgreSQL</returns>
    public static string GetPgName(this Enum value)
    {
        var stringValue = value.ToString();
        var fieldInfo = value.GetType().GetField(stringValue!);
        var attribute = fieldInfo?.GetCustomAttribute<PgNameAttribute>(true);
        var pgName = attribute?.PgName ?? stringValue;
        return pgName;
    }
}