using System.Globalization;
using System.Windows.Data;

namespace NothingServices.WPFApp.Converters;

/// <summary>
/// Конвертер проверки содержимого текста для элемента
/// </summary>
[ValueConversion(typeof(string), typeof(bool))]
public class StringHasValueConverter : IValueConverter
{
    /// <summary>
    /// Проверяет задан ли текст для элемента
    /// </summary>
    /// <param name="value">Строка</param>
    /// <param name="targetType">Тип целевого объекта</param>
    /// <param name="parameter">Параметр конвертации</param>
    /// <param name="culture">Информация о специфике культуре</param>
    /// <returns>
    /// Возвращает <see langword="false"/>, если строка является пустой и
    /// <see langword="true"/>, когда строка имеет значение
    /// </returns>
    /// <exception cref="ArgumentException">Тип элемента не соответствует конвертеру</exception>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string stringValue)
            throw new ArgumentException(value?.GetType().Name);
        var hasValue = string.IsNullOrEmpty(stringValue);
        return hasValue;
    }

    /// <summary>
    /// Обратное преобразование недопустимо
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType">Тип целевого объекта</param>
    /// <param name="parameter">Параметр конвертации</param>
    /// <param name="culture">Информация о специфике культуре</param>
    /// <exception cref="NotImplementedException">Обратное преобразование недопустимо</exception>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException("Обратное преобразование недопустимо");
    }
}