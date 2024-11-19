using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда открыть представление окна списка моделей
/// </summary>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class OpenNothingModelsListCommand(
    IMainWindowManager mainWindowManager,
    INotificationService notificationService)
    : BaseCommand
{
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificationService _notificationService = notificationService;

    /// <summary>
    /// Проверка возможности выполнить команду открыть представление окна списка моделей
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        var valid = parameter is INothingApiClientStrategy;
        return valid;
    }

    /// <summary>
    /// Открыть представление окна списка моделей
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override void Execute(object? parameter)
    {
        try
        {
            var strategy = parameter as INothingApiClientStrategy
                ?? throw new ArgumentException(parameter?.GetType().Name);
            _mainWindowManager.Strategy = strategy;
            _mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
        }
        catch (Exception ex)
        {
            _mainWindowManager.Strategy = null;
            _mainWindowManager.Next(MainWindowContentType.ApiSelectionVM);
            _notificationService.Notify(ex.Message);
        }
    }
}