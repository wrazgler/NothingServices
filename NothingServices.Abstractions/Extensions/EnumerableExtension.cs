namespace NothingServices.Abstractions.Extensions;

/// <summary>
/// Методы расширений для коллекций
/// </summary>
public static class EnumerableExtension
{
    /// <summary>
    /// Обход коллекции и выполнение указанного действия для каждого элемента
    /// </summary>
    /// <param name="items">Объект коллекции</param>
    /// <param name="itemAction">Действие для выполнения</param>
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> itemAction)
    {
        foreach (T item in items)
        {
            itemAction(item);
        }
    }
}