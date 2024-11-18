using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
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

    /// <summary>
    /// Регистрация свойства цвета заднего фона при выделении элемента
    /// </summary>
    public static readonly DependencyProperty SelectedBackgroundProperty = DependencyProperty.RegisterAttached(
        "SelectedBackground",
        typeof(SolidColorBrush),
        typeof(ElementAssistant));

    /// <summary>
    /// Получение цвета заднего фона при выделении элемента
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <returns>Цвет заднего фона при выделении элемента</returns>
    public static SolidColorBrush GetSelectedBackground(DependencyObject element)
        => (SolidColorBrush)element.GetValue(SelectedBackgroundProperty);

    /// <summary>
    /// Установка цвета заднего фона при выделении элемента
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <param name="value">Цвет заднего фона при выделении элемента</param>
    public static void SetSelectedBackground(DependencyObject element, SolidColorBrush value)
        => element.SetValue(SelectedBackgroundProperty, value);

    /// <summary>
    /// Регистрация свойства цвета границы элемента при выделении элемента
    /// </summary>
    public static readonly DependencyProperty SelectedBorderBrushProperty = DependencyProperty.RegisterAttached(
        "SelectedBorderBrush",
        typeof(SolidColorBrush),
        typeof(ElementAssistant));

    /// <summary>
    /// Получение цвета границы элемента при выделении элемента
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <returns>Цвет границы элемента при выделении элемента</returns>
    public static SolidColorBrush GetSelectedBorderBrush(DependencyObject element)
        => (SolidColorBrush)element.GetValue(SelectedBorderBrushProperty);

    /// <summary>
    /// Установка цвета границы элемента при выделении элемента
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <param name="value">Цвет границы элемента при выделении элемента</param>
    public static void SetSelectedBorderBrush(DependencyObject element, SolidColorBrush value)
        => element.SetValue(SelectedBorderBrushProperty, value);

    /// <summary>
    /// Регистрация свойства цвета переднего фона при выделении элемента
    /// </summary>
    public static readonly DependencyProperty SelectedForegroundProperty = DependencyProperty.RegisterAttached(
        "SelectedForeground",
        typeof(SolidColorBrush),
        typeof(ElementAssistant));

    /// <summary>
    /// Получение цвета переднего фона при выделении элемента
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <returns>Цвет переднего фона при выделении элемента</returns>
    public static SolidColorBrush GetSelectedForeground(DependencyObject element)
        => (SolidColorBrush)element.GetValue(SelectedForegroundProperty);

    /// <summary>
    /// Установка цвета переднего фона при выделении элемента
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <param name="value">Цвет переднего фона при выделении элемента</param>
    public static void SetSelectedForeground(DependencyObject element, SolidColorBrush value)
        => element.SetValue(SelectedForegroundProperty, value);

    /// <summary>
    /// Регистрация свойства текста в элементе
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
        "Text",
        typeof(string),
        typeof(ElementAssistant));

    /// <summary>
    /// Получение текста в элементе
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <returns>Текст в элементе</returns>
    public static string GetText(DependencyObject element)
        => (string)element.GetValue(TextProperty);

    /// <summary>
    /// Установка текста в элементе
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <param name="value">Текст в элементе</param>
    public static void SetText(DependencyObject element, string value)
        => element.SetValue(TextProperty, value);

    /// <summary>
    /// Регистрация свойства стиля расположения текста элемента
    /// </summary>
    public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.RegisterAttached(
        "TextAlignment",
        typeof(TextAlignment),
        typeof(ElementAssistant));

    /// <summary>
    /// Получение стиля расположения текста элемента
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <returns>Стиль расположения текста элемента</returns>
    public static TextAlignment GetTextAlignment(DependencyObject element)
        => (TextAlignment)element.GetValue(TextAlignmentProperty);

    /// <summary>
    /// Установка стиля расположения текста элемента
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <param name="value">Стиль расположения текста элемента</param>
    public static void SetTextAlignment(DependencyObject element, TextAlignment value)
        => element.SetValue(TextAlignmentProperty, value);

    /// <summary>
    /// Регистрация свойства стиля переноса текста элемента
    /// </summary>
    public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.RegisterAttached(
        "TextWrapping",
        typeof(TextWrapping),
        typeof(ElementAssistant));

    /// <summary>
    /// Получение стиля переноса текста элемента
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <returns>Стиль переноса текста элемента</returns>
    public static TextWrapping GetTextWrapping(DependencyObject element)
        => (TextWrapping)element.GetValue(TextWrappingProperty);

    /// <summary>
    /// Установка стиля переноса текста элемента
    /// </summary>
    /// <param name="element">Элемент интерфейса</param>
    /// <param name="value">Стиль переноса текста элемента</param>
    public static void SetTextWrapping(DependencyObject element, TextWrapping value)
        => element.SetValue(TextWrappingProperty, value);
}