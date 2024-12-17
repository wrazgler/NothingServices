using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.ViewModels.Buttons;

namespace NothingServices.WPFApp.ViewModels.Controls;

/// <summary>
/// Данные представления модели
/// </summary>
public sealed class NothingModelVM : ObservableObject, INothingModelVM
{
    /// <summary>
    /// Идентификатор модели
    /// </summary>
    [Description("Идентификатор модели")]
    public int Id { get; init; }

    /// <summary>
    /// Имя модели
    /// </summary>
    [Description("Имя модели")]
    public required string Name { get; init; }

    /// <summary>
    /// Кнопка удалить модель
    /// </summary>
    public required IButtonVM DeleteButtonVM { get; init; }

    /// <summary>
    /// Кнопка обновить модель
    /// </summary>
    public required IButtonVM UpdateButtonVM { get; init; }
}