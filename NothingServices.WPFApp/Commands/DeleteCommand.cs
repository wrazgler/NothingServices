using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда удалить модель
/// </summary>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class DeleteCommand(
    IDialogService dialogService,
    IMainWindowManager mainWindowManager,
    INotificationService notificationService)
    : BaseCommand
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    /// <summary>
    /// Проверка возможности выполнить команду удалить модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not DeleteNothingModelVM deleteNothingModelVM)
            return false;

        if (deleteNothingModelVM.Id == 0)
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
            var nothingModelVM = await strategy.DeleteNothingModelAsync(
                deleteNothingModelVM,
                _cancellationTokenSource.Token);
            _mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            _dialogService.CloseDialog();
            _notificationService.Notify($"Удалено \"{nothingModelVM.Name}\"");
        }
        catch (Exception ex)
        {
            _notificationService.Notify(ex.Message);
        }
    }
}