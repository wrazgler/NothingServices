using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.ViewModels.Buttons;

namespace NothingServices.WPFApp.ViewModels.Controls;

/// <summary>
/// Данные представления модели
/// </summary>
public class NothingModelVM : ObservableObject
{
    /// <summary>
    /// Идентификатор модели
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Имя модели
    /// </summary>
    public required string Name { get; init; }

}