using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Commands;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки обновить модель
/// </summary>
/// <param name="updateCommand">Команда обновить существующую модель</param>
public class UpdateButtonVM(UpdateCommand updateCommand) : ObservableObject, IButtonVM
{
    /// <summary>
    /// Текст кнопки обновить модель
    /// </summary>
    public string Text { get; } = "Обновить модель";

    /// <summary>
    /// Текстовая подсказка кнопки обновить модель
    /// </summary>
    public string ToolTip { get; } = "Обновить существующую модель с указанным идентификатором";

    /// <summary>
    /// Команда кнопки обновить модель
    /// </summary>
    public ICommand Command { get; } = updateCommand;
}