using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.ViewModels.Buttons;

namespace NothingServices.WPFApp.ViewModels.Controls;

/// <summary>
/// Данные представления окна удалить модель
/// </summary>
/// <param name="cancelButtonVM">Кнопка отменить удаление</param>
/// <param name="deleteButtonVM">Кнопка удалить модель</param>
/// <param name="nothingModelVM">Данные представления модели</param>
public class DeleteNothingModelVM(
    IButtonVM cancelButtonVM,
    IButtonVM deleteButtonVM,
    INothingModelVM nothingModelVM)
    : ObservableObject, IDialogContentVM
{
    private const string TitleFormat = "Удалить модели: {0}";

    /// <summary>
    /// Идентификатор модели
    /// </summary>
    [Description("Идентификатор модели")]
    public int Id { get; } = nothingModelVM.Id;

    /// <summary>
    /// Заголовок окна удалить модель
    /// </summary>
    public string Title { get; } = string.Format(TitleFormat, nothingModelVM.Name);

    /// <summary>
    /// Кнопка отменить удаление
    /// </summary>
    public IButtonVM CancelButtonVM { get; } = cancelButtonVM;

    /// <summary>
    /// Кнопка удалить модель
    /// </summary>
    public IButtonVM DeleteButtonVM { get; } = deleteButtonVM;
}