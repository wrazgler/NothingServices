using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.Controls;

/// <summary>
/// Элемент окна уведомлений
/// </summary>
[TemplatePart(Name = ActivateStoryboardName, Type = typeof(Storyboard))]
[TemplatePart(Name = DeactivateStoryboardName, Type = typeof(Storyboard))]
public class Notificator : Control
{
    private const string ActivateStoryboardName = "ActivateStoryboard";
    private const string DeactivateStoryboardName = "DeactivateStoryboard";
    private readonly object _waitHandleLock = new();

    private Action? MessageQueueRegistrationCleanUp { get; set; }

    /// <summary>
    /// Событие синхронизации потоков
    /// </summary>
    public EventWaitHandle? EventWaitHandle { get; set; }

    /// <summary>
    /// Длительность анимации появления
    /// </summary>
    public TimeSpan ActivateStoryboardDuration { get; set; }

    /// <summary>
    /// Длительность анимации исчезновения
    /// </summary>
    public TimeSpan DeactivateStoryboardDuration { get; set; }

    /// <summary>
    /// Регистрация свойства текст уведомления
    /// </summary>
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
        nameof(Message),
        typeof(ContentControl),
        typeof(Notificator));

    /// <summary>
    /// Текст уведомления
    /// </summary>
    public ContentControl? Message
    {
        get => (ContentControl?)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    /// <summary>
    /// Регистрация свойства очередь уведомлений
    /// </summary>
    public static readonly DependencyProperty MessageQueueProperty = DependencyProperty.Register(
        nameof(MessageQueue),
        typeof(INotificationService),
        typeof(Notificator),
        new PropertyMetadata(MessageQueuePropertyChangedCallback),
        MessageQueueValidateValueCallback);

    /// <summary>
    /// Очередь уведомлений
    /// </summary>
    public INotificationService? MessageQueue
    {
        get => (INotificationService?)GetValue(MessageQueueProperty);
        set => SetValue(MessageQueueProperty, value);
    }

    private static void MessageQueuePropertyChangedCallback(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs eventArgs)
    {
        var notificator = (Notificator)dependencyObject;
        notificator.MessageQueueRegistrationCleanUp?.Invoke();
        if(eventArgs.NewValue is not INotificationService notificationService)
            return;
        notificator.MessageQueueRegistrationCleanUp = notificationService?.Pair(notificator);;
    }

    private static bool MessageQueueValidateValueCallback(object? value)
    {
        if (value is null || ((INotificationService)value).ValidateDispatcher(Dispatcher.CurrentDispatcher))
            return true;
        throw new ArgumentException("NotificatorMessageQueue must be created by the same thread.", nameof(value));
    }

    /// <summary>
    /// Регистрация свойства активности окна уведомлений
    /// </summary>
    public static readonly DependencyProperty ActiveProperty = DependencyProperty.Register(
        nameof(Active), typeof(bool),
        typeof(Notificator),
        new PropertyMetadata(ActivePropertyChangedCallback));

    /// <summary>
    /// Активность окна уведомлений
    /// </summary>
    public bool Active
    {
        get => (bool)GetValue(ActiveProperty);
        set => SetValue(ActiveProperty, value);
    }

    private static readonly RoutedEvent ActivateEvent = EventManager.RegisterRoutedEvent(
        nameof(ActivateEvent),
        RoutingStrategy.Bubble,
        typeof(RoutedPropertyChangedEventHandler<bool>),
        typeof(Notificator));

    private event RoutedPropertyChangedEventHandler<bool> ActivateEventHandler
    {
        add => AddHandler(ActivateEvent, value);
        remove => RemoveHandler(ActivateEvent, value);
    }

    private static readonly RoutedEvent DeactivateEvent = EventManager.RegisterRoutedEvent(
        nameof(DeactivateEvent),
        RoutingStrategy.Bubble,
        typeof(RoutedEventArgs),
        typeof(Notificator));

    private event RoutedPropertyChangedEventHandler<ContentControl> DeactivateEventHandler
    {
        add => AddHandler(DeactivateEvent, value);
        remove => RemoveHandler(DeactivateEvent, value);
    }

    private static void ActivePropertyChangedCallback(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is not Notificator notificator)
            return;

        var args = new RoutedPropertyChangedEventArgs<bool>((bool)eventArgs.OldValue, (bool)eventArgs.NewValue)
        {
            RoutedEvent = ActivateEvent
        };
        notificator.RaiseEvent(args);

        if ((bool)eventArgs.NewValue)
            return;

        if (notificator.Message is null)
            return;

        var dispatcherTimer = new DispatcherTimer
        {
            Tag = new Tuple<Notificator, ContentControl>(notificator, notificator.Message),
            Interval = notificator.DeactivateStoryboardDuration
        };
        dispatcherTimer.Tick += DeactivateStoryboardDispatcherTimerOnTick;
        dispatcherTimer.Start();
    }

    private static void DeactivateStoryboardDispatcherTimerOnTick(object? sender, EventArgs eventArgs)
    {
        if (sender is not DispatcherTimer dispatcherTimer)
            return;

        dispatcherTimer.Stop();
        dispatcherTimer.Tick -= DeactivateStoryboardDispatcherTimerOnTick;
        var source = (Tuple<Notificator, ContentControl>)dispatcherTimer.Tag;
        var args = new RoutedEventArgs(DeactivateEvent);
        source.Item1.RaiseEvent(args);
    }

    /// <summary>
    /// Добавление длительности анимации появления и исчезновения окна
    /// </summary>
    public override void OnApplyTemplate()
    {
        ActivateStoryboardDuration = GetStoryboardResourceDuration(ActivateStoryboardName);
        DeactivateStoryboardDuration = GetStoryboardResourceDuration(DeactivateStoryboardName);
        MouseEnter += OnMouseEnter;
        MouseLeave += OnMouseLeave;
        base.OnApplyTemplate();
    }

    private TimeSpan GetStoryboardResourceDuration(string resourceName)
    {
        var storyboard = Template.Resources.Contains(resourceName)
            ? Template.Resources[resourceName] as Storyboard
            : null;
        var durationTimeSpan =  storyboard is { Duration.HasTimeSpan: true }
            ? storyboard.Duration.TimeSpan
            : TimeSpan.Zero;
        return durationTimeSpan;
    }

    private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
    {
        EventWaitHandle?.Reset();
    }

    private async void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        if (((UIElement)sender).IsMouseOver)
            return;
        lock (_waitHandleLock)
        {
            EventWaitHandle?.Set();
        }
    }
}