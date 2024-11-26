using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда открыть представление окна создания новой модели
/// </summary>
/// <param name="createNothingModelView">Представление создать модель</param>
/// <param name="createNothingModelVMFactory">Фабрика создания объекта данных представления окна создать модель</param>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class OpenCreateNothingModelCommand(
    ICreateNothingModelVMFactory createNothingModelVMFactory,
    IDialogService dialogService,
    INotificationService notificationService,
    ICreateNothingModelView createNothingModelView)
    : BaseCommand
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly ICreateNothingModelView _createNothingModelView = createNothingModelView;
    private readonly ICreateNothingModelVMFactory _createNothingModelVMFactory = createNothingModelVMFactory;

    /// <summary>
    /// Проверка возможности выполнить команду открыть представление окна создания новой модели
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    /// <summary>
    /// Открыть представление окна создания новой модели
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override void Execute(object? parameter)
    {
        try
        {
            var createNothingModelVM = _createNothingModelVMFactory.Create();
            _dialogService.OpenDialog(createNothingModelVM, _createNothingModelView);
        }
        catch (Exception ex)
        {
            _notificationService.Notify(ex.Message);
        }
    }
}