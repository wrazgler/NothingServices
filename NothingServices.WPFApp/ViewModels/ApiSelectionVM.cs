using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Models;
using NothingServices.WPFApp.Services;
using NothingServices.WPFApp.ViewModels.Buttons;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления окна выбора внешнего сервиса
/// </summary>
public class ApiSelectionVM : ObservableObject, IMainWindowContentVM
{
    private readonly IMainWindowManager _mainWindowManager;
    private bool _visible = true;

    /// <summary>
    /// Инициализатор данных представления окна выбора внешнего сервиса
    /// </summary>
    /// <param name="gRpcApiButtonVM">Кнопка gRpc Api</param>
    /// <param name="mainWindowManager">Сервис управление отображением преставления на главном окне</param>
    /// <param name="restApiButtonVM">Кнопка Rest Api</param>
    public ApiSelectionVM(
        IMainWindowManager mainWindowManager,
        IGRpcApiButtonVM gRpcApiButtonVM,
        IRestApiButtonVM restApiButtonVM)
    {
        _mainWindowManager = mainWindowManager;
        _mainWindowManager.OnNext += OnNext;
        GRpcApiButtonVM = gRpcApiButtonVM;
        RestApiButtonVM = restApiButtonVM;
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
    /// Кнопка gRpc Api
    /// </summary>
    public IGRpcApiButtonVM GRpcApiButtonVM { get; }

    /// <summary>
    /// Кнопка Rest Api
    /// </summary>
    public IRestApiButtonVM RestApiButtonVM { get; }

    private void OnNext(MainWindowContentType nextType)
    {
        Active = nextType == MainWindowContentType.ApiSelectionVM;
    }
}