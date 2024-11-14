using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NothingServices.WPFApp.ViewModels.Controls;
using NothingServices.WPFApp.Views.Controls;

namespace NothingServices.WPFApp.Dialogs;

/// <summary>
/// Представление диалогового окна
/// </summary>
public class Dialog : ContentControl
{
    private const string ContentCoverGridName = "PART_ContentCoverGrid";
    private const string OpenStateName = "Open";
    private const string ClosedStateName = "Closed";

    private Grid? ContentCoverGrid { get; set; }

    /// <summary>
    /// Регистрация свойства закрытия диалогового окна по клику вне
    /// </summary>
    public static readonly DependencyProperty CloseOnClickAwayProperty = DependencyProperty.Register(
        nameof(CloseOnClickAway),
        typeof(bool),
        typeof(Dialog),
        new PropertyMetadata(default(bool)));

    /// <summary>
    /// Закрытие диалогового окно по клику вне
    /// </summary>
    public bool CloseOnClickAway
    {
        get => (bool)GetValue(CloseOnClickAwayProperty);
        set => SetValue(CloseOnClickAwayProperty, value);
    }

    /// <summary>
    /// Регистрация свойства контента диалогового окна
    /// </summary>
    public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register(
        nameof(Content),
        typeof(IDialogContentView),
        typeof(Dialog),
        new PropertyMetadata(default(IDialogContentVM)));

    /// <summary>
    /// Контент диалогового окна
    /// </summary>
    public IDialogContentVM? DialogContent
    {
        get => (IDialogContentVM)GetValue(DialogContentProperty);
        set => SetValue(DialogContentProperty, value);
    }

    /// <summary>
    /// Регистрация свойства отображения диалогового окна
    /// </summary>
    public static readonly DependencyProperty OpenProperty = DependencyProperty.Register(
        nameof(Open),
        typeof(bool),
        typeof(Dialog),
        new FrameworkPropertyMetadata(
            default(bool),
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OpenPropertyChangedCallback));

    /// <summary>
    /// Отображение диалогового окна
    /// </summary>
    public bool Open
    {
        get => (bool)GetValue(OpenProperty);
        set => SetValue(OpenProperty, value);
    }

    /// <summary>
    /// Добавление закрытия окна из вне при создании элемента диалогового окна
    /// </summary>
    public override void OnApplyTemplate()
    {
        ContentCoverGrid = GetTemplateChild(ContentCoverGridName) as Grid;
        if (ContentCoverGrid != null)
            ContentCoverGrid.MouseLeftButtonUp += CloseOnMouseLeftButtonUp;

        VisualStateManager.GoToState(this, GetStateName(), false);
        base.OnApplyTemplate();
    }

    private void CloseOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
    {
        if (CloseOnClickAway && DialogContent != null)
            SetCurrentValue(OpenProperty, false);
    }

    private static void OpenPropertyChangedCallback(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
        var dialogHost = (Dialog)dependencyObject;
        VisualStateManager.GoToState(dialogHost, dialogHost.GetStateName(), true);
    }

    private string GetStateName()
    {
        var state = Open ? OpenStateName : ClosedStateName;
        return state;
    }
}