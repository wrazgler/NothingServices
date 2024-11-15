using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.Views;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления диалогового окна
/// </summary>
public class DialogVM : ObservableObject
{
    private bool _open = false;
    private IDialogContentView? _content;

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
    public IDialogContentView? Content
    {
        get => _content;
        set
        {
            if (_content == value)
                return;

            _content = value;
            OnPropertyChanged();
        }
    }
}