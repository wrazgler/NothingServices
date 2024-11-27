using NothingServices.WPFApp.Controls;

namespace NothingServices.WPFApp.ViewModels;

/// <summary>
/// Данные представления диалогового окна
/// </summary>
public interface IDialogVM
{
    /// <summary>
    /// Нужно ли отображать контент диалогового окна
    /// </summary>
    public bool Open { get; set; }

    /// <summary>
    /// Представление, отображаемое в диалоговом окне
    /// </summary>
    public IDialogControl? Content { get; set; }
}