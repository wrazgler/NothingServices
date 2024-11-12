using CommunityToolkit.Mvvm.ComponentModel;

namespace NothingServices.WPFApp.ViewModels.Controls;

/// <summary>
/// Данные представления удалить модель
/// </summary>
/// <param name="nothingModelVM">Данные представления модели</param>
public class DeleteNothingModelVM(NothingModelVM nothingModelVM) : ObservableObject
{
    private const string TitleFormat = "Удалить модели: {0}";

    /// <summary>
    /// Идентификатор модели
    /// </summary>
    public int Id { get; } = nothingModelVM.Id;

    /// <summary>
    /// Имя модели
    /// </summary>
    public string Name { get; } = nothingModelVM.Name;

    /// <summary>
    /// Заголовок окна удалить модель
    /// </summary>
    public string Title { get; } = string.Format(TitleFormat, nothingModelVM.Name);
}