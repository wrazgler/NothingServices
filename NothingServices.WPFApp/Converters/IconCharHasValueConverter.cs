using System.Globalization;
using System.Windows.Data;
using FontAwesome.Sharp;

namespace NothingServices.WPFApp.Converters;

/// <summary>
/// Конвертер проверки содержимого изображения для элемента
/// </summary>
[ValueConversion(typeof(IconChar), typeof(bool))]
public class IconCharHasValueConverter : IValueConverter
{
    /// <summary>
    /// Проверяет задано ли изображение для элемента
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType">Тип целевого объекта</param>
    /// <param name="parameter">Параметр конвертации</param>
    /// <param name="culture">Информация о специфике культуре</param>
    /// <returns>
    /// Возвращает <see langword="false"/>, если изображение не задано и
    /// <see langword="true"/>, когда изображение задано
    /// </returns>
    /// <exception cref="ArgumentException">Тип элемента не соответствует конвертеру</exception>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not IconChar iconChar)
            throw new ArgumentException(value?.GetType().Name);
        var hasValue = iconChar != IconChar.None;
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