using NothingServices.Abstractions.Exceptions;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда создать новую модель
/// </summary>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
/// <param name="cancellationTokenSource">Объект управления токена отмены</param>
public sealed class CreateCommand(
    IDialogService dialogService,
    IMainWindowManager mainWindowManager,
    INotificationService notificationService,
    CancellationTokenSource? cancellationTokenSource = null)
    : BaseCommand, ICreateCommand
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly CancellationTokenSource _cancellationTokenSource = cancellationTokenSource
                                                                        ?? new CancellationTokenSource();

    /// <summary>
    /// Проверка возможности выполнить команду создать новую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    /// <returns>
    /// Возвращает <see langword="true"/>, если можно выполнить команду и <see langword="true"/>, если нельзя
    /// </returns>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not CreateNothingModelVM createNothingModelVM)
            return false;

        if (string.IsNullOrEmpty(createNothingModelVM.Name.Trim()))
            return false;

        return true;
    }

    /// <summary>
    /// Создать новую модель
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
    /// <exception cref="PropertyRequiredException{T}">
    /// Требуемое поле не задано
    /// </exception>
    public override async void Execute(object? parameter)
    {
        try
        {
            if(parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            var createNothingModelVM = parameter as CreateNothingModelVM
                ?? throw new ArgumentException($"Некорректный тип параметра команды: {parameter.GetType().Name}");
            if (createNothingModelVM.Name == null || string.IsNullOrEmpty(createNothingModelVM.Name.Trim()))
                throw new PropertyRequiredException<CreateNothingModelVM>(nameof(createNothingModelVM.Name));
            var strategy = _mainWindowManager.Strategy
                ?? throw new NullReferenceException("Стратегия работы приложения не задана");
            var nothingModelVM = await strategy.CreateNothingModel(
                createNothingModelVM,
                _cancellationTokenSource.Token);
             _mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
             _dialogService.CloseDialog();
             _notificationService.Notify($"Создано \"{nothingModelVM.Name}\"");
        }
        catch (Exception ex)
        {
            _notificationService.Notify(ex.Message, ex.ToString());
        }
    }
}