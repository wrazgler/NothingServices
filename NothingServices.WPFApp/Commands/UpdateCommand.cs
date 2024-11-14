using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда обновить существующую модель
/// </summary>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificator">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class UpdateCommand(
    IMainWindowManager mainWindowManager,
    INotificator notificator)
    : BaseCommand
{
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificator _notificator = notificator;
    private readonly CancellationTokenSource _cancellationTokenSource = new(100000);

    /// <summary>
    /// Проверка возможности выполнить команду обновить существующую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not UpdateNothingModelVM updateNothingModelVM)
            return false;

        if (updateNothingModelVM.Id == 0)
            return false;

        if (string.IsNullOrEmpty(updateNothingModelVM.Name))
            return false;

        return true;
    }

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override async void Execute(object? parameter)
    {
        try
        {
            var updateNothingModelVM = parameter as UpdateNothingModelVM
                ?? throw new ArgumentException(parameter?.GetType().Name);
            var strategy = _mainWindowManager.Strategy
                ?? throw new NullReferenceException(_mainWindowManager.Strategy?.GetType().Name);
            await strategy.UpdateNothingModelAsync(
                updateNothingModelVM,
                _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            _notificator.Notificate(ex.Message);
        }
    }
}