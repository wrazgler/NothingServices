using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.ViewModels.Buttons;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления окна выбора внешнего сервиса
/// </summary>
/// <param name="gRpcApiButtonVM">Данные представления кнопки gRpc Api</param>
/// <param name="restApiButtonVM">Данные представления кнопки Rest Api</param>
public class ApiSelectionVM(
    GRpcApiButtonVM gRpcApiButtonVM,
    RestApiButtonVM restApiButtonVM)
    : ObservableObject, IMainWindowContentVM
{
    private bool _visible = true;

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
    /// Данные представления кнопки gRpc Api
    /// </summary>
    public GRpcApiButtonVM GRpcApiButtonVM { get; } = gRpcApiButtonVM;

    /// <summary>
    /// Данные представления кнопки Rest Api
    /// </summary>
    public RestApiButtonVM RestApiButtonVM { get; } = restApiButtonVM;
}