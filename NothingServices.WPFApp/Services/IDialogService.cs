using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Сервис работы диалогового окна
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Закрыть диалоговое окно
    /// </summary>
    void CloseDialog();

    /// <summary>
    /// Открыть диалоговое окно
    /// </summary>
    /// <param name="dialogContentVM">Данные представления, отображаемое в диалоговом окне</param>
    /// <param name="dialogContentView">Представление, отображаемое в диалоговом окне</param>
    void OpenDialog<TViewModel, TView>(TViewModel dialogContentVM, TView dialogContentView)
        where TViewModel : IDialogContentVM
        where TView : IDialogControl;
}