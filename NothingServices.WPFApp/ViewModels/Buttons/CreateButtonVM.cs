using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Commands;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки создать модель
/// </summary>
/// <param name="command">Команда, исполняемая кнопкой</param>
public class CreateButtonVM(ICommand command)
    : ObservableObject, IButtonVM
{
    /// <summary>
    /// Текст кнопки создать модель
    /// </summary>
    public string Text { get; } = "Создать";

    /// <summary>
    /// Текстовая подсказка кнопки создать модель
    /// </summary>
    public string ToolTip { get; } = "Создать новую модель с указанным именем";

    /// <summary>
    /// Команда кнопки создать модель
    /// </summary>
    public ICommand Command { get; } = command;
}