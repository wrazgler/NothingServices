using System.Collections.ObjectModel;

namespace NothingServices.WPFApp.Extensions;

/// <summary>
/// Методы расширений для коллекций
/// </summary>
public static class EnumerableExtension
{
    /// <summary>
    /// Преобразовать коллекцию в объект <see cref="ObservableCollection{T}"/>
    /// </summary>
    /// <param name="items">Объект коллекции</param>
    /// <returns>Объект <see cref="ObservableCollection{T}"/>, содержащий элементы исходной коллекции</returns>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
    {
        var observableCollection = new ObservableCollection<T>(items);
        return observableCollection;
    }
}