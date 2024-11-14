using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда открыть представление окна выбора внешнего сервиса
/// </summary>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificator">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class OpenApiSelectionCommand(
    IMainWindowManager mainWindowManager,
    INotificator notificator)
    : BaseCommand
{
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificator _notificator = notificator;

    /// <summary>
    /// Проверка возможности выполнить команду открыть представление окна выбора внешнего сервиса
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    /// <summary>
    /// Открыть представление окна выбора внешнего сервиса
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override void Execute(object? parameter)
    {
        try
        {
            _mainWindowManager.Strategy = null;
            _mainWindowManager.Next(MainWindowContentType.ApiSelectionVM);
        }
        catch (Exception ex)
        {
            _notificator.Notificate(ex.Message);
        }
    }
}