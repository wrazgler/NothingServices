namespace NothingServices.WPFApp.ViewModels.Controls;

/// <summary>
/// Данные представления, отображаемое в диалоговом окне
/// </summary>
public interface IDialogContentVM
{
    /// <summary>
    /// Заголовок диалогового окна
    /// </summary>
    public string Title { get; }
}