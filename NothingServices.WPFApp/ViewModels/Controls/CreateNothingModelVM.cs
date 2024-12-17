using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NothingServices.WPFApp.ViewModels.Buttons;

namespace NothingServices.WPFApp.ViewModels.Controls;

/// <summary>
/// Данные представления окна создать модель
/// </summary>
/// <param name="cancelButtonVM">Кнопка отменить создание</param>
/// <param name="createButtonVM">Кнопка создать модель</param>
public sealed class CreateNothingModelVM(
    IButtonVM cancelButtonVM,
    IButtonVM createButtonVM)
    : ObservableObject, IDialogContentVM
{
    private string _name = string.Empty;

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
    /// Заголовок окна создать модель
    /// </summary>
    public string Title { get; } = "Введите имя новой модели";

    /// <summary>
    /// Кнопка отменить создание
    /// </summary>
    public IButtonVM CancelButtonVM { get; } = cancelButtonVM;

    /// <summary>
    /// Кнопка создать модель
    /// </summary>
    public IButtonVM CreateButtonVM { get; } = createButtonVM;
}