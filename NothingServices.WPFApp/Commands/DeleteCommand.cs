using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда удалить модель
/// </summary>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificator">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class DeleteCommand(
    IMainWindowManager mainWindowManager,
    INotificator notificator)
    : BaseCommand
{
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificator _notificator = notificator;
    private readonly CancellationTokenSource _cancellationTokenSource = new(100000);

    /// <summary>
    /// Проверка возможности выполнить команду удалить модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not NothingModelVM nothingModelVM)
            return false;

        if (nothingModelVM.Id == 0)
            return false;

        return true;
    }

    /// <summary>
    /// Удалить модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override async void Execute(object? parameter)
    {
        try
        {
            var deleteNothingModelVM = parameter as DeleteNothingModelVM
                ?? throw new ArgumentException(parameter?.GetType().Name);
            var strategy = _mainWindowManager.Strategy
                ?? throw new NullReferenceException(_mainWindowManager.Strategy?.GetType().Name);
            await strategy.DeleteNothingModelAsync(
                deleteNothingModelVM,
                _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            _notificator.Notificate(ex.Message);
        }
    }
}