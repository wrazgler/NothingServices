using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки отменить
/// </summary>
/// <param name="command">Команда, исполняемая кнопкой</param>
public sealed class CancelButtonVM(ICommand command)
    : ObservableObject, IButtonVM
{
    /// <summary>
    /// Текст кнопки отменить
    /// </summary>
    public string Text { get; } = "Отмена";

    /// <summary>
    /// Текстовая подсказка кнопки отменить
    /// </summary>
    public string ToolTip { get; } = "Отменить выбранное действие и вернуться назад";

    /// <summary>
    /// Команда кнопки отменить
    /// </summary>
    public ICommand Command { get; } = command;
}