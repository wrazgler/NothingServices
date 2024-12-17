using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда открыть представление окна выбора внешнего сервиса
/// </summary>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public sealed class OpenApiSelectionCommand(
    IMainWindowManager mainWindowManager,
    INotificationService notificationService)
    : BaseCommand
{
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificationService _notificationService = notificationService;

    /// <summary>
    /// Проверка возможности выполнить команду открыть представление окна выбора внешнего сервиса
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    /// <returns>
    /// Возвращает <see langword="true"/>, если можно выполнить команду и <see langword="true"/>, если нельзя
    /// </returns>
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
            _notificationService.Notify(ex.Message, ex.ToString());
        }
    }
}