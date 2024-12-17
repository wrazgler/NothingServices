namespace NothingServices.WPFApp.Models;

/// <summary>
/// Данные уведомления
/// </summary>
public sealed class NotificatorItem
{
    /// <summary>
    /// Текст уведомления
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Подсказка уведомления
    /// </summary>
    public required string ToolTip { get; set; }
}