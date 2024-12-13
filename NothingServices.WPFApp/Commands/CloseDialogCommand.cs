using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда закрыть представление диалогового окна
/// </summary>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class CloseDialogCommand(
    IDialogService dialogService,
    INotificationService notificationService)
    : BaseCommand, ICloseDialogCommand
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly INotificationService _notificationService = notificationService;

    /// <summary>
    /// Проверка возможности выполнить команду закрыть представление диалогового окна
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
            _notificationService.Notify(ex.Message, ex.ToString());
        }
    }
}