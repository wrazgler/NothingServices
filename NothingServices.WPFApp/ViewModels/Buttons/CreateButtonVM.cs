using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки создать модель
/// </summary>
public class CreateButtonVM : ObservableObject, IButtonVM
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
    public ICommand Command { get; }
}