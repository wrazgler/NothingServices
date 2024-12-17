using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NothingServices.WPFApp.Controls;

/// <summary>
/// Элемент диалогового окна
/// </summary>
[TemplatePart(Name = DialogContentName, Type = typeof(ContentControl))]
[TemplatePart(Name = ContentCoverGridName, Type = typeof(Grid))]
[TemplateVisualState(GroupName = "PopupStates", Name = OpenStateName)]
[TemplateVisualState(GroupName = "PopupStates", Name = ClosedStateName)]
public sealed class Dialog : ContentControl
{
    private const string ContentCoverGridName = "PART_ContentCoverGrid";
    private const string DialogContentName = "PART_DialogContent";
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
        nameof(DialogContent),
        typeof(IDialogControl),
        typeof(Dialog));

    /// <summary>
    /// Контент диалогового окна
    /// </summary>
    public IDialogControl? DialogContent
    {
        get => (IDialogControl)GetValue(DialogContentProperty);
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

    private void CloseOnMouseLeftButtonUp(object sender, MouseButtonEventArgs eventArgs)
    {
        if (CloseOnClickAway && DialogContent != null)
            SetCurrentValue(OpenProperty, false);
    }

    private static void OpenPropertyChangedCallback(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs eventArgs)
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