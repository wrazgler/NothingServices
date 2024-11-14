namespace NothingServices.WPFApp.ViewModels.Controls;

/// <summary>
/// Данные представления модели
/// </summary>
public interface INothingModelVM
{
    /// <summary>
    /// Идентификатор модели
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Имя модели
    /// </summary>
    public string Name { get; }
}