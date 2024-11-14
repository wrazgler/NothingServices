using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Commands;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Buttons;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления окна списка моделей
/// </summary>
public class NothingModelsListVM : ObservableObject, IMainWindowContentVM
{
    private readonly IMainWindowManager _mainWindowManager ;
    private readonly CancellationTokenSource _cancellationTokenSource = new(100000);
    private bool _visible = true;
    private ObservableCollection<NothingModelVM>? _nothingModels;

    /// <summary>
    /// Инициализатор данных представления окна списка моделей
    /// </summary>
    /// <param name="backButtonVM">Кнопка вернуться назад</param>
    /// <param name="openCreateNothingModelCommand">Команда открыть представление окна создания новой модели</param>
    /// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
    public NothingModelsListVM(
        IMainWindowManager mainWindowManager,
        BackButtonVM backButtonVM,
        OpenCreateNothingModelCommand openCreateNothingModelCommand)
    {
        _mainWindowManager = mainWindowManager;
        _mainWindowManager.OnNext += OnNext;
        BackButtonVM = backButtonVM;
        CreateButtonVM = new CreateButtonVM(openCreateNothingModelCommand);
    }

    /// <summary>
    /// Нужно ли отображать контент на главном окне
    /// </summary>
    public bool Active
    {
        get => _visible;
        set
        {
            if (_visible == value)
                return;

            _visible = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Список моделей
    /// </summary>
    public ObservableCollection<NothingModelVM>? NothingModels
    {
        get => _nothingModels;
        private set
        {
            if (_nothingModels == value)
                return;

            _nothingModels = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Кнопка вернуться назад
    /// </summary>
    public IButtonVM BackButtonVM { get; }

    /// <summary>
    /// Кнопка создать модель
    /// </summary>
    public IButtonVM CreateButtonVM { get; }

    private void OnNext(MainWindowContentType nextType)
    {
        NothingModels = _mainWindowManager.Strategy != null
            ? GetNothingModels()
            : null;
        Active = nextType == MainWindowContentType.NothingModelsListVM;
    }

    private ObservableCollection<NothingModelVM> GetNothingModels()
    {
        var strategy = _mainWindowManager.Strategy
            ?? throw new NullReferenceException(nameof(_mainWindowManager.Strategy));
        var task = Task.Run(() => strategy.GetNothingModelsAsync(_cancellationTokenSource.Token));
        task.Wait();
        var nothingModels = task.Result;
        return nothingModels;
    }
}