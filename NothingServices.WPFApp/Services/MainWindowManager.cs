using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Сервис управление отображением преставления на главном окне
/// </summary>
public class MainWindowManager : IMainWindowManager
{
    /// <summary>
    /// Представление, отображаемое на главном окне
    /// </summary>
    public IMainWindowContentVM? Current { get; set; }

    /// <summary>
    /// Стратегия взаимодействия с клиентом NothingApi
    /// </summary>
    public INothingApiClientStrategy? Strategy { get; set; }

    /// <summary>
    /// Изменить представление, отображаемое на главном окне
    /// </summary>
    /// <param name="next">Следующее представление, отображаемое на главном окне</param>
    public void Next(IMainWindowContentVM next)
    {
        if(Current != null)
            Current.Visible = false;
        next.Visible = true;
        Current = next;
    }
}