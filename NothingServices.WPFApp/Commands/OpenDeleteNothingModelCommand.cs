using NothingServices.Abstractions.Exceptions;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда открыть представление окна удалить существующую модель
/// </summary>
/// <param name="deleteNothingModelView">Представление удалить модель</param>
/// <param name="deleteNothingModelVMFactory">Фабрика создания объекта данных представления окна удалить модель</param>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public sealed class OpenDeleteNothingModelCommand(
    IDeleteNothingModelVMFactory deleteNothingModelVMFactory,
    IDialogService dialogService,
    INotificationService notificationService,
    IDeleteNothingModelView deleteNothingModelView)
    : BaseCommand, IOpenDeleteNothingModelCommand
{
    private readonly IDeleteNothingModelView _deleteNothingModelView = deleteNothingModelView;
    private readonly IDeleteNothingModelVMFactory _deleteNothingModelVMFactory = deleteNothingModelVMFactory;
    private readonly IDialogService _dialogService = dialogService;
    private readonly INotificationService _notificationService = notificationService;

    /// <summary>
    /// Проверка возможности выполнить команду открыть представление окна удалить существующую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    /// <returns>
    /// Возвращает <see langword="true"/>, если можно выполнить команду и <see langword="true"/>, если нельзя
    /// </returns>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not INothingModelVM nothingModelVM)
            return false;

        if (nothingModelVM.Id == 0)
            return false;

        return true;
    }

    /// <summary>
    /// Открыть представление окна удалить существующую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    /// <exception cref="ArgumentException">
    /// Неверный тип входного параметра
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Параметр ссылается на <see langword="null"/>
    /// </exception>
    /// <exception cref="PropertyRequiredException{T}">
    /// Требуемое поле не задано
    /// </exception>
    public override void Execute(object? parameter)
    {
        try
        {
            if(parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            var nothingModelVM = parameter as INothingModelVM
                ?? throw new ArgumentException($"Некорректный тип параметра команды: {parameter.GetType().Name}");
            if (nothingModelVM.Id == 0)
                throw new PropertyRequiredException<INothingModelVM>(nameof(nothingModelVM.Id));
            var deleteNothingModelVM = _deleteNothingModelVMFactory.Create(nothingModelVM);
            _dialogService.OpenDialog(deleteNothingModelVM, _deleteNothingModelView);

        }
        catch (Exception ex)
        {
            _notificationService.Notify(ex.Message, ex.ToString());
        }
    }
}