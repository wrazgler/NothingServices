using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки удалить модель
/// </summary>
public class DeleteButtonVM : ObservableObject, IButtonVM
{
    /// <summary>
    /// Текст кнопки удалить модель
    /// </summary>
    public string Text { get; } = "Удалить модель";

    /// <summary>
    /// Текстовая подсказка кнопки удалить модель
    /// </summary>
    public string ToolTip { get; } = "Удалить модель с указанным идентификатором";

    /// <summary>
    /// Команда кнопки удалить модель
    /// </summary>
    public ICommand Command { get; }
}