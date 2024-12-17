using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Strategies;

namespace NothingServices.WPFApp.ViewModels.Buttons;

/// <summary>
/// Данные представления кнопки Rest Api
/// </summary>
/// <param name="strategy">Стратегия взаимодействия с клиентом NothingWebApi</param>
/// <param name="openNothingModelsListCommand">Команда открыть представление окна списка моделей</param>
public sealed class RestApiButtonVM(
    NothingWebApiClientStrategy strategy,
    IOpenNothingModelsListCommand openNothingModelsListCommand)
    : ObservableObject, IRestApiButtonVM
{
    /// <summary>
    /// Текст кнопки Rest Api
    /// </summary>
    public string Text { get; } = "Rest Api";

    /// <summary>
    /// Текстовая подсказка кнопки Rest Api
    /// </summary>
    public string ToolTip { get; } = "Получить список моделей из сервиса Rest Api";

    /// <summary>
    /// Команда кнопки Rest Api
    /// </summary>
    public ICommand Command { get; } = openNothingModelsListCommand;

    /// <summary>
    /// Стратегия взаимодействия с клиентом NothingApi
    /// </summary>
    public INothingApiClientStrategy Strategy { get; } = strategy;
}