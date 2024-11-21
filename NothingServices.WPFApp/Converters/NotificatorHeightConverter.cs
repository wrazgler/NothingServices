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
        if (value == null || value.Length != 2 || value[0] == null || value[1] == null)
            return Binding.DoNothing;

        var valid = double.TryParse(value[0]?.ToString()?.Replace(".", ","), out var value1);
        valid &= double.TryParse(value[1]?.ToString()?.Replace(".", ","), out var value2);
        var result = valid ? value1 * value2 : Binding.DoNothing;
        return result;
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