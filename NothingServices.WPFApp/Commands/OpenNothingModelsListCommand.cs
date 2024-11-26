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
    /// <returns>
    /// Возвращает <see langword="true"/>, если можно выполнить команду и <see langword="true"/>, если нельзя
    /// </returns>
    public override bool CanExecute(object? parameter)
    {
        if(parameter is not INothingApiClientStrategy)
            return false;

        return true;
    }

    /// <summary>
    /// Открыть представление окна списка моделей
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    /// <exception cref="ArgumentException">
    /// Неверный тип входного параметра
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Параметр ссылается на <see langword="null"/>
    /// </exception>
    public override void Execute(object? parameter)
    {
        try
        {
            if(parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            var strategy = parameter as INothingApiClientStrategy
                ?? throw new ArgumentException($"Некорректный тип параметра команды: {parameter.GetType().Name}");
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