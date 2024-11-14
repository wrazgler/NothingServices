using System.Windows.Controls;
using NothingServices.WPFApp.ViewModels;
using NothingServices.WPFApp.ViewModels.Controls;
using NothingServices.WPFApp.Views.Controls;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Сервис работы диалогового окна
/// </summary>
/// <param name="dialogVM">Данные представления диалогового окна</param>
public class DialogService(DialogVM dialogVM) : IDialogService
{
    private readonly DialogVM _dialogVM = dialogVM;

    /// <summary>
    /// Закрыть диалоговое окно
    /// </summary>
    public void CloseDialog()
    {
        _dialogVM.Open = false;
    }

    /// <summary>
    /// Открыть диалоговое окно
    /// </summary>
    /// <param name="dialogContentVM">Данные представления, отображаемое в диалоговом окне</param>
    /// <param name="dialogContentView">Представление, отображаемое в диалоговом окне</param>
    public void OpenDialog<TViewModel, TView>(TViewModel dialogContentVM, TView dialogContentView)
        where TViewModel: IDialogContentVM
        where TView : Control, IDialogContentView
    {
        dialogContentView.DataContext = dialogContentVM;
        _dialogVM.DialogContent = dialogContentView;
        _dialogVM.Open = true;
    }
}