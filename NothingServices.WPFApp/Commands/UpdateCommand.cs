using NothingServices.Abstractions.Exceptions;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Commands;

/// <summary>
/// Команда обновить существующую модель
/// </summary>
/// <param name="dialogService">Сервис работы диалогового окна</param>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
/// <param name="cancellationTokenSource">Объект управления токена отмены</param>
public sealed class UpdateCommand(
    IDialogService dialogService,
    IMainWindowManager mainWindowManager,
    INotificationService notificationService,
    CancellationTokenSource? cancellationTokenSource = null)
    : BaseCommand, IUpdateCommand
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly CancellationTokenSource _cancellationTokenSource = cancellationTokenSource
                                                                        ?? new CancellationTokenSource();

    /// <summary>
    /// Проверка возможности выполнить команду обновить существующую модель
    /// </summary>
    /// <param name="parameter">Параметр команды</param>
    /// <returns>
    /// Возвращает <see langword="true"/>, если можно выполнить команду и <see langword="true"/>, если нельзя
    /// </returns>
    public override bool CanExecute(object? parameter)
    {
        if (parameter is not UpdateNothingModelVM updateNothingModelVM)
            return false;

        if (updateNothingModelVM.Id == 0)
            return false;

        if (string.IsNullOrEmpty(updateNothingModelVM.Name.Trim()))
            return false;

        return true;
    }

    /// <summary>
    /// Обновить существующую модель
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
            var updateNothingModelVM = parameter as UpdateNothingModelVM
                ?? throw new ArgumentException($"Некорректный тип параметра команды: {parameter.GetType().Name}");
            if (updateNothingModelVM.Id == 0)
                throw new PropertyRequiredException<UpdateNothingModelVM>(nameof(updateNothingModelVM.Id));
            if (updateNothingModelVM.Name == null || string.IsNullOrEmpty(updateNothingModelVM.Name.Trim()))
                throw new PropertyRequiredException<UpdateNothingModelVM>(nameof(updateNothingModelVM.Name));
            var strategy = _mainWindowManager.Strategy
                ?? throw new NullReferenceException("Стратегия работы приложения не задана");
            var nothingModelVM = await strategy.UpdateNothingModel(
                updateNothingModelVM,
                _cancellationTokenSource.Token);
            _mainWindowManager.Next(MainWindowContentType.NothingModelsListVM);
            _dialogService.CloseDialog();
            _notificationService.Notify($"Обновлено \"{nothingModelVM.Name}\"");
        }
        catch (Exception ex)
        {
            _notificationService.Notify(ex.Message, ex.ToString());
        }
    }
}