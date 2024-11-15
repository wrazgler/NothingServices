using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда создать новую модель
/// </summary>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificator">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class CreateCommand(
    IDialogService dialogService,
    IMainWindowManager mainWindowManager,
    INotificator notificator)
    : BaseCommand
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificator _notificator = notificator;
    private readonly CancellationTokenSource _cancellationTokenSource = new(100000);

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
             await strategy.CreateNothingModelAsync(
                createNothingModelVM,
                _cancellationTokenSource.Token);
             _mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
             _dialogService.CloseDialog();
        }
        catch (Exception ex)
        {
            _notificator.Notificate(ex.Message);
        }
    }
}