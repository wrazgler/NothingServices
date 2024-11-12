using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Strategies;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки gRpc Api
/// </summary>
/// <param name="strategy">Стратегия взаимодействия с клиентом NothingRpcApi</param>
/// <param name="openNothingModelsListCommand">Команда открыть представление окна списка моделей</param>
public class GRpcApiButtonVM(
    NothingRpcApiClientStrategy strategy,
    OpenNothingModelsListCommand openNothingModelsListCommand)
    : ObservableObject, IButtonVM
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
    public ICommand Command { get; } = openNothingModelsListCommand;

    /// <summary>
    /// Стратегия взаимодействия с клиентом NothingApi
    /// </summary>
    public INothingApiClientStrategy Strategy { get; } = strategy;
}