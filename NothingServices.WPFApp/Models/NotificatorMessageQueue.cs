namespace NothingServices.WPFApp.Models;

/// <summary>
/// Очередь уведомлений
/// </summary>
public class NotificatorMessageQueue
{
    private readonly object _messagesLock = new();
    private readonly LinkedList<string> _nodes = [];

    /// <summary>
    /// Добавляет новое сообщение в конец очереди
    /// </summary>
    /// <param name="message">Новое сообщение</param>
    public void AddLast(string message)
    {
        lock (_messagesLock)
        {
            _nodes.AddLast(message);
        }
    }

    /// <summary>
    /// Получить первое сообщение из очереди
    /// </summary>
    /// <returns>Первое сообщение из очереди</returns>
    public LinkedListNode<string>? GetFirst()
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