using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда закрыть представление диалогового окна
/// </summary>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="notificator">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class CloseDialogCommand(
    IDialogService dialogService,
    INotificator notificator)
    : BaseCommand
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly INotificator _notificator = notificator;

    /// <summary>
    /// Проверка возможности выполнить команду закрыть представление диалогового окна
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    /// <summary>
    /// Закрыть представление диалогового окна
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override void Execute(object? parameter)
    {
        try
        {
            _dialogService.CloseDialog();
        }
        catch (Exception ex)
        {
            _notificator.Notificate(ex.Message);
        }
    }
}