using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.Strategies;
using NothingServices.WPFApp.ViewModels;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда открыть представление окна списка моделей
/// </summary>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificator">Сервис отображения уведомлений в пользовательском интерфейсе</param>
/// <param name="nothingModelsListVM">Данные представления окна списка моделей</param>
public class OpenNothingModelsListCommand(
    IMainWindowManager mainWindowManager,
    INotificator notificator,
    NothingModelsListVM nothingModelsListVM) : BaseCommand
{
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificator _notificator = notificator;
    private readonly NothingModelsListVM _nothingModelsListVM = nothingModelsListVM;

    /// <summary>
    /// Проверка возможности выполнить команду открыть представление окна списка моделей
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        var valid = parameter is not INothingApiClientStrategy;
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
            _mainWindowManager.Next(_nothingModelsListVM);
        }
        catch (Exception ex)
        {
            _notificator.Notificate(ex.Message);
        }
    }
}