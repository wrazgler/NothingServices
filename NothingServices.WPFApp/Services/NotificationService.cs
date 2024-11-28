using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using NothingServices.WPFApp.Controls;
using NothingServices.WPFApp.Models;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Сервис отображения уведомлений в пользовательском интерфейсе
/// </summary>
/// <param name="dispatcher">Рабочий поток очереди</param>
/// <param name="duration">Длительность отображения уведомления</param>
public class NotificationService(Dispatcher? dispatcher = null, TimeSpan? duration = null)
    : INotificationService
{
    private readonly Dispatcher _dispatcher = GetDispatcher(dispatcher);
    private readonly NotificatorMessageQueue _notificatorMessageQueue = new();
    private readonly SemaphoreSlim _notificationSemaphore = new(1, 1);
    private readonly TimeSpan _duration = duration ?? new TimeSpan(0, 0, 0, 2);

    /// <summary>
    /// Элемент окна уведомлений
    /// </summary>
    public INotificator? Notificator { get; private set; }

    /// <summary>
    /// Привязать элемент окна уведомлений
    /// </summary>
    /// <param name="notificator">Элемент окна уведомлений</param>
    /// <returns>Действие отвязать элемент окна уведомлений</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка, аргумент является пустой ссылкой
    /// </exception>
    public Action Pair(INotificator notificator)
    {
        Notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));
        return () => Notificator = null;
    }

    /// <summary>
    /// Проверить, что потоки элемента окна уведомлений и сервиса отображения уведомлений совпадают
    /// </summary>
    /// <param name="dispatcher">Поток элемента окна уведомлений</param>
    /// <returns>
    /// Возвращает <see langword="true"/>, если потоки элемента окна уведомлений
    /// и сервиса отображения уведомлений совпадают, и <see langword="false"/>, когда потоки отличаются
    /// </returns>
    public bool ValidateDispatcher(Dispatcher dispatcher)
    {
        var valid = _dispatcher == dispatcher;
        return valid;
    }

    /// <summary>
    /// Отобразить уведомление в пользовательском интерфейсе
    /// </summary>
    /// <param name="message">Текст уведомления</param>
    /// <param name="toolTip">Подсказка уведомления</param>
    public void Notify(string message, string? toolTip = null)
    {
        var notificatorItem = new NotificatorItem()
        {
            Message = message,
            ToolTip = toolTip ?? message,
        };
        _notificatorMessageQueue.AddLast(notificatorItem);
        _dispatcher.InvokeAsync(ShowNext);
    }

    private async Task ShowNext()
    {
        await _notificationSemaphore.WaitAsync();
        try
        {
            if(Notificator == null || !await ValidateNotificator(Notificator))
               throw new NullReferenceException("Элемент окна уведомлений не задан");
            var messageNode = _notificatorMessageQueue.GetFirst()
                ?? throw new NullReferenceException("Очередь уведомлений пуста");
            await Show(Notificator, messageNode.Value);
        }
        finally
        {
            _notificationSemaphore.Release();
        }
    }

    private async Task<bool> ValidateNotificator(INotificator? notificator)
    {
        while (true)
        {
            if (_dispatcher.HasShutdownStarted)
                break;

            if (notificator is { IsLoaded: true, Visibility: Visibility.Visible })
                return true;

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        return false;
    }

    private async Task Show(INotificator notificator, NotificatorItem notificatorItem)
    {
        var content = new ContentControl
        {
            Content = notificatorItem.Message,
            ToolTip = notificatorItem.ToolTip,
        };
        notificator.EventWaitHandle = new ManualResetEvent(!notificator.IsMouseOver);
        notificator.Message = content;
        notificator.Active = true;
        var durationEventWaitHandle = new ManualResetEvent(false);
        StartDuration(_duration, durationEventWaitHandle);
        await Task.Run(() =>
        {
            WaitHandle.WaitAll(
            [
                notificator.EventWaitHandle,
                durationEventWaitHandle
            ]);
        });
        notificator.EventWaitHandle.Set();
        durationEventWaitHandle.Set();
        notificator.Active = false;
        notificator.Message = null;
        durationEventWaitHandle.Dispose();
    }

    private static void StartDuration(TimeSpan duration, EventWaitHandle eventWaitHandle)
    {
        var completionTime = DateTime.Now.Add(duration);
        Task.Run(() =>
        {
            while (true)
            {
                if (DateTime.Now >= completionTime)
                {
                    eventWaitHandle.Set();
                    break;
                }

                if (eventWaitHandle.WaitOne(TimeSpan.Zero))
                    break;
            }
        });
    }

    private static Dispatcher GetDispatcher(Dispatcher? dispatcher)
    {
        dispatcher ??= Dispatcher.FromThread(Thread.CurrentThread);
        if(dispatcher is null)
            throw new InvalidOperationException();
        return dispatcher;
    }
}