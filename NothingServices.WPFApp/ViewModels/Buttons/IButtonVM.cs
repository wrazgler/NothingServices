using System.Windows.Input;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки
/// </summary>
public interface IButtonVM
{
    /// <summary>
    /// Текст кнопки
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Текстовая подсказка кнопки
    /// </summary>
    public string ToolTip { get; }

    /// <summary>
    /// Команда кнопки
    /// </summary>
    public ICommand Command { get; }
}