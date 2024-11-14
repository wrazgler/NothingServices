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
public class MainWindowVM(
    IAppVersionProvider appVersionProvider,
    ApiSelectionVM apiSelectionVM,
    DialogVM dialogVM,
    NothingModelsListVM nothingModelsListVM)
    : ObservableObject
{
    private const string TitleFormat = "Клиент работы с NothingServices - v{0}";

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
    public DialogVM DialogVM { get; } = dialogVM;

    /// <summary>
    /// Данные представления окна списка моделей
    /// </summary>
    public NothingModelsListVM NothingModelsListVM { get; } = nothingModelsListVM;
}