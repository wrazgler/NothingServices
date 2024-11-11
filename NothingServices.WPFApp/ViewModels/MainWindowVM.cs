using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.MainWindowContent;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления главного окна
/// </summary>
/// <param name="apiSelectionVM">Данные представления окна выбора внешнего сервиса</param>
/// <param name="appVersionProvider">Провайдер версии приложения</param>
public class MainWindowVM(
    IAppVersionProvider appVersionProvider,
    ApiSelectionVM apiSelectionVM)
    : ObservableObject
{
    private const string TitleFormat = "Клиент работы с NothingServices - v{0}";
    private readonly string _title = string.Format(TitleFormat, appVersionProvider.GetVersion());

    /// <summary>
    /// Заголовок главного окна
    /// </summary>
    public string Title
    {
        get => _title;
    }

    /// <summary>
    /// Данные представления окна выбора внешнего сервиса
    /// </summary>
    public ApiSelectionVM ApiSelectionVM { get; } = apiSelectionVM;
}