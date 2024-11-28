using System.Windows;
using System.Windows.Controls;
using NothingServices.WPFApp.Services;

namespace NothingServices.WPFApp.Controls;

/// <summary>
/// Элемент окна уведомлений
/// </summary>
public interface INotificator
{
    /// <summary>
    /// Активность окна уведомлений
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Загружено ли окно уведомлений
    /// </summary>
    public bool IsLoaded { get; }

    /// <summary>
    /// Наведен ли курсор мыши на окно уведомлений
    /// </summary>
    public bool IsMouseOver { get; }

    /// <summary>
    /// Текст уведомления
    /// </summary>
    public ContentControl? Message { get; set; }

    /// <summary>
    /// Событие синхронизации потоков
    /// </summary>
    public EventWaitHandle? EventWaitHandle { get; set; }

    /// <summary>
    /// Очередь уведомлений
    /// </summary>
    public INotificationService? MessageQueue { get; set; }

    /// <summary>
    /// Видимость окна уведомлений
    /// </summary>
    public Visibility Visibility { get; }
}