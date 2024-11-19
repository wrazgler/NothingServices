using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;
using NothingServices.WPFApp.Views;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда открыть представление окна удалить существующую модель
/// </summary>
/// <param name="deleteNothingModelView">Представление удалить модель</param>
/// <param name="deleteNothingModelVMFactory">Фабрика создания объекта данных представления окна удалить модель</param>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class OpenDeleteNothingModelCommand(
    IDeleteNothingModelVMFactory deleteNothingModelVMFactory,
    IDialogService dialogService,
    INotificationService notificationService,
    DeleteNothingModelView deleteNothingModelView)
    : BaseCommand
{
    private readonly DeleteNothingModelView _deleteNothingModelView = deleteNothingModelView;
    private readonly IDeleteNothingModelVMFactory _deleteNothingModelVMFactory = deleteNothingModelVMFactory;
    private readonly IDialogService _dialogService = dialogService;
    private readonly INotificationService _notificationService = notificationService;

    /// <summary>
    /// Проверка возможности выполнить команду открыть представление окна удалить существующую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not NothingModelVM)
            return false;

        return true;
    }

    /// <summary>
    /// Открыть представление окна удалить существующую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    public override void Execute(object? parameter)
    {
        try
        {
            var nothingModelVM = parameter as NothingModelVM
                ?? throw new ArgumentException(parameter?.GetType().Name);
            var deleteNothingModelVM = _deleteNothingModelVMFactory.Create(nothingModelVM);
            _dialogService.OpenDialog(deleteNothingModelVM, _deleteNothingModelView);

        }
        catch (Exception ex)
        {
            _notificationService.Notify(ex.Message);
        }
    }
}