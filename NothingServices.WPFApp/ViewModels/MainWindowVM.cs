using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления главного окна
/// </summary>
/// <param name="apiSelectionVM">Данные представления окна выбора внешнего сервиса</param>
/// <param name="appVersionProvider">Провайдер версии приложения</param>
/// <param name="dialogVM">Данные представления диалогового окна</param>
/// <param name="nothingModelsListVM">Данные представления окна списка моделей</param>
/// <param name="notificationService">Сервис отображения уведомлений в пользовательском интерфейсе</param>
public sealed class MainWindowVM(
    IAppVersionProvider appVersionProvider,
    IDialogVM dialogVM,
    INotificationService notificationService,
    ApiSelectionVM apiSelectionVM,
    NothingModelsListVM nothingModelsListVM)
    : ObservableObject, IMainWindowVM
{
    private const string TitleFormat = "Клиент работы с NothingServices - v{0}";

    /// <summary>
    /// Сервис отображения уведомлений в пользовательском интерфейсе
    /// </summary>
    public INotificationService NotificationService { get; } = notificationService;

    /// <summary>
    /// Заголовок главного окна
    /// </summary>
    public string Title { get; } = string.Format(TitleFormat, appVersionProvider.GetVersion());

    /// <summary>
    /// Данные представления окна выбора внешнего сервиса
    /// </summary>
    public ApiSelectionVM ApiSelectionVM { get; } = apiSelectionVM;

    /// <summary>
    /// Данные представления диалогового окна
    /// </summary>
    public IDialogVM DialogVM { get; } = dialogVM;

    /// <summary>
    /// Данные представления окна списка моделей
    /// </summary>
    public NothingModelsListVM NothingModelsListVM { get; } = nothingModelsListVM;
}