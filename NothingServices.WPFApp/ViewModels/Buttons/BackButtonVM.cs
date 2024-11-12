using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Commands;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки вернуться назад
/// </summary>
/// <param name="openApiSelectionVMCommand">Команда открыть представление окна списка моделей</param>
public class BackButtonVM(OpenApiSelectionVMCommand openApiSelectionVMCommand)
    : ObservableObject, IButtonVM
{
    /// <summary>
    /// Текст кнопки вернуться назад
    /// </summary>
    public string Text { get; } = "Назад";

    /// <summary>
    /// Текстовая подсказка кнопки gвернуться назад
    /// </summary>
    public string ToolTip { get; } = "Вернуться назад к выбору внешнего сервиса";

    /// <summary>
    /// Команда кнопки вернуться назад
    /// </summary>
    public ICommand Command { get; } = openApiSelectionVMCommand;
}