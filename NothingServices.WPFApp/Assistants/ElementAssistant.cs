using System.Windows;
using System.Windows.Media;

namespace NothingServices.WPFApp.Assistants;

/// <summary>
/// Дополнительные свойства для стилизации элементов
/// </summary>
public static class ElementAssistant
{
    /// <summary>
    /// Регистрация свойства радиус углов
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(ElementAssistant),
        new PropertyMetadata(new CornerRadius(2.0)));

    /// <summary>
    /// Получение радиуса элемента интерфейса
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <returns>Радиус элемента</returns>
    public static CornerRadius GetCornerRadius(DependencyObject element)
        => (CornerRadius)element.GetValue(CornerRadiusProperty);

    /// <summary>
    /// Установка радиуса элемента интерфейса
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <param name="value">Значение радиуса</param>
    public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        => element.SetValue(CornerRadiusProperty, value);

    /// <summary>
    /// Регистрация свойства цвет заднего фона при наведении мыши
    /// </summary>
    public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.RegisterAttached(
        "MouseOverBackground",
        typeof(SolidColorBrush),
        typeof(ElementAssistant));

    /// <summary>
    /// Получение цвета заднего фона при наведении мыши
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <returns>Цвет заднего фона при наведении мыши</returns>
    public static SolidColorBrush GetMouseOverBackground(DependencyObject element)
        => (SolidColorBrush)element.GetValue(MouseOverBackgroundProperty);

    /// <summary>
    /// Установка цвета заднего фона при наведении мыши
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <param name="value">Цвет заднего фона при наведении мыши</param>
    public static void SetMouseOverBackground(DependencyObject element, SolidColorBrush value)
        => element.SetValue(MouseOverBackgroundProperty, value);

    /// <summary>
    /// Регистрация свойства цвет переднего фона при наведении мыши
    /// </summary>
    public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.RegisterAttached(
        "MouseOverForeground",
        typeof(SolidColorBrush),
        typeof(ElementAssistant));

    /// <summary>
    /// Получение цвета переднего фона при наведении мыши
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <returns>Цвет переднего фона при наведении мыши</returns>
    public static SolidColorBrush GetMouseOverForeground(DependencyObject element)
        => (SolidColorBrush)element.GetValue(MouseOverForegroundProperty);

    /// <summary>
    /// Установка цвета переднего фона при наведении мыши
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <param name="value">Цвет переднего фона при наведении мыши</param>
    public static void SetMouseOverForeground(DependencyObject element, SolidColorBrush value)
        => element.SetValue(MouseOverForegroundProperty, value);

    /// <summary>
    /// Регистрация свойства цвет границы элемента при наведении мыши
    /// </summary>
    public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.RegisterAttached(
        "MouseOverBorderBrush",
        typeof(SolidColorBrush),
        typeof(ElementAssistant));

    /// <summary>
    /// Получение цвета границы элемента при наведении мыши
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <returns>Цвет границы элемента при наведении мыши</returns>
    public static SolidColorBrush GetMouseOverBorderBrush(DependencyObject element)
        => (SolidColorBrush)element.GetValue(MouseOverBorderBrushProperty);

    /// <summary>
    /// Установка цвета границы элемента при наведении мыши
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <param name="value">Цвет границы элемента при наведении мыши</param>
    public static void SetMouseOverBorderBrush(DependencyObject element, SolidColorBrush value)
        => element.SetValue(MouseOverBorderBrushProperty, value);
}