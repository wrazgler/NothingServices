using NothingServices.WPFApp.ViewModels.Buttons;

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

    /// <summary>
    /// Кнопка удалить модель
    /// </summary>
    public IButtonVM DeleteButtonVM { get; }

    /// <summary>
    /// Кнопка обновить модель
    /// </summary>
    public IButtonVM UpdateButtonVM { get; }
}