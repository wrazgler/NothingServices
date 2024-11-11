using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.Views;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Бизнес логика запуска приложения
/// </summary>
/// <param name="mainWindow">Представление главного окна приложения</param>
/// <param name="mainWindowVM">Данные представления главного окна</param>
public class StartupService(
    MainWindow mainWindow,
    MainWindowVM mainWindowVM)
{
    private readonly MainWindow _mainWindow = mainWindow;
    private readonly MainWindowVM _mainWindowVM = mainWindowVM;

    /// <summary>
    /// Бизнес логика запуска приложения
    /// </summary>
    public void Start()
    {
        _mainWindow.DataContext = _mainWindowVM;
        _mainWindow.Show();
    }
}