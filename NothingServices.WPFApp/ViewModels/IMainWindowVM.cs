using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления главного окна
/// </summary>
public interface IMainWindowVM
{
    /// <summary>
    /// Сервис отображения уведомлений в пользовательском интерфейсе
    /// </summary>
    public INotificationService NotificationService { get; }

    /// <summary>
    /// Заголовок главного окна
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Данные представления окна выбора внешнего сервиса
    /// </summary>
    public ApiSelectionVM ApiSelectionVM { get; }

    /// <summary>
    /// Данные представления диалогового окна
    /// </summary>
    public IDialogVM DialogVM { get; }

    /// <summary>
    /// Данные представления окна списка моделей
    /// </summary>
    public NothingModelsListVM NothingModelsListVM { get; }
}