using System.Globalization;
using System.Windows.Data;

namespace NothingServices.WPFApp.Converters;

/// <summary>
/// Конвертер высоты окна уведомлений
/// </summary>
[ValueConversion(typeof(double[]), typeof(double))]
public class NotificatorHeightConverter : IMultiValueConverter
{
    /// <summary>
    /// Перемножает входящие значения для получения высоты элемента
    /// </summary>
    /// <param name="value">Коллекция из двух значений для перемножения</param>
    /// <param name="targetType">Тип целевого объекта</param>
    /// <param name="parameter">Параметр конвертации</param>
    /// <param name="culture">Информация о специфике культуре</param>
    /// <returns>
    /// Возвращает высоту окна уведомлений
    /// </returns>
    /// <exception cref="ArgumentException">Тип элемента не соответствует конвертеру</exception>
    public object? Convert(object?[]? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        if (value is not { Length: 2 } || value[0] == null || value[1] == null)
            return Binding.DoNothing;
        var valid = TryGetValue(value[0], out var value1);
        valid &= TryGetValue(value[1], out var value2);
        var result = valid ? value1 * value2 : Binding.DoNothing;
        return result;
    }

    private static bool TryGetValue(object? value, out double result)
    {
        switch (value)
        {
            case double doubleValue:
                result = doubleValue;
                return true;
            case string stringValue:
            {
                var valid = double.TryParse(stringValue.Trim().Replace(".", ","), out result);
                return valid;
            }
            default:
                result = 0;
                return false;
        }
    }

    /// <summary>
    /// Обратное преобразование недопустимо
    /// </summary>
    /// <param name="value">Объект конвертации</param>
    /// <param name="targetTypes">Типы целевых объектов</param>
    /// <param name="parameter">Параметр конвертации</param>
    /// <param name="culture">Информация о специфике культуре</param>
    /// <exception cref="NotImplementedException">Обратное преобразование недопустимо</exception>
    public object?[] ConvertBack(object? value, Type[]? targetTypes, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException("Обратное преобразование недопустимо");
    }
}