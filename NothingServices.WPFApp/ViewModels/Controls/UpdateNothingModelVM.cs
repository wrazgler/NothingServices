using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.ViewModels.Buttons;

namespace NothingServices.WPFApp.ViewModels.Controls;

/// <summary>
/// Данные представления окна обновить модель
/// </summary>
/// <param name="cancelButtonVM">Кнопка отменить обновление</param>
/// <param name="updateButtonVM">Кнопка обновить модель</param>
/// <param name="nothingModelVM">Данные представления модели</param>
public sealed class UpdateNothingModelVM(
    IButtonVM cancelButtonVM,
    IButtonVM updateButtonVM,
    INothingModelVM nothingModelVM)
    : ObservableObject, IDialogContentVM
{
    private const string TitleFormat = "Введите новое имя модели: {0}";
    private string _name = nothingModelVM.Name;

    /// <summary>
    /// Идентификатор модели
    /// </summary>
    [Description("Идентификатор модели")]
    public int Id { get; } = nothingModelVM.Id;

    /// <summary>
    /// Имя модели
    /// </summary>
    [Description("Имя модели")]
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value)
                return;

            _name = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Подсказка в тестовом блоке
    /// </summary>
    public string Text { get; } = "Введите имя";

    /// <summary>
    /// Заголовок окна обновить модель
    /// </summary>
    public string Title { get; } = string.Format(TitleFormat, nothingModelVM.Name);

    /// <summary>
    /// Кнопка отменить обновление
    /// </summary>
    public IButtonVM CancelButtonVM { get; } = cancelButtonVM;

    /// <summary>
    /// Кнопка обновить модель
    /// </summary>
    public IButtonVM UpdateButtonVM { get; } = updateButtonVM;
}