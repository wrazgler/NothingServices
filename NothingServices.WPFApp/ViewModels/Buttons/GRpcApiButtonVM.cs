using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки gRpc Api
/// </summary>
public class GRpcApiButtonVM : ObservableObject, IButtonVM
{
    /// <summary>
    /// Текст кнопки gRpc Api
    /// </summary>
    public string Text { get; } = "gRpc Api";

    /// <summary>
    /// Текстовая подсказка кнопки gRpc Api
    /// </summary>
    public string ToolTip { get; } = "Получить список моделей из сервиса gRpc Api";

    /// <summary>
    /// Команда кнопки gRpc Api
    /// </summary>
    public ICommand Command { get; }
}