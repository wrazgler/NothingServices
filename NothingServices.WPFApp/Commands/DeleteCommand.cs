using NothingServices.Abstractions.Exceptions;
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
/// <param name="cancellationTokenSource">Объект управления токена отмены</param>
public class DeleteCommand(
    IDialogService dialogService,
    IMainWindowManager mainWindowManager,
    INotificationService notificationService,
    CancellationTokenSource? cancellationTokenSource = null)
    : BaseCommand, IDeleteCommand
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly CancellationTokenSource _cancellationTokenSource = cancellationTokenSource
                                                                        ?? new CancellationTokenSource();

    /// <summary>
    /// Проверка возможности выполнить команду удалить модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    /// <returns>
    /// Возвращает <see langword="true"/>, если можно выполнить команду и <see langword="true"/>, если нельзя
    /// </returns>
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
    /// <exception cref="ArgumentException">
    /// Неверный тип входного параметра
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Параметр ссылается на <see langword="null"/>
    /// </exception>
    /// <exception cref="NullReferenceException">
    /// Ошибка, возникшая при получении стратегии работы приложения
    /// </exception>
    /// <exception cref="PropertyRequiredException">
    /// Требуемое поле не задано
    /// </exception>
    public override async void Execute(object? parameter)
    {
        try
        {
            if(parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            var deleteNothingModelVM = parameter as DeleteNothingModelVM
                ?? throw new ArgumentException($"Некорректный тип параметра команды: {parameter.GetType().Name}");
            if (deleteNothingModelVM.Id == 0)
                throw new PropertyRequiredException(typeof(DeleteNothingModelVM), nameof(deleteNothingModelVM.Id));
            var strategy = _mainWindowManager.Strategy
                ?? throw new NullReferenceException("Стратегия работы приложения не задана");
            var nothingModelVM = await strategy.DeleteNothingModel(
                deleteNothingModelVM,
                _cancellationTokenSource.Token);
            _mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            _dialogService.CloseDialog();
            _notificationService.Notify($"Удалено \"{nothingModelVM.Name}\"");
        }
        catch (Exception ex)
        {
            _notificationService.Notify(ex.Message, ex.ToString());
        }
    }
}