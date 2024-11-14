using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Views.Controls;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления диалогового окна
/// </summary>
public class DialogVM : ObservableObject
{
    private bool _open = false;
    private IDialogContentView? _dialogContentView;

    /// <summary>
    /// Нужно ли отображать контент диалогового окна
    /// </summary>
    public bool Open
    {
        get => _open;
        set
        {
            if (_open == value)
                return;

            _open = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Представление, отображаемое в диалоговом окне
    /// </summary>
    public IDialogContentView? DialogContent
    {
        get => _dialogContentView;
        set
        {
            if (_dialogContentView == value)
                return;

            _dialogContentView = value;
            OnPropertyChanged();
        }
    }
}