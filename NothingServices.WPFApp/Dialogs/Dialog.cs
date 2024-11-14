using System.Windows;
using System.Windows.Controls;
using NothingServices.WPFApp.ViewModels.Controls;
using NothingServices.WPFApp.Views.Controls;

namespace NothingServices.WPFApp.Dialogs;

/// <summary>
/// Представление диалогового окна
/// </summary>
public class Dialog : ContentControl
{
    /// <summary>
    /// Регистрация свойства контент для окна диалог
    /// </summary>
    public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register(
        nameof(Content),
        typeof(IDialogContentView),
        typeof(Dialog),
        new PropertyMetadata(default(IDialogContentVM)));

    /// <summary>
    /// Получить контент диалогового окна
    /// </summary>
    /// <param name="dialog">Диалоговое окно </param>
    public static IDialogContentVM GetContent(Dialog dialog)
        => (IDialogContentVM)dialog.GetValue(DialogContentProperty);

    /// <summary>
    /// Задать контент диалогового окна
    /// </summary>
    /// <param name="dialog">Диалоговое окно </param>
    /// <param name="dialogContentView">Контент диалогового окно </param>
    public static void SetContent(Dialog dialog, IDialogContentView dialogContentView)
        => dialog.SetValue(DialogContentProperty, dialogContentView);

    /// <summary>
    /// Контент диалогового окна
    /// </summary>
    public object? DialogContent
    {
        get => GetValue(DialogContentProperty);
        set => SetValue(DialogContentProperty, value);
    }
}