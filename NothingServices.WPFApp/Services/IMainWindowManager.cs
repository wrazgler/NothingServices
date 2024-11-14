using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Strategies;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Сервис управление отображением преставления на главном окне
/// </summary>
public interface IMainWindowManager
{
    /// <summary>
    /// Стратегия взаимодействия с клиентом NothingApi
    /// </summary>
    INothingApiClientStrategy? Strategy { get; set; }

    /// <summary>
    /// Событие изменения представления, отображаемое на главном окне
    /// </summary>
    event Action<MainWindowContentType>? OnNext;

    /// <summary>
    /// Изменить представление, отображаемое на главном окне
    /// </summary>
    /// <param name="nextType">Тип представления, отображаемого на главном окне</param>
    void Next(MainWindowContentType nextType);
}