using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Commands;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки создать модель
/// </summary>
/// <param name="createCommand">Команда создать новую модель</param>
public class CreateButtonVM(CreateCommand createCommand) : ObservableObject, IButtonVM
{
    /// <summary>
    /// Текст кнопки создать модель
    /// </summary>
    public string Text { get; } = "Создать модель";

    /// <summary>
    /// Текстовая подсказка кнопки создать модель
    /// </summary>
    public string ToolTip { get; } = "Создать новую модель с указанным именем";

    /// <summary>
    /// Команда кнопки создать модель
    /// </summary>
    public ICommand Command { get; } = createCommand;
}