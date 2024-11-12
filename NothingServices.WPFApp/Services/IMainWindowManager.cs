using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Сервис управление отображением преставления на главном окне
/// </summary>
public interface IMainWindowManager
{
    /// <summary>
    /// Представление, отображаемое на главном окне
    /// </summary>
    IMainWindowContentVM? Current { get; set; }

    /// <summary>
    /// Стратегия взаимодействия с клиентом NothingApi
    /// </summary>
    INothingApiClientStrategy? Strategy { get; set; }

    /// <summary>
    /// Изменить представление, отображаемое на главном окне
    /// </summary>
    /// <param name="next">Следующее представление, отображаемое на главном окне</param>
    void Next(IMainWindowContentVM next);
}