using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using NpgsqlTypes;

namespace NothingServices.Abstractions.Extensions;

/// <summary>
/// Методы расширений для типов данных
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Получить значение свойства по умолчанию аттрибута <see cref="DefaultValueAttribute"/> у свойства типа
    /// </summary>
    /// <param name="type">Объект типа</param>
    /// <param name="propertyName">Имя свойства</param>
    /// <returns>Значение свойства по умолчанию</returns>
    public static object? GetDefaultValue(this Type type, string propertyName)
    {
        var member = type.GetMember(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
            .SingleOrDefault();
        var attribute = member?.GetCustomAttribute<DefaultValueAttribute>();
        var value = attribute?.Value;
        return value;
    }

    /// <summary>
    /// Получить значение свойства по умолчанию аттрибута <see cref="DefaultValueAttribute"/> у свойства типа
    /// </summary>
    /// <param name="type">Объект типа</param>
    /// <param name="propertyName">Имя свойства</param>
    /// <returns>Значение свойства по умолчанию</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    public static T GetDefaultValue<T>(this Type type, string propertyName)
    {
        var value = type.GetDefaultValue(propertyName)
            ?? throw new ArgumentNullException(nameof(propertyName));
        return (T)value;
    }

    /// <summary>
    /// Получить описание свойства аттрибута <see cref="DescriptionAttribute"/> у свойства типа
    /// </summary>
    /// <param name="type">Объект типа</param>
    /// <param name="propertyName">Имя свойства</param>
    /// <returns>Описание свойства</returns>
    public static string GetDescription(this Type type, string propertyName)
    {
        var member = type.GetMember(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
            .SingleOrDefault();
        var attribute = member?.GetCustomAttribute<DescriptionAttribute>();
        var description = attribute?.Description ?? propertyName;
        return description;
    }

    /// <summary>
    /// Получить имя Json свойства аттрибута <see cref="JsonPropertyNameAttribute"/> у свойства типа
    /// </summary>
    /// <param name="type">Объект типа</param>
    /// <param name="propertyName">Имя свойства</param>
    /// <returns>Имя свойства</returns>
    public static string GetJsonPropertyName(this Type type, string propertyName)
    {
        var member = type.GetMember(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
            .SingleOrDefault();
        var attribute = member?.GetCustomAttribute<JsonPropertyNameAttribute>();
        var description = attribute?.Name ?? propertyName;
        return description;
    }

    /// <summary>
    /// Получить имя таблицы из аттрибута <see cref="TableAttribute"/> у типа данных
    /// </summary>
    /// <param name="type">Объект типа</param>
    /// <returns>Имя таблицы</returns>
    public static string GetTableName(this Type type)
    {
        var attribute = type.GetCustomAttribute<TableAttribute>();
        var tableName = attribute?.Name ?? type.ToString();
        return tableName;
    }

    /// <summary>
    /// Получить имя для PostgreSQL аттрибута <see cref="PgNameAttribute"/> у типа данных
    /// </summary>
    /// <param name="type">Объект типа</param>
    /// <returns>Имя для PostgreSQL</returns>
    public static string GetPgName(this Type type)
    {
        var attribute = type.GetCustomAttribute<PgNameAttribute>();
        var pgName = attribute?.PgName ?? type.ToString();
        return pgName;
    }
}