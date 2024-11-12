using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки Rest Api
/// </summary>
public class RestApiButtonVM : ObservableObject, IButtonVM
{
    /// <summary>
    /// Текст кнопки Rest Api
    /// </summary>
    public string Text { get; } = "Rest Api";

    /// <summary>
    /// Текстовая подсказка кнопки Rest Api
    /// </summary>
    public string ToolTip { get; } = "Получить список моделей из сервиса Rest Api";

    /// <summary>
    /// Команда кнопки Rest Api
    /// </summary>
    public ICommand Command { get; }
}