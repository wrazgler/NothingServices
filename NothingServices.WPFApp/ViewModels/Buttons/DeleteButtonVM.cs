using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки удалить модель
/// </summary>
/// <param name="command">Команда, исполняемая кнопкой</param>
public class DeleteButtonVM(ICommand command) : ObservableObject, IButtonVM
{
    /// <summary>
    /// Текст кнопки удалить модель
    /// </summary>
    public string Text { get; } = "Удалить";

    /// <summary>
    /// Текстовая подсказка кнопки удалить модель
    /// </summary>
    public string ToolTip { get; } = "Удалить модель с указанным идентификатором";

    /// <summary>
    /// Команда кнопки удалить модель
    /// </summary>
    public ICommand Command { get; } = command;
}