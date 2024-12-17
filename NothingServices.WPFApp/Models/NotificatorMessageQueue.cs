namespace NothingServices.WPFApp.Models;

/// <summary>
/// Очередь уведомлений
/// </summary>
public sealed class NotificatorMessageQueue
{
    private readonly object _messagesLock = new();
    private readonly LinkedList<NotificatorItem> _nodes = [];

    /// <summary>
    /// Добавляет новое сообщение в конец очереди
    /// </summary>
    /// <param name="notificatorItem">Данные уведомления</param>
    public void AddLast(NotificatorItem notificatorItem)
    {
        lock (_messagesLock)
        {
            _nodes.AddLast(notificatorItem);
        }
    }

    /// <summary>
    /// Получить первое сообщение из очереди
    /// </summary>
    /// <returns>Первое сообщение из очереди</returns>
    public LinkedListNode<NotificatorItem>? GetFirst()
    {
        lock (_messagesLock)
        {
            var node = _nodes.First;
            if (node is null)
                return null;
            _nodes.Remove(node);
            return node;
        }
    }
}