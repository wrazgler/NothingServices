using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда создать новую модель
/// </summary>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificator">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class CreateCommand(
    IMainWindowManager mainWindowManager,
    INotificator notificator)
    : BaseCommand
{
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificator _notificator = notificator;
    private readonly CancellationTokenSource _cancellationTokenSource = new(10000);

    /// <summary>
    /// Проверка возможности выполнить команду создать новую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not CreateNothingModelVM createNothingModelVM)
            return false;

        if (string.IsNullOrEmpty(createNothingModelVM.Name))
            return false;

        return true;
    }

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override async void Execute(object? parameter)
    {
        try
        {
            var createNothingModelVM = parameter as CreateNothingModelVM
                 ?? throw new ArgumentException(parameter?.GetType().Name);
            var strategy = _mainWindowManager.Strategy
                ?? throw new NullReferenceException(_mainWindowManager.Strategy?.GetType().Name);
            var nothingModelVM = await strategy.CreateNothingModelAsync(
                createNothingModelVM,
                _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            _notificator.Notificate(ex.Message);
        }
    }
}