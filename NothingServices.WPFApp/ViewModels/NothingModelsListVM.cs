using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления окна списка моделей
/// </summary>
/// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
public class NothingModelsListVM(IMainWindowManager mainWindowManager):
    ObservableObject, IMainWindowContentVM
{
    private readonly object _locker = new();
    private readonly IMainWindowManager _mainWindowManager = mainWindowManager;
    private readonly CancellationTokenSource _cancellationTokenSource = new(10000);
    private bool _visible = true;
    private ObservableCollection<NothingModelVM>? _nothingModels;

    /// <summary>
    /// Нужно ли отображать контент на главном окне
    /// </summary>
    public bool Visible
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
    public ObservableCollection<NothingModelVM> NothingModels
    {
        get => _nothingModels ??= GetNothingModels();
        private set
        {
            if (_nothingModels == value)
                return;

            _nothingModels = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<NothingModelVM> GetNothingModels()
    {
        if (_nothingModels != null)
            return _nothingModels;

        lock (_locker)
        {
            var strategy =_mainWindowManager.Strategy
                ?? throw new NullReferenceException(nameof(_mainWindowManager.Strategy));
            var task = strategy.GetNothingModelsAsync(_cancellationTokenSource.Token);
            task.Wait();
            var nothingModels = task.Result;
            return nothingModels;
        }
    }
}