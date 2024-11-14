using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;
using NothingServices.WPFApp.Views.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда открыть представление окна создания новой модели
/// </summary>
/// <param name="createNothingModelView">Представление создать модель</param>
/// <param name="createNothingModelVMFactory">Фабрика создания объекта данных представления окна создать модель</param>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="notificator">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class OpenCreateNothingModelCommand(
    ICreateNothingModelVMFactory createNothingModelVMFactory,
    IDialogService dialogService,
    INotificator notificator,
    CreateNothingModelView createNothingModelView)
    : BaseCommand
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly INotificator _notificator = notificator;
    private readonly CreateNothingModelView _createNothingModelView = createNothingModelView;
    private readonly CreateNothingModelVM _createNothingModelVM = createNothingModelVMFactory.Create();

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
            _dialogService.OpenDialog(_createNothingModelVM, _createNothingModelView);
        }
        catch (Exception ex)
        {
            _notificator.Notificate(ex.Message);
        }
    }
}