using System.Windows.Threading;
using NothingServices.WPFApp.Controls;

namespace NothingServices.WPFApp.Services;

/// <summary>
/// Сервис отображения уведомлений в пользовательском интерфейсе
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Привязать элемент окна уведомлений
    /// </summary>
    /// <param name="notificator">Элемент окна уведомлений</param>
    /// <returns>Действие отвязать элемент окна уведомлений</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка, аргумент является пустой ссылкой
    /// </exception>
    Action Pair(Notificator notificator);

    /// <summary>
    /// Проверить, что потоки элемента окна уведомлений и сервиса отображения уведомлений совпадают
    /// </summary>
    /// <param name="dispatcher">Поток элемента окна уведомлений</param>
    /// <returns>
    /// Возвращает <see langword="true"/>, если потоки элемента окна уведомлений
    /// и сервиса отображения уведомлений совпадают, и <see langword="false"/>, когда потоки отличаются
    /// </returns>
    bool ValidateDispatcher(Dispatcher dispatcher);

    /// <summary>
    /// Отобразить уведомление в пользовательском интерфейсе
    /// </summary>
    /// <param name="message">Текст уведомления</param>
    void Notify(string message);
}