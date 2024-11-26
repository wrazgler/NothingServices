using NothingServices.Abstractions.Exceptions;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Factories;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда открыть представление окна обновления существующей модели
/// </summary>
/// <param name="updateNothingModelView">Представление обновить модель</param>
/// <param name="updateNothingModelVMFactory">Фабрика создания объекта данных представления окна обновить модель</param>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public class OpenUpdateNothingModelCommand(
    IUpdateNothingModelVMFactory updateNothingModelVMFactory,
    IDialogService dialogService,
    INotificationService notificationService,
    IUpdateNothingModelView updateNothingModelView)
    : BaseCommand, IOpenUpdateNothingModelCommand
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly IUpdateNothingModelVMFactory _updateNothingModelVMFactory = updateNothingModelVMFactory;
    private readonly IUpdateNothingModelView _updateNothingModelView = updateNothingModelView;

    /// <summary>
    /// Проверка возможности выполнить команду открыть представление окна обновления существующей модели
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

        if (string.IsNullOrEmpty(nothingModelVM.Name.Trim()))
            return false;

        return true;
    }

    /// <summary>
    /// Открыть представление окна обновления существующей модели
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    /// <exception cref="ArgumentException">
    /// Неверный тип входного параметра
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Параметр ссылается на <see langword="null"/>
    /// </exception>
    /// <exception cref="PropertyRequiredException">
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
                throw new PropertyRequiredException(typeof(INothingModelVM), nameof(nothingModelVM.Id));
            if (nothingModelVM.Name == null || string.IsNullOrEmpty(nothingModelVM.Name.Trim()))
                throw new PropertyRequiredException(typeof(INothingModelVM), nameof(nothingModelVM.Name));
            var updateNothingModelVM = _updateNothingModelVMFactory.Create(nothingModelVM);
            _dialogService.OpenDialog(updateNothingModelVM, _updateNothingModelView);
        }
        catch (Exception ex)
        {
            _notificationService.Notify(ex.Message);
        }
    }
}