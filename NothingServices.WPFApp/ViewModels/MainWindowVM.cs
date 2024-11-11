using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления главного окна
/// </summary>
/// <param name="appVersionProvider">Провайдер версии приложения</param>
public class MainWindowVM(IAppVersionProvider appVersionProvider)
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
}