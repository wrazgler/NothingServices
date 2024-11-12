using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.Views;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Бизнес логика запуска приложения
/// </summary>
/// <param name="mainWindow">Представление главного окна приложения</param>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
/// <param name="mainWindowVM">Данные представления главного окна</param>
public class StartupService(
    IMainWindowManager mainWindowManager,
    MainWindow mainWindow,
    MainWindowVM mainWindowVM)
{
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly MainWindow _mainWindow = mainWindow;
    private readonly MainWindowVM _mainWindowVM = mainWindowVM;

    /// <summary>
    /// Бизнес логика запуска приложения
    /// </summary>
    public void Start()
    {
        _mainWindowManager.Next(MainWindowContentType.ApiSelectionVM);
        _mainWindow.DataContext = _mainWindowVM;
        _mainWindow.Show();
    }
}