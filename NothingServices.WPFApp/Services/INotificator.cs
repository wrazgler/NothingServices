namespace NothingServices.WPFApp.Services;

/// <summary>
/// Сервис отображения уведомлений в пользовательском интерфейсе
/// </summary>
public interface INotificator
{
    /// <summary>
    /// Отобразить уведомление в пользовательском интерфейсе
    /// </summary>
    /// <param name="message">Текст уведомления</param>
    void Notificate(string message);
}